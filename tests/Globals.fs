[<AutoOpen>]
module Globals

open Fable.Core
open Npm

[<Import("*", "assert")>]
let Assert: Node.Assert.IExports = jsNative

let request (app : #obj) = npm.supertest.supertest app

type Node.Http.ServerResponse with
    [<Emit("$0.setHeader($1...)")>]
    member __.setHeader(name: string, value: string) : unit = jsNative
    [<Emit("$0.setHeader($1...)")>]
    member __.setHeader(name: string, value: ResizeArray<string>) : unit = jsNative

[<Emit("undefined")>]
let inline jsUndefined<'T> : 'T = jsNative
