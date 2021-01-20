namespace rec Npm.Types

open System
open Fable.Core
open Fable.Core.JS

module Qs =

    type Array<'T> = Collections.Generic.IList<'T>
    type RegExp = Text.RegularExpressions.Regex

    type [<AllowNullLiteral>] IExports =
        abstract stringify: obj: obj * ?options: IStringifyOptions -> string
        // abstract parse: str: string -> ParsedQs
        abstract parse: str: string * ?options: IParseOptions -> ParsedQs
        // abstract parse: str: string * ?options: IParseOptions -> ParseReturn

    type [<AllowNullLiteral>] ParseReturn =
        [<EmitIndexer>] abstract Item: key: string -> obj with get, set

    type [<AllowNullLiteral>] defaultEncoder =
        [<Emit "$0($1...)">] abstract Invoke: str: obj * ?defaultEncoder: obj * ?charset: string -> string

    type [<AllowNullLiteral>] defaultDecoder =
        [<Emit "$0($1...)">] abstract Invoke: str: string * ?decoder: obj * ?charset: string -> string

    type [<AllowNullLiteral>] IStringifyOptions =
        abstract delimiter: string with get, set
        abstract strictNullHandling: bool with get, set
        abstract skipNulls: bool with get, set
        abstract encode: bool with get, set
        abstract encoder: Func<obj, defaultEncoder, string, IStringifyOptionsEncoder, string> with get, set
        abstract filter: U2<Array<U2<string, float>>, (string -> obj option -> obj option)> with get, set
        abstract arrayFormat: IArrayFormat with get, set
        abstract indices: bool with get, set
        abstract sort: (obj option -> obj option -> float) with get, set
        abstract serializeDate: (DateTime -> string) with get, set
        abstract format: IStringifyOptionsFormat with get, set
        abstract encodeValuesOnly: bool with get, set
        abstract addQueryPrefix: bool with get, set
        abstract allowDots: bool with get, set
        abstract charset: IStringifyOptionsCharset with get, set
        abstract charsetSentinel: bool with get, set

    type [<AllowNullLiteral>] IParseOptions =
        abstract comma: bool with get, set
        abstract delimiter: U2<string, RegExp> with get, set
        abstract depth: float with get, set
        abstract decoder: Func<string, defaultDecoder, string, IStringifyOptionsEncoder, obj> with get, set
        abstract arrayLimit: float with get, set
        abstract arrayFormat: IArrayFormat with get, set
        abstract parseArrays: bool with get, set
        abstract allowDots: bool with get, set
        abstract plainObjects: bool with get, set
        abstract allowPrototypes: bool with get, set
        abstract parameterLimit: float with get, set
        abstract strictNullHandling: bool with get, set
        abstract ignoreQueryPrefix: bool with get, set
        abstract charset: IStringifyOptionsCharset with get, set
        abstract charsetSentinel: bool with get, set
        abstract interpretNumericEntities: bool with get, set

    type [<AllowNullLiteral>] ParsedQs =
        [<EmitIndexer>] abstract Item: key: string -> U4<string, ResizeArray<string>, ParsedQs, ResizeArray<ParsedQs>> option with get, set

    type [<StringEnum(CaseRules.LowerFirst)>] [<RequireQualifiedAccess>] IStringifyOptionsEncoder =
        | Key
        | Value

    type [<StringEnum>] [<RequireQualifiedAccess>] IArrayFormat =
        | Indices
        | Brackets
        | Repeat
        | Comma

    type [<StringEnum>] [<RequireQualifiedAccess>] IStringifyOptionsFormat =
        | [<CompiledName "RFC1738">] RFC1738
        | [<CompiledName "RFC3986">] RFC3986

    type [<StringEnum>] [<RequireQualifiedAccess>] IStringifyOptionsCharset =
        | [<CompiledName "utf-8">] Utf8
        | [<CompiledName "iso-8859-1">] Iso88591
