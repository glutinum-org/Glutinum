[<AutoOpen>]
module rec Npm.Exports

open Fable.Core

type Npm.Types.IExports with
    /// <summary>
    /// When ranges are returned, the array has a "type" property which is the type of
    /// range that is required (most commonly, "bytes"). Each array element is an object
    /// with a "start" and "end" property for the portion of the range.
    ///
    /// Use ParseRangeResult pattern matcher against the result of <c>parseString.Invoke</c> to get a typed result.
    ///
    /// <code lang="fsharp">
    /// let range = parseRange.Invoke(1000, "bytes=0-499")
    ///
    /// match range with
    /// | ParseRangeResult.UnkownError error ->
    ///     printfn "Result of parseRange.Invoke is an unkown error value... If this happen a fix is probably needed in the binding"
    ///
    /// | ParseRangeResult.ResultInvalid error ->
    ///     printfn "Result of parseRange.Invoke is an error of type ResultInvalid"
    ///
    /// | ParseRangeResult.ResultUnsatisfiable error ->
    ///     printfn "Result of parseRange.Invoke is an error of type ResultUnsatisfiable"
    ///
    /// | ParseRangeResult.Range range ->
    ///     // Here you can access your successful result
    ///     Expect.equal range.``type`` "bytes" ""
    ///     Expect.equal range.Count 1 ""
    ///     Expect.equal range.[0].start 0 ""
    ///     Expect.equal range.[0].``end`` 499 ""
    /// </code>
    /// </summary>
    [<Import("default", "range-parser");Emit("$0($1...)")>]
    member __.rangeParser (size: int, str: string, ?options: RangeParser.Types.Options) : RangeParser.Types.ParseRangeResult = jsNative

    /// <summary>
    /// Equivalent to
    /// <code lang="fsharp">
    /// npm.parseRange(
    ///     size,
    ///     str,
    ///     jsOptions<RangeParser.Types.Options>(fun o ->
    ///         o.combine <- true
    ///     )
    /// )
    /// </code>
    /// </summary>
    [<Import("default", "range-parser");Emit("$0($1,$2, { combine : $3 })")>]
    member __.rangeParser (size: int, str: string, combine : bool) : RangeParser.Types.ParseRangeResult = jsNative

