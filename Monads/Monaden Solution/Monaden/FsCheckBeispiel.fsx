#r "..\\packages\\FsCheck.0.9.4.0\\lib\\net40-Client\\FsCheck.dll"
open FsCheck

let ``zweimal List.rev ist Identität``(xs : int list) =
    List.rev (List.rev xs) = xs

Check.Quick ``zweimal List.rev ist Identität``

let ``List.rev ist Identität``(xs : int list) =
    List.rev xs = xs

Check.Quick ``List.rev ist Identität``

type Würfel = internal W of int
let würfel n = 
    if n < 1 || n > 6 
    then failwith "ungültiger Wert"
    else W n

let würfelGen =
    gen {
        let! n = Gen.choose (1,6)
        return würfel n
    }