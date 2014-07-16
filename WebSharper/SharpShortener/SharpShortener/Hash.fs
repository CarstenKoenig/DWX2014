(* Berechnet einen "Hash" für eine Id (Zahl)
*  Dabei wird einfach A,B,C,...,Z,0,1,..,9 als Zeichenmenge
*  angenommen und die Id zu dieser Basis dargestelt 
* 
*  stringToId ermöglicht das berechnen der Id aus einem String
*  (der also nicht im richtigen Typ Hash.T vorliegt)
*  - da die Umwandlung Hash->Id z.B. an einem falschen Zeichen
*     scheitern könnten, sind diese Operationen partiell (option)
*  - da diese Funktion vom Router auch für statische Elemente öfter aufgerufen wird
*    ist eine Abkürzung ohne Exceptions vorgesehen, indem der String auf vorhandensein von
*    '/' geprüft wird
*
*  trotdem sollte natürlich
*  fromId >> toId == id
*  erfüllt werden (d.h. toId ist Invers zu fromId)
*  wir können das garantieren, da Hash.T nur durch eine Id erzeugt werden kann.
*  siehe auch den entsprechenden Test in SharpShortener.Tests.HashTests.fs
*
*)

namespace SharpShortener

open System
open System.Text

open IntelliFactory.WebSharper

type Id = int

[<JavaScript>]
module Hash =

    let private alphabet = List.concat [['a' .. 'z']; ['0'..'9']]
    let private alphabetLen = alphabet.Length
    
    let private indexOf : char -> int =
        let map =
            List.zip alphabet [0..alphabet.Length-1]
            |> Map.ofList
        fun c -> map.[c]

    type T = private T of string
        with 
        override this.ToString() = 
            match this with | T s -> s

    /// stelle die Zahl id mit der Basis alphabet dar
    let fromId (id : Id) : T =
        let mutable output = ""
        let mutable n = id
        while n > 0 do
            output <- output + string alphabet.[int (n % alphabetLen)]
            n <- n / alphabetLen
        T output

    /// versucht eine Id aus der Hash-Darstellung zu berechnen
    let stringToId (hash : string) : Id option =
        if hash.Contains "/" then None else
        try
            List.foldBack 
                (fun c n -> n*alphabetLen + indexOf c) 
                (List.ofSeq hash)
                0
            |> Some
        with _ -> None

    /// berechne die Id aus der Hash-Darstellung
    let toId (T hash : T) : Id =
        stringToId hash |> Option.get 