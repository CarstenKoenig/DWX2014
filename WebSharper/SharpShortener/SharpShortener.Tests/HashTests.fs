namespace SharpShortener.Tests

open System

open Xunit
open FsCheck.Xunit
open FsUnit

open SharpShortener

module ``Hash-Modul Tests`` = 

    [<Property>]
    let ``toId nach fromId ist die Identität``(FsCheck.NonNegativeInt id) =
        (Hash.toId << Hash.fromId) id = id