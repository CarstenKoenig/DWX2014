% F\# im Web
% Carsten König
% DWX 2014

# F\# für das Web mit WebSharper

## Demo

URL shortener 

![App in Action](images/Home.jpg)

## WebSharper

- Webapplikationen komplett in F#
- ASP.NET / Web.API Integration
- VisualStudio, teilweise Xamarin/MonoDevelop
- F# -> Javascript Compiler
- einfachste Client/Server Kommunikation
- HTML Templating
- Sitelets/Pagelets/...

## Sitelets

klassische Server-Part, ähnlich ASP.MVC

Typsichere Links, Routen und Content parametrisiert über einen *Aktions*datentyp.

## HTML Templates

HTML Templating ähnlich zu ASP.NET ASPX- bwz. Razor-Viewengine

## Pagelets

*Widgets*/Controls für den Client, Aufbau einer HTML/JavaScript Seite mit einer
F\# - DSL

# Sitelets

## Aktionstyp

Entspricht den Seiten/Aktionen:

```fsharp
type Aktion =
    | Home
    | Goto of SharpShortener.Id 
    | Stats
```

## Router

Bildet *Aktionen* auf URLs und URLs auf *Aktionen* ab.

```fsharp
let router : Router<Aktion> =
    [
        [
            Aktion.Home, "/"
            Aktion.Stats, "/Stats"
        ] |> Router.Table
        Router.New
            (fun request -> ... )
            (fun aktion -> ... )
    ] |> Router.Sum
```

## Controller

Ordnet *Aktionen* **Content** zu.

```fsharp
let controller : Controller<Aktion> =
    {
        Handle = function
            | Aktion.Home    -> homeContent
            | Aktion.Goto id -> gotoContent id
            | Aktion.Stats   -> statsContent
    }
```

## Content

Erzeugt aus einem `Context` (Request, Resourcen, ...) eine HTML Antwort.

```fsharp
let homeContent =
    Skin.WithTemplate "#-Shortener" <| fun ctx ->
        [
            Div [new Controls.Shorten()]
            A [HRef (ctx.Link Aktion.Stats)] -< [Text "Statistiken"]
        ]
```

# HTML Templates

## Template

```html
<!DOCTYPE html>
<head>
    <title>${title}</title>
</head>
<body>
    <div class="templateContent">
        <div data-replace="body"></div>
    </div>
</body>
```

## Holes

- `${Name}`: Platzhalter der mit Text ersetzt wird
- `data-replace="Name"`: Platzhalter-Element das komplett ersetzt wird
- `data-hole="Namd"`: Platzhalter-Element, wird mit Kindelementen gefüllt.

## Skin

Verarbeitet ein Template und wandelt es in Content um.

```fsharp
    type Page =
        {
            Title : string
            Body : list<Content.HtmlElement>
        }

    let MainTemplate =
        Content.Template<Page>("~/Main.html")
            .With("title", fun x -> x.Title)
            .With("body", fun x -> x.Body)
```

## Hilfsfunktionen

```fsharp
let WithTemplate title body : Content<Aktion> =
    Content.WithTemplate MainTemplate <| fun context ->
        {
            Title = title
            Body = body context
        }
```

damit

```fsharp
let homeContent =
    Skin.WithTemplate "#-Shortener" <| fun ctx ->
        [
            Div [new Controls.Shorten()]
            A [HRef (ctx.Link Aktion.Stats)] -< [Text "Statistiken"]
        ]
```

# Pagelets

## Funktionsweise

Werden auf dem Server erstellt, an den Client übertragen und dort gerendert bzw.
in die Seite eingefügt.

Ein wenig wie die Views in MVC.

## HTML Kombinatoren

Für die meisten HTML Elemente gibt es entsprechende F\# Funktionen/Kombinatoren.

```fsharp
Div [Width "200px"] -< [
    H1 [Text "Hallo DWX"]
]
```

## HTML Events

Einige Ereignisse werden direkt von den Kombinatoren unterstützt und können mit 

```fsharp
let ( |>! ) x f = f x; x
```

einfach angehängt werden

```fsharp
Div [
    Input [Attr.Type "button"; Attr.Value "drück mal!"]
    |>! OnClick (fun element eventArguments ->
        element.Value <- ";)")
]
```

## HTML Events

*low-level* Handler

Können über die `Dom`-Eigenschaft angehängt werden:

```fsharp
btn.Dom.AddEventListener(
    "click", 
    (fun () -> JavaScript.Alert("Click")), false)
```

normalerweise sollte auf `OnAfterRender` gewartet werden:

```fsharp
let btn = 
    Button [Text "Test"]
    |>! OnAfterRender (fun el ->
        el.Dom.AddEventListener(
            "click", 
            (fun () -> JavaScript.Alert("Click")),
            false))
```