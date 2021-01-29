[<AutoOpen>]
module Globals

open Fable.Core

[<Import("*", "assert")>]
let Assert: Node.Assert.IExports = jsNative

type Node.Http.ServerResponse with
    [<Emit("$0.setHeader($1...)")>]
    member __.setHeader(name: string, value: string) : unit = jsNative
    [<Emit("$0.setHeader($1...)")>]
    member __.setHeader(name: string, value: ResizeArray<string>) : unit = jsNative
