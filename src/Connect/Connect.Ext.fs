[<AutoOpen>]
module Connect.Ext

open Fable.Core

type Node.Http.IExports with
    [<Emit("$0.createServer($1)")>]
    member __.createServer(_ : Connect.CreateServer.Server) : Node.Http.Server = jsNative
