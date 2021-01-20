namespace rec RangeParser.Types

open System
open Fable.Core

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
