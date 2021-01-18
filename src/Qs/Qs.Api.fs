namespace rec Qs

open Fable.Core
open Fable.Core.JsInterop

[<AutoOpen>]
module Api =

    let [<Import("default","qs")>] qs: Types.IExports = jsNative
