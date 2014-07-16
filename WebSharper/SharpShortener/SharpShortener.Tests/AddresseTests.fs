namespace SharpShortener.Tests

open System

open Xunit
open FsCheck.Xunit
open FsUnit

open SharpShortener

module ``Addresse-Modul Tests`` = 

    [<Fact>]
    let ``'http://localhost:55958/XH4' ist ein interner Link mit Id 39155``() =
        "http://localhost:55958/XH4"
        |> Addresse.linkFromString "http://localhost:55958/"
        |> should equal (Some <| Addresse.link 39155)

    [<Fact>]
    let ``'localhost:55958/XH4' ist ein interner Link mit Id 39155``() =
        "localhost:55958/XH4"
        |> Addresse.linkFromString "http://localhost:55958/"
        |> should equal (Some <| Addresse.link 39155)

    [<Fact>]
    let ``'www.google.de' ist eine externe Url``() =
        "www.google.de"
        |> Addresse.externFromString "http://localhost:55958/"
        |> Option.map Addresse.externToUrl
        |> should equal (Url.fromString "www.google.de")
        

module ``RequestAddresse-Modul Tests`` = 
    [<Fact>]
    let ``'http://localhost:55958/XH4' ist ein interner Link mit Id 39155``() =
        Uri("http://localhost:55958/XH4")
        |> RequestAddresse.linkFromUri
        |> should equal (Some <| Addresse.link 39155)