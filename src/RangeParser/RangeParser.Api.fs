namespace rec RangeParser

open Fable.Core
open Fable.Core.JsInterop

[<AutoOpen>]
module Api =

    let [<Import("default","range-parser")>] parseRange: Types.IExports = jsNative
