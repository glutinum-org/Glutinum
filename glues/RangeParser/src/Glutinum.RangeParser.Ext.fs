namespace Glutinum.RangeParser

open Fable.Core

module internal Interop =

    [<Emit("typeof $0")>]
    let internal jsTypeOf _ : string = jsNative

[<RequireQualifiedAccess>]
module ParseRangeResult =

    let (|UnkownError|ResultInvalid|ResultUnsatisfiable|Range|) (result : RangeParser.ParseRangeResult) : Choice<RangeParser.Errored, unit, unit, RangeParser.Ranges> =
        if Interop.jsTypeOf result = "number" then
            let error = unbox<int> result

            if error = -1 then
                ResultUnsatisfiable
            else if error = -2 then
                ResultInvalid
            else
                UnkownError (unbox result)
        else
            Range (unbox result)
