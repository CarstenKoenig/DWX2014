namespace SharpShortener

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html

[<JavaScript>]
module ShortenPagelet =

    let requestShortenedUrl (cont : Url.T option -> unit) (url : Url.T) =
        async {
            let! url = Remoting.ShortenUrl url
            return cont url
        }
        |> Async.Start


    [<Inline "$0.Body.select()">]
    let selectText(e) = ()

    let enableElement (e : Element) enable =
        if not enable
        then e.SetAttribute ("disabled","")
        else e.RemoveAttribute "disabled"

    let setText (e : Element) text =
        e.Text <- text

    let setValue (f : 'a -> string) (e : Element) (value : 'a) =
        e.Value <- f value

    let getValue (f : string -> 'a) (e : Element) =
        fun () -> f e.Value

    let wireUp (button : Element) (input : Element) =

        let getUrl = getValue Url.fromString input 
        let setUrl = setValue (fun (u : Url.T) -> string u) input
        let emptyUrl = setValue (fun () -> "") input
        let enableAll() =
            enableElement button true
            enableElement input true
        let disableAll() =
            enableElement button false
            enableElement input false
            
        let onTextChange () =
            match getUrl() with
            | Some _ ->
                enableElement button true
                setText button "kürzen"
            | None ->
                enableElement button false
                setText button "..."

        let onClick () =
            match getUrl() with
            | Some url ->
                let cont linkUrl =
                    enableAll()
                    match linkUrl with
                    | Some url -> setUrl url
                                  selectText input
                    | None     -> emptyUrl ()
                    onTextChange()
                disableAll()
                url |> requestShortenedUrl cont
            | None -> ()

        OnClick (fun _ _ -> onClick()) button
        OnChange (fun _ -> onTextChange()) input
        OnKeyUp (fun _ _ -> onTextChange()) input
        enableElement button false

    let Main () =
        let input = Input [Text ""]
        let button = Button [Text "Kürzen"]
        wireUp button input
        Div [
            input
            button
        ] -< [ Attr.Class "eingabe" ]
