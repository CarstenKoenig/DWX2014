(* Darstellung von URLs
*  die URLs werden mit Hilfe einer simplen RegEx geparst
*  damit kann der gleiche Code sowohl auf der Server als
*  auch auf der ClientSeite benutzt werden
*  
*  eine Url.T kann nur erzeugt werden (fromString), falls
*  der String eine "gültige" URL darstellt 
*  (naja, gültig laut der Regex)
*)

namespace SharpShortener

open System
open System.Text
open System.Net

open IntelliFactory.WebSharper

/// implementiere JavaScript-Ausgabe für Regex.IsMatch
[<Proxy "System.Text.RegularExpressions.Regex, System">]
module RegexProxy =
    [<Inline "(new RegExp($1)).test($0)">]
    let IsMatch (input : string, pattern : string) = false

[<JavaScript>]
module Url =

    let private pattern = "^(https?:\/\/)?(www\.)?(\w+.?)+$"

    let isValidUrl (url : string) =
        RegularExpressions.Regex.IsMatch(url, pattern)

    let private withProtocol (url : string) : string =
        if url.StartsWith "http://" || url.StartsWith "https://"
        then url
        else "http://" + url

    let private withoutProtocol (url : string) : string =
        url.Replace("http://","")
           .Replace("https://","");

    let private withoutWWW (url : string) : string =
        url.Replace("www.","")

    type T = private T of string
        with 
        override this.ToString() = 
            match this with | T s -> s
       
    let fromString (url : string) : T option =
        if isValidUrl (url.ToLower())
        then Some (T <| withProtocol url)
        else None

    /// gib nur den Teil hinter 'http://www.' zurück
    /// dient dazu die Darstellung für die Statistik
    /// zu verkürzen
    let nameOnly (T url) : string =
        (string url).ToLower()
        |> withoutProtocol
        |> withoutWWW
