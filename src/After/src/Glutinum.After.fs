// ts2fable 0.8.0
module rec Glutinum.After

open Fable.Core

[<Import("default", "after")>]
let e : IExports = jsNative

type [<AllowNullLiteral>] IExports =
    [<Emit("$0($1...)")>]
    abstract after : count : int * callback : 'Callback * ?errCallback : (obj option -> unit) -> 'T
