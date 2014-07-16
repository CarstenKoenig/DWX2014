namespace Monaden

open System

module Generator =

    type GenM<'a> = internal Gen of (unit -> 'a)

    let sample (Gen g : GenM<'a>) : 'a =
        g()

    let returnM (a : 'a) : GenM<'a> =
        Gen <| fun () -> a

    let bind (g : GenM<'a>) (f : 'a -> GenM<'b>) : GenM<'b> =
        sample g |> f

    let (>>=) m f =
        bind m f

    type GenBuilder internal () =
        member x.Bind(m, f) = m >>= f
        member x.Return(v) = returnM v
        member x.ReturnFrom(v) = v
        member x.Delay(f) = f ()

    let gen = GenBuilder()

    let intGen : GenM<int> =
        let rnd = System.Random(int System.DateTime.Now.Ticks)
        Gen <| fun () -> rnd.Next()

    let choose (von, bis) : GenM<int> =
        let l = bis - von + 1
        gen {
            let! i = intGen
            return von + (abs i % l)
        }

    module Beispiel =

        type Würfel =
            internal W of int with
            override this.ToString() =
                match this with | W n -> sprintf "%d (w6)" n

        let würfel n = 
            if n < 1 || n > 6 
            then failwith "ungültiger Wert"
            else W n

        let würfelGen =
            gen {
                let! n = choose (1,6)
                return würfel n
            }

        let zweiWürfelGen =
            gen {
                let! w1 = würfelGen
                let! w2 = würfelGen
                return (w1, w2)
            }

        let würfle2() : (Würfel*Würfel) =
            sample zweiWürfelGen