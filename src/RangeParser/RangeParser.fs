namespace rec RangeParser

open System
open Fable.Core

[<AutoOpen>]
module Api =
    [<Import("default", "range-parser")>]
    let rangeParser : IExports = jsNative

type IExports =
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
    abstract rangeParser : size: int * str: string * ?options: RangeParser.Options -> RangeParser.ParseRangeResult

    /// <summary>
    /// Equivalent to
    /// <code lang="fsharp">
    /// npm.parseRange(
    ///     size,
    ///     str,
    ///     jsOptions&lt;RangeParser.Types.Options>(fun o -&gt;
    ///         o.combine &lt;- true
    ///     )
    /// )
    /// </code>
    /// </summary>
    [<Import("default", "range-parser");Emit("$0($1,$2, { combine : $3 })")>]
    abstract rangeParser : size: int * str: string * combine : bool -> RangeParser.ParseRangeResult

module RangeParser =

    type Array<'T> = Collections.Generic.IList<'T>

    type [<AllowNullLiteral>] Ranges =
        inherit Array<Range>
        abstract ``type``: string with get, set

    type [<AllowNullLiteral>] Range =
        abstract start: int with get, set
        abstract ``end``: int with get, set

    type [<AllowNullLiteral>] Options =
        /// The "combine" option can be set to `true` and overlapping & adjacent ranges
        /// will be combined into a single range.
        abstract combine: bool with get, set

    /// <summary>
    /// Alias type representing an error case. The runtime representation of this type is always equal to -1
    ///
    /// Please use ParseRangeResult.ResultUnsatisfiable pattern matcher againt the result of <c>parseString.Invoke</c> to get a typed result
    /// </summary>
    type ResultUnsatisfiable =
        int

    /// <summary>
    /// Alias type representing an error case. The runtime representation of this type is always equal to -2
    ///
    /// Please use ParseRangeResult.ResultUnsatisfiable pattern matcher againt the result of <c>parseString.Invoke</c> to get a typed result
    /// </summary>
    type ResultInvalid =
        int

    /// <summary>
    /// Alias type representing an error case
    /// </summary>
    type Errored =
        U2<ResultUnsatisfiable, ResultInvalid>

    /// <summary>
    /// Alias type representing the result of <c>parseString.Invoke</c>
    /// </summary>
    type ParseRangeResult =
        U2<Errored, Ranges>
