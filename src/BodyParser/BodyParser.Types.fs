namespace rec BodyParser.Types

open System
open Fable.Core
open Fable.Core.JS
open Node

type NextHandleFunction = Connect.Types.CreateServer.NextHandleFunction

// let [<Import("*","module")>] bodyParser: BodyParser.IExports = jsNative

// type [<AllowNullLiteral>] IExports =
//     abstract bodyParser: ?options: obj -> NextHandleFunction

type [<AllowNullLiteral>] IExports =
    /// Returns middleware that only parses json and only looks at requests
    /// where the Content-Type header matches the type option.
    abstract json: ?options: OptionsJson -> NextHandleFunction
    /// Returns middleware that parses all bodies as a Buffer and only looks at requests
    /// where the Content-Type header matches the type option.
    abstract raw: ?options: Options -> NextHandleFunction
    /// Returns middleware that parses all bodies as a string and only looks at requests
    /// where the Content-Type header matches the type option.
    abstract text: ?options: OptionsText -> NextHandleFunction
    /// Returns middleware that only parses urlencoded bodies and only looks at requests
    /// where the Content-Type header matches the type option
    abstract urlencoded: ?options: OptionsUrlencoded -> NextHandleFunction

type [<AllowNullLiteral>] Options =
    /// When set to true, then deflated (compressed) bodies will be inflated; when false, deflated bodies are rejected. Defaults to true.
    abstract inflate: bool option with get, set
    /// Controls the maximum request body size. If this is a number,
    /// then the value specifies the number of bytes; if it is a string,
    /// the value is passed to the bytes library for parsing. Defaults to '100kb'.
    abstract limit: U2<float, string> option with get, set
    /// The type option is used to determine what media type the middleware will parse
    abstract ``type``: U3<string, ResizeArray<string>, (Http.IncomingMessage -> obj option)> option with get, set
    /// The verify option, if supplied, is called as verify(req, res, buf, encoding),
    /// where buf is a Buffer of the raw request body and encoding is the encoding of the request.
    abstract verify: req: Http.IncomingMessage * res: Http.ServerResponse * buf: Buffer * encoding: string -> unit

type [<AllowNullLiteral>] OptionsJson =
    inherit Options
    /// The reviver option is passed directly to JSON.parse as the second argument.
    abstract reviver: key: string * value: obj option -> obj option
    /// When set to `true`, will only accept arrays and objects;
    /// when `false` will accept anything JSON.parse accepts. Defaults to `true`.
    abstract strict: bool option with get, set

type [<AllowNullLiteral>] OptionsText =
    inherit Options
    /// Specify the default character set for the text content if the charset
    /// is not specified in the Content-Type header of the request.
    /// Defaults to `utf-8`.
    abstract defaultCharset: string option with get, set

type [<AllowNullLiteral>] OptionsUrlencoded =
    inherit Options
    /// The extended option allows to choose between parsing the URL-encoded data
    /// with the querystring library (when `false`) or the qs library (when `true`).
    abstract extended: bool option with get, set
    /// The parameterLimit option controls the maximum number of parameters
    /// that are allowed in the URL-encoded data. If a request contains more parameters than this value,
    /// a 413 will be returned to the client. Defaults to 1000.
    abstract parameterLimit: float option with get, set
