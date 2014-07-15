namespace SharpShortener.Tests

open System

open Xunit
open FsCheck.Xunit
open FsUnit

open SharpShortener

module ``Url-Modul Tests`` = 

    [<Fact>]
    let ``leerrer String ist keine valide Url``() =
        ""
        |> Url.isValidUrl
        |> should equal false

    [<Fact>]
    let ``alert('') ist keine valide Url``() =
        "alert('')"
        |> Url.isValidUrl
        |> should equal false

    [<Fact>]
    let ``http://www.google.de ist eine valide Url``() =
        "http://www.google.de"
        |> Url.isValidUrl
        |> should equal true

    [<Fact>]
    let ``www.google.de ist eine valide Url``() =
        "www.google.de"
        |> Url.isValidUrl
        |> should equal true

    [<Fact>]
    let ``'http://localhost:55958/xH4' ist eine valide Url``() =
        "http://localhost:55958/xH4"
        |> Url.isValidUrl
        |> should equal true

    [<Fact>]
    let ``'localhost:55958/xH4' ist eine valide Url``() =
        "localhost:55958/xH4"
        |> Url.isValidUrl
        |> should equal true

    [<Fact>]
    let ``http://www.google.de wird so übernommen``() =
        "http://www.google.de"
        |> Url.fromString
        |> Option.map string
        |> should equal <| Some ("http://www.google.de")

    [<Fact>]
    let ``https://www.google.de wird so übernommen``() =
        "https://www.google.de"
        |> Url.fromString
        |> Option.map string
        |> should equal <| Some ("https://www.google.de")

    [<Fact>]
    let ``www.google.de wird ergänzt zu http://www.google.de``() =
        "www.google.de"
        |> Url.fromString
        |> Option.map string
        |> should equal <| Some ("http://www.google.de")

    [<Fact>]
    let ``nameOnly von 'https://www.google.de' liefert 'google.de'``() =
        "https://www.google.de"
        |> Url.fromString
        |> Option.map Url.nameOnly
        |> should equal <| Some ("google.de")

    [<Fact>]
    let ``nameOnly von 'http://www.google.de' liefert 'google.de'``() =
        "http://www.google.de"
        |> Url.fromString
        |> Option.map Url.nameOnly
        |> should equal <| Some ("google.de")

    [<Fact>]
    let ``nameOnly von 'www.google.de' liefert 'google.de'``() =
        "www.google.de"
        |> Url.fromString
        |> Option.map Url.nameOnly
        |> should equal <| Some ("google.de")

    [<Fact>]
    let ``nameOnly von 'google.de' liefert 'google.de'``() =
        "google.de"
        |> Url.fromString
        |> Option.map Url.nameOnly
        |> should equal <| Some ("google.de")

    [<Fact>]
    let ``nameOnly von 'HTTP://www.Google.De' liefert 'google.de'``() =
        "HTTP://www.Google.de"
        |> Url.fromString
        |> Option.map Url.nameOnly
        |> should equal <| Some ("google.de")

