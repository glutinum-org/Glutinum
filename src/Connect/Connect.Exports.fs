[<AutoOpen>]
module rec Npm.Exports

open Fable.Core

type Npm.Types.IExports with
    [<Import("default", "connect"); Emit("$0()")>]
    member __.connect () : Connect.Types.CreateServer.Server = jsNative
