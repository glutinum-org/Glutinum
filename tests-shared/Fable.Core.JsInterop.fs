module Fable.Core.JsInterop

open Fable.Core

[<Emit("delete $0")>]
let inline jsDelete<'T> (v : 'T) : unit = jsNative
