namespace Monaden

open System

module Maybe =

    type Maybe<'a> =
      | Just of 'a
      | Nothing

    let returnM (a : 'a) : Maybe<'a> =
        Just a

    let bind (m : Maybe<'a>) (f : 'a -> Maybe<'b>) : Maybe<'b> =
        match m with
        | Just a  -> f a
        | Nothing -> Nothing

    let (>>=) m f =
        bind m f

    type MaybeBuilder internal () =
        member x.Bind(m, f) = m >>= f
        member x.Return(v) = returnM v
        member x.ReturnFrom(v) = v
        member x.Delay(f) = f ()

    let maybe = MaybeBuilder()