[<AutoOpen>]
module rec Npm.Exports

open Fable.Core

type Npm.Types.IExports with
    [<Import("default", "connect"); Emit("$0()")>]
    member __.connect () : Types.Connect.CreateServer.Server = jsNative
