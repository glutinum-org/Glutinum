[<AutoOpen>]
module rec Npm.Exports

open Fable.Core

type Npm.Types.IExports with
    [<Import("default", "mime")>]
    member __.mime with get () : Types.Mime.IExports = jsNative
