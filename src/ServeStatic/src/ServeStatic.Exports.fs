namespace ServeStatic

[<AutoOpen>]
module Exports =

    open Fable.Core

    [<Import("default", "serve-static")>]
    let serveStatic : Types.IExports = jsNative
