namespace SuperAgent

open Fable.Core

[<AutoOpen>]
module Api =

    let [<Import("default","supertest")>] superagent: Types.Request.SuperAgentStatic = jsNative
