namespace rec Mime

open Fable.Core
open Fable.Core.JsInterop

[<AutoOpen>]
module Api =

    [<Import("default", "mime")>]
    let mime : Types.IExports = jsNative

    [<Import("default", "mime/Mime")>]
    let Mime : Types.MimeStatic = jsNative
