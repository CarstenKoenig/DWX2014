(* Verwaltet die gespeicherten Links
*  über ihre Id und vergibt diese
*
*  erzeugt außerdem Zugriffstatistiken in der Form
*  "(externe Url, Anzahl aufgerufen)"
* 
*  Gespeichert wird hier vereinfacht nur im Speicher
*  um leere order arg kurze Hashwerte zu vermeiden
*  ist der erste Id-Wert 4711
*)

namespace SharpShortener

open System
open System.Collections.Generic

module Storage =

    open Addresse

    let startAt = 4711

    let storageLock = obj()
    let private _forward = Dictionary<Id, (Extern * int ref)>()
    let private _back = Dictionary<Extern, Id>()

    let store (adr : Extern) : Id =
        lock storageLock (fun () -> 
            match _back.TryGetValue adr with
            | true, lId -> lId
            | false, _  -> 
                let lId = _forward.Count + startAt
                _forward.Add (lId, (adr, ref 0))
                _back.Add (adr, lId)
                lId)

    let resolve (lId : Id) : Extern option =
        lock storageLock (fun () ->
            match _forward.TryGetValue lId with
            | true, (res, hitC) -> incr hitC
                                   Some res
            | false, _          -> None)

    let stats () : (Url.T * int) array =
        lock storageLock (fun () ->
            _forward.Values
            |> Seq.map (fun (res, hitC) -> (externToUrl res, !hitC))
            |> Seq.toArray
        )

    // ein paar Beispieleinträge
    do
        let dwxId = store (Addresse.externFromString "" "http://www.developer-week.de").Value
        let webSharperId = store (Addresse.externFromString "" "http://www.websharper.com").Value
        let fsharpId = store (Addresse.externFromString "" "http://fsharp.org").Value
        let googleId = store (Addresse.externFromString "" "http://www.google.de").Value

        [1..5] |> List.iter (fun _ -> resolve dwxId |> ignore)
        [1..7] |> List.iter (fun _ -> resolve webSharperId |> ignore)
        [1..3] |> List.iter (fun _ -> resolve fsharpId |> ignore)
        [1..6] |> List.iter (fun _ -> resolve googleId |> ignore)
             
        