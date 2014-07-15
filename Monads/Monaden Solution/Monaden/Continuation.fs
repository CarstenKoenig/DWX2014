#nowarn "40"

namespace Monaden

open System

module Continuation =

    type Cont<'a>  = 'a -> unit 

    type ContM<'a> = internal { run : Cont<'a> -> unit }

    let mkCont (f : Cont<'a> -> unit) : ContM<'a> = 
        { run = f }    

    let runWith (f : 'a -> 'b) (m : ContM<'a>) = 
        m.run (f >> ignore)

    let returnM (a : 'a) : ContM<'a> =
        mkCont (fun f -> f a)

    let bind (m : ContM<'a>) (f : 'a -> ContM<'b>) : ContM<'b> =
        fun (c : Cont<'b>) ->
            m.run <| fun a -> (f a).run c
        |> mkCont

    let (>>=) (m : ContM<'a>) (f : 'a -> ContM<'b>) : ContM<'b> =
        bind m f

    type ContBuilder() =
        member x.Bind(m, f) = m >>= f
        member x.Return(v) = returnM v
        member x.ReturnFrom(v) = v
        member x.Delay(f) = f ()

    let cont = ContBuilder()    

    module Controls =
        open System.Windows.Forms

        let delay (span : TimeSpan) : ContM<unit> =
            fun f ->
                let timer = new Timer()
                timer.Interval <- int span.TotalMilliseconds
                timer.Tick.Add (fun _ -> timer.Dispose(); f())
                timer.Start()
            |> mkCont

        let awaitClickEvent (b : Button) : ContM<unit> =
            fun f -> 
                let rec d : IDisposable = 
                    b.Click.Subscribe (fun _ -> d.Dispose(); f())
                ()
            |> mkCont

        let enable (c : Control) =
            c.Enabled <- true

        let disable (c : Control) =
            c.Enabled <- false

        let text (txt : TextBox) =
            txt.Text

        let clear (txt : TextBox) = 
            txt.Clear()
            txt.Focus() |> ignore

        let message (msg : string) =
            MessageBox.Show msg
            |> ignore

        let textOnOk (txt : TextBox, ok : Button) : ContM<string> =
            cont {
                clear txt
                do! awaitClickEvent ok
                return text txt
            }

        let aktion (box : GroupBox, enableB : Button, txt : TextBox, okB : Button) =
            let rec loop() =
                cont {
                    disable box
                    do! awaitClickEvent enableB
                    disable enableB
                    enable box
                    let! msg = textOnOk (txt, okB)
                    disable box
                    do! delay <| TimeSpan.FromSeconds 2.0
                    enable enableB
                    message msg
                    return! loop()
                }
            loop() |> runWith ignore
            
        let verzögertesHallo() =
            cont {
               do! delay <| TimeSpan.FromSeconds 2.0
               return "Hello, World!"
            } |> runWith MessageBox.Show