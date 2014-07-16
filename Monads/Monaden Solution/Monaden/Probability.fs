namespace Monaden

open System

module Probability =

    type Wahrscheinlichkeit = double
    type Verteilung<'a> = ('a * Wahrscheinlichkeit) list
    type Ereignis<'a> = 'a -> bool

    let printVerteilung (v : Verteilung<'a>) =
        let negate x = -x
        v |> List.sortBy (snd >> negate) |> List.iter (fun (a,p) -> printfn "%A: %.2f%%" a (p * 100.0))

    let sicher (a : 'a) : Verteilung<'a> =
        [a, 1.0]

    let gleichVerteilung (ls : 'a list) = 
        let ws = 1.0 / float (List.length ls)
        List.map (fun l -> (l, ws)) ls

    let wsEreignis (e : Ereignis<'a>) (vs : Verteilung<'a>) : Wahrscheinlichkeit =
        vs |> List.filter (fst >> e)
           |> List.sumBy snd

    let (>?) a b = wsEreignis b a

    [<AutoOpen>]
    module internal Operations =

        let normalize (v : Verteilung<'a>) =
            let dict = new System.Collections.Generic.Dictionary<_,_>()
            let get a = if dict.ContainsKey a then dict.[a] else 0.0
            let add (a,p) = dict.[a] <- get a + p
            v |> List.iter add
            dict |> Seq.map (fun kvp -> (kvp.Key, kvp.Value)) 
                 |> List.ofSeq

    [<AutoOpen>]
    module Monad =
        let returnM (a : 'a) : Verteilung<'a> =
            sicher a

        let bind (v : Verteilung<'a>) (f : 'a -> Verteilung<'b>) : Verteilung<'b> =
            [ for (a,p) in v do
              for (b,p') in f a do
              yield (b, p*p')
            ] |> normalize

        let (>>=) f m =
            bind f m

        type VertBuilder internal () =
            member x.Bind(m, f) = m >>= f
            member x.Return(v) = returnM v
            member x.ReturnFrom(v) = v
            member x.Delay(f) = f ()

    let vert = Monad.VertBuilder()    

    let rec takeN (v : Verteilung<'a>) (n : int) : Verteilung<'a list> =
        vert {
            if n <= 0 then return [] else
            let! wert = v
            let! rest = takeN v (n-1)
            return (wert::rest)
        }

    module Beispiel =
        
        let würfel : Verteilung<int> =
            gleichVerteilung [1..6]

        let nWürfel = takeN würfel

        let ``WS: mindestens 2 Sechser in 4 Würfeln`` =
            let hatZweiSecher (augen : int list) : bool = 
                augen |> List.filter ((=) 6)
                      |> (fun l -> List.length l >= 2)
            nWürfel 4 >? hatZweiSecher

        let ``Mensch ärgere dich (nicht?)`` =
            vert {
                let! w1 = würfel
                if w1 = 6 then return "ich komm raus" else
                let! w2 = würfel
                if w2 = 6 then return "ich komm raus" else
                let! w3 = würfel
                if w3 = 6 then return "ich komm raus" else
                return "grrr"
            }

        let ``Verluste beim Risiko (3 gegen 2)`` =
            vert {
                let! offensive = nWürfel 3
                let! defensive = nWürfel 2
                let defensivVerluste = 
                    List.zip (offensive |> List.sort |> List.tail)
                             (defensive |> List.sort)
                    |> List.sumBy (fun (o,d) -> if d >= o then 0 else 1)
                return sprintf "%d:%d" (2-defensivVerluste) defensivVerluste
            } |> printVerteilung
