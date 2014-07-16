(* Es gibt zwei Arten von Addressen
*  - externe URL (die also nicht mit der Basis-Addresse
*                 der Applikation beginnen)
*  - interne Links (die durch eine interne Id gegeben sind)
*
* die Methoden und Typen hier stellen sicher, dass
* die beiden Arten zuverlässig unterschieden werden
* das geschieht immer anhand einer Basisadresse der Applikation
* (z.B. "http://localhost:1111/")
* da Addressen der Form [basisAdr][Hashwert] (z.B. "http://localhost:1111/XH4")
* eine verlinkte URL mit Hashwert ("XH4") gennzeichnen sollen
*)

namespace SharpShortener

open System
open System.Text
open System.Net

open IntelliFactory.WebSharper

[<JavaScript>]
module Addresse =

    type Link = private Link of Id
    type Extern = private Extern of Url.T

    let link (id : Id) =
        Link id

    let getId = function
        | Link id -> id

    let private tryGetHash baseAdr (url : Url.T) =
        let url = string url
        if url.StartsWith baseAdr 
        then Some (url.Substring(baseAdr.Length)) else
        let url = "http://" + url
        if url.StartsWith baseAdr
        then Some (url.Substring(baseAdr.Length))
        else None

    let linkFromUrl (baseAdr : string) (url : Url.T) : Link option =
        tryGetHash baseAdr url
        |> Option.bind Hash.stringToId
        |> Option.map Link

    let linkFromString (baseAdr : string) (url : string) : Link option =
        Url.fromString url
        |> Option.bind (linkFromUrl baseAdr)

    let linkToUrl (baseUri : string) (Link id) = 
        baseUri + string (Hash.fromId id) 
        |> Url.fromString 
        |> Option.get

    let isLink (baseAdr : string) url : bool =
        linkFromUrl baseAdr url
        |> Option.isSome

    let externFromUrl (baseAdr : string) (url : Url.T) : Extern option =
        if isLink baseAdr url 
        then None 
        else Some (Extern url)

    let externFromString (baseAdr : string) (url : string) : Extern option =
        Url.fromString url
        |> Option.bind (externFromUrl baseAdr)

    let externToUrl (Extern url : Extern) : Url.T =
        url

module RequestAddresse =

    let baseAddress (url : Uri) =
        let reqPath = url.OriginalString
        let absPath = url.AbsolutePath
        reqPath.Substring(0,reqPath.Length-absPath.Length+1)

    /// liest die Basisaddresse für die Link-Erkennung aus
    /// der URI aus und versucht den Link auszulesen
    let linkFromUri (uri : Uri) : Addresse.Link option =
        let reqPath = uri.OriginalString
        let absPath = uri.AbsolutePath
        let baseAdr = reqPath.Substring(0,reqPath.Length-absPath.Length+1)
        Url.fromString reqPath
        |> Option.bind (Addresse.linkFromUrl baseAdr)
