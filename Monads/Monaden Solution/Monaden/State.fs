namespace Monaden

open System

module State =

    type StateM<'s,'a> = internal { runState : 's -> ('a*'s) }

    let mkState f = { runState = f }

    let returnM (a : 'a) : StateM<'s,'a> =
        mkState <| fun s -> (a, s)

    let bind (f : StateM<'s,'a>) (g : 'a -> StateM<'s,'b>) : StateM<'s,'b> =
        fun s ->
            let (a,s') = f.runState s
            (g a).runState s'
        |> mkState

    let (>>=) m f =
        bind m f

    let (>=>) f g =
        fun a -> f a >>= g

    type StateBuilder internal () =
        member x.Bind(m, f) = m >>= f
        member x.Return(v) = returnM v
        member x.ReturnFrom(v) = v
        member x.Delay(f) = f ()

    let state = StateBuilder()    

    let eval (s : 's) (m : StateM<'s,'a>) : 'a =
        m.runState s
        |> fst

    let get() : StateM<'s,'s> =
        fun s -> (s,s)
        |> mkState

    let set (s : 's) : StateM<'s, unit> =
        fun _ -> ((), s)
        |> mkState

    module Beispiel =
        
        let nextFib : StateM<int*int, int> =
            state {
                let! l1,l2 = get()
                let akt = l1+l2
                do! set (l2,akt)
                return akt
            }

        let test () : int =
            state {
                let! _ = nextFib
                let! _ = nextFib
                return! nextFib
            } |> eval (1,1) // = 5