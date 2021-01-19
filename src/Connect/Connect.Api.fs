namespace Connect

open Fable.Core

[<AutoOpen>]
module Api =

    let [<Import("default","connect")>] connect () : Types.CreateServer.Server = jsNative
