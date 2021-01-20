[<AutoOpen>]
module rec Npm.Types

open Fable.Core


type Npm.Types.IExports with
    [<Import("*", "body-parser")>]
    member __.BodyParser with get () : BodyParser.Types.IExports = jsNative
