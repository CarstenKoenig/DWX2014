namespace SharpShortener

open IntelliFactory.Html
open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Sitelets

type Aktion =
    | Home
    | Goto of SharpShortener.Id 
    | Stats
    | Feedback

/// Verwendung des Pagelets (ShortenPaglet.fs)
module Controls =

    [<Sealed>]
    type Shorten() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            ShortenPagelet.Main() :> _

    [<Sealed>]
    type Stats() =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            StatsPagelet.Main() :> _

    [<Sealed>]
    type Feedback (baseAdr) =
        inherit Web.Control()

        [<JavaScript>]
        override __.Body =
            FeedbackPagelet.Main baseAdr :> _


/// HTML Template und co.
module Skin =
    open System.Web

    type Page =
        {
            Title : string
            Body : list<Content.HtmlElement>
        }

    let MainTemplate =
        Content.Template<Page>("~/Main.html")
            .With("title", fun x -> x.Title)
            .With("body", fun x -> x.Body)

    let WithTemplate title body : Content<Aktion> =
        Content.WithTemplate MainTemplate <| fun context ->
            {
                Title = title
                Body = body context
            }


/// Definiert das Sitelet der Applikation
module Site =

    /// Stellt Bijektion zwischen Url's und Aktionen her
    let router : Router<Aktion> =
        [
            [
                Aktion.Home, "/"
                Aktion.Stats, "/Stats"
                Aktion.Feedback, "/Feedback"
            ] |> Router.Table
            Router.New
                (fun request ->
                    match RequestAddresse.linkFromUri request.Uri with
                    | Some link -> Some (Goto (Addresse.getId link))
                    | _ -> None)
                (function
                | Goto id ->
                    Addresse.link id
                    |> Addresse.linkToUrl "/"
                    |> fun url -> Location (string url, System.UriKind.Relative) 
                    |> Some
                | _ -> None)
        ] |> Router.Sum

    let homeContent =
        Skin.WithTemplate "#-Shortener" <| fun ctx ->
            [
                Div [new Controls.Shorten()]
                P [ A [HRef (ctx.Link Aktion.Stats)] -< [Text "Statistiken"] ]
                P [ A [HRef (ctx.Link Aktion.Feedback)] -< [Text "Feedback"] ]
            ]

    let gotoContent id =
        match Storage.resolve id with
        | Some res ->
            Content.RedirectTemporaryToUrl 
                (System.UriBuilder (string (Addresse.externToUrl res)))
                .Uri.AbsoluteUri

        | _ -> homeContent

    let statsContent =
        Skin.WithTemplate "#-Shortener" <| fun ctx ->
            [
                Div [new Controls.Stats()]
                P [ A [HRef (ctx.Link Aktion.Home)] -< [Text "Home"] ]
                P [ A [HRef (ctx.Link Aktion.Feedback)] -< [Text "Feedback"] ]
            ]

    let feedbackContent =
        Skin.WithTemplate "#-Shortener" <| fun ctx ->
            let baseAdr = RequestAddresse.baseAddress ctx.Request.Uri
            [
                Div [new Controls.Feedback (baseAdr)]
                P [ A [HRef (ctx.Link Aktion.Home)] -< [Text "Home"] ]
                P [ A [HRef (ctx.Link Aktion.Stats)] -< [Text "Statistiken"] ]
            ]

    /// Ordnet den Aktionen Inhalt zu
    let controller : Controller<Aktion> =
        {
            Handle = function
                | Aktion.Home     -> homeContent
                | Aktion.Goto id  -> gotoContent id
                | Aktion.Stats    -> statsContent
                | Aktion.Feedback -> feedbackContent
        }

    /// das Sitelet der Applikation
    let MainSitelet =
        {
            Router = router
            Controller = controller
        }

[<Sealed>]
type Website() =
    interface IWebsite<Aktion> with
        member this.Sitelet = Site.MainSitelet
        member this.Actions = [Home; Stats]

type Global() =
    inherit System.Web.HttpApplication()

    member g.Application_Start(sender: obj, args: System.EventArgs) =
        ()

[<assembly: Website(typeof<Website>)>]
do ()
