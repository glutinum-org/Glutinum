[<AutoOpen>]
module rec Npm.Exports

open Fable.Core

type Npm.Types.IExports with
    [<Import("*", "body-parser")>]
    member __.bodyParser with get () : BodyParser.Types.IExports = jsNative
