namespace Monaden.Tests

open System

module ``FsCheck Beispiel`` =

    open Xunit
    open FsCheck.Xunit

    [<Property>]
    let ``zweimal List.rev ist Identität``(xs : int list) =
        List.rev (List.rev xs) = xs


    [<Property>]
    let ``List.rev ist Identität``(xs : int list) =
        List.rev xs = xs
