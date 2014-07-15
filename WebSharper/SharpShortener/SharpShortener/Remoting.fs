namespace SharpShortener

open IntelliFactory.WebSharper

module Remoting =

    let getBaseAddress() =
        System.Web.HttpContext.Current.Request.Url.AbsoluteUri

    [<Remote>]
    let ShortenUrl (url : Url.T) : Async<Url.T option> =
        async {
            let baseUri = getBaseAddress()
            return
                Addresse.externFromUrl baseUri url
                |> Option.map (fun res ->
                    let id = Storage.store res
                    Addresse.linkToUrl baseUri (Addresse.link id))
        }

    [<Remote>]
    let getStats () =
        Storage.stats()
