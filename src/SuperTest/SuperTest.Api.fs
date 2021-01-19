namespace SuperTest

open Fable.Core

[<AutoOpen>]
module Api =

    let [<Import("default","supertest")>] supertest: Types.IExports = jsNative
