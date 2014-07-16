namespace SharpShortener

open IntelliFactory.WebSharper
open IntelliFactory.WebSharper.Html
open IntelliFactory.WebSharper.Formlet

[<JavaScript>]
module FeedbackPagelet =

    type Email = Email of string
    type Nachricht = string

    type MeldeGrund =
        | LinkBroken
        | OffensiveContent

    type Auswahl =
        | LinkMelden
        | Kontakt

    type Ergebnis =
        | Link of (Id * MeldeGrund)
        | Nachricht of (Email * Nachricht)

    let FeedbackFormlet baseAdr : Formlet<Ergebnis> =
        let email =
            Controls.Input ""
            |> Validator.IsEmail "Bitte geben Sie eine gültige Email an"
            |> Formlet.Map Email
            |> Enhance.WithTextLabel "Email"
            |> Enhance.WithValidationIcon
        let nachricht =
            Controls.TextArea ""
            |> Validator.IsNotEmpty "Bitte eine Nachricht eingeben"
            |> Enhance.WithTextLabel "Nachricht"
            |> Enhance.WithValidationIcon
        let kontakt =
            Formlet.Yield (fun email nachricht -> Nachricht (email, nachricht))
            <*> email
            <*> nachricht

        let linkId =
            Controls.Input ""
            |> Formlet.Map (Addresse.linkFromString baseAdr)
            |> Validator.Is Option.isSome "bitte geben Sie eine gekürzte URL ein"
            |> Formlet.Map (Option.get >> Addresse.getId)
            |> Enhance.WithTextLabel "Link"
            |> Enhance.WithValidationIcon
        let grund =
            ["Link funktioniert nicht", LinkBroken; "unangebrachter Inhalt", OffensiveContent ]
            |> Formlet.Controls.Select 0
            
        let melden =
            Formlet.Yield (fun grund id -> Link (id,grund))
            <*> grund
            <*> linkId
 
        Formlet.Do {
            let! contactTypeFormlet =
                ["Link melden", melden; "Kontakt", kontakt ]
                |> Formlet.Controls.Select 0
            return! contactTypeFormlet
        }
        |> Enhance.WithCustomSubmitButton      
            {Enhance.FormButtonConfiguration.Default with Label = Some "Abschicken"}
        |> Enhance.WithFormContainer
        |> Enhance.WithCssClass "feedback"

    let Main baseAdr =
        FeedbackFormlet baseAdr 
        |> Formlet.Run (
            function
            | Link (_,_) -> JavaScript.Alert "Vielen Dank, wir werden uns um den Link kümmern"
            | Nachricht (Email email, _) -> JavaScript.Alert ("Vielen Dank für Ihr Feedback " + email))
