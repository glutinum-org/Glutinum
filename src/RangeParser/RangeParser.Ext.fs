namespace RangeParser

open Fable.Core

[<AutoOpen>]
module Ext =

    [<Emit("typeof $0")>]
    let jsTypeOf _ : string = jsNative

    [<RequireQualifiedAccess>]
    module ParseRangeResult =

        let (|UnkownError|ResultInvalid|ResultUnsatisfiable|Range|) (result : Types.ParseRangeResult) : Choice<Types.Errored, unit, unit, Types.Ranges> =
            if jsTypeOf result = "number" then
                let error = unbox<int> result

                if error = -1 then
                    ResultUnsatisfiable
                else if error = -2 then
                    ResultInvalid
                else
                    UnkownError (unbox result)
            else
                Range (unbox result)
