[<AutoOpen>]
module rec Npm.Exports

open Fable.Core

type Npm.Types.IExports with
    [<Import("*", "body-parser")>]
    member __.bodyParser with get () : Types.BodyParser.IExports = jsNative
