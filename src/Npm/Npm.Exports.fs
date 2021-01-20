namespace Npm

open Fable.Core


[<AutoOpen>]
module Exports =

    [<Global>]
    let npm : Types.IExports = jsNative
