[<AutoOpen>]
module Globals

open Fable.Core
open Fable.Core.JS
open Glutinum.SuperTest

[<Import("*", "assert")>]
let Assert: Node.Assert.IExports = jsNative

let request (app : #obj) = supertest.supertest app

type Node.Http.ServerResponse with
    [<Emit("$0.setHeader($1...)")>]
    member __.setHeader(name: string, value: string) : unit = jsNative
    [<Emit("$0.setHeader($1...)")>]
    member __.setHeader(name: string, value: ResizeArray<string>) : unit = jsNative

[<Emit("undefined")>]
let inline jsUndefined<'T> : 'T = jsNative

[<Emit("void 0")>]
let inline returnNothingHack<'T> : 'T = jsNative

type NumberConstructor with
    [<Emit("$0($1...)")>]
    member __.Create(v : obj) = jsNative

[<Emit("typeof $0")>]
let internal jsTypeOf _ : string = jsNative
