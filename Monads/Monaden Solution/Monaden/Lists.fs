namespace Monaden

open System

module Lisss =

    let ohne x =
        List.filter ((<>) x)

    let delete d =
        function
        | [] -> []
        | (x::xs) when x = d
            -> xs
        | (_::xs) 
            -> xs

    /// im Talk vorgestellter Variante
    /// permutationen [1;1;1] -> [[1]; [1]; [1]]
    let rec permutationen = function
      | [] -> [[]]
      | xs -> [ for x in xs do
                let rest = xs |> ohne x
                for xs' in permutationen rest do
                yield (x::xs') ]

    /// ergibt einsichtigere Ergebnise (auf Performancekosten)
    /// permutationen [1;1;1] -> [[1;1;1]]
    let rec permutationen' = function
      | [] -> [[]]
      | xs -> [ for x in List.ofSeq (Seq.distinct xs) do
                let rest = xs |> delete x
                for xs' in permutationen' rest do
                yield (x::xs') ]