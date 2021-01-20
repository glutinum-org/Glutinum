[<AutoOpen>]
module rec Npm.Types

open Fable.Core

type Npm.Types.IExports with
    [<Import("default", "mime")>]
    member __.mime with get () : Mime.Types.IExports = jsNative

    // [<Import("default", "mime/Mime")>]
    // member __.Mime with get () : Mime.Types.MimeStatic = jsNative


// namespace rec Mime

// open Fable.Core
// open Fable.Core.JsInterop

// // [<AutoOpen>]
// // module Api =

// //     [<Import("default", "mime")>]
// //     let mime : Types.IExports = jsNative

// //     [<Import("default", "mime/Mime")>]
// //     let Mime : Types.MimeStatic = jsNative
