// ts2fable 0.8.0
module rec ExpressServeStaticCore

open System
open ExpressServeStaticCore
open ExpressServeStaticCore
open Fable.Core
open Fable.Core.JS
open Node
open Npm

type Array<'T> = System.Collections.Generic.IList<'T>
type Error = System.Exception
type RegExp = System.Text.RegularExpressions.Regex

type EventEmitter = Events.EventEmitter
type RangeParserOptions = Types.RangeParser.Options
type RangeParserResult = Types.RangeParser.ParseRangeResult
type RangeParserRanges = Types.RangeParser.Ranges
type ParsedQs = Types.Qs.ParsedQs

module Express =

    type [<AllowNullLiteral>] Request =
        interface end

    type [<AllowNullLiteral>] Response =
        interface end

    type [<AllowNullLiteral>] Application =
        interface end

type Query =
    ParsedQs

type [<AllowNullLiteral>] NextFunction =
    [<Emit "$0($1...)">] abstract Invoke: ?err: obj -> unit
    /// "Break-out" of a router by calling {next('router')};
    [<Emit "$0('router')">] abstract Invoke_router: unit -> unit

type [<AllowNullLiteral>] Dictionary<'T> =
    [<EmitIndexer>] abstract Item: key: string -> 'T with get, set

type [<AllowNullLiteral>] ParamsDictionary =
    [<EmitIndexer>] abstract Item: key: string -> string with get, set
    [<EmitIndexer>] abstract Item: key: int -> string with get, set

type ParamsArray =
    ResizeArray<string>

type Params =
    U2<ParamsDictionary, ParamsArray>

type RequestHandler =
    RequestHandler<ParamsDictionary, obj option, obj option, ParsedQs, Dictionary<obj option>>


type RequestHandler<'P> =
    RequestHandler<'P, obj option, obj option, ParsedQs, Dictionary<obj option>>

type RequestHandler<'P, 'ResBody> =
    RequestHandler<'P, 'ResBody, obj option, ParsedQs, Dictionary<obj option>>

type RequestHandler<'P, 'ResBody, 'ReqBody> =
    RequestHandler<'P, 'ResBody, 'ReqBody, ParsedQs, Dictionary<obj option>>

type RequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery> =
    RequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery, Dictionary<obj option>>

type [<AllowNullLiteral>] RequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals when 'Locals :> Dictionary<obj option>> =
    [<Emit "$0($1...)">] abstract Invoke: req: Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals> * res: Response<'ResBody, 'Locals> * next: NextFunction -> unit
    //System.Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>

type Adapter =
    static member inline RequestHandler (f : System.Func<Request, Response, NextFunction, unit>) : RequestHandler =
        unbox f
        
    static member inline RequestHandler (f : System.Func<Request, Response, unit>) : RequestHandler =
        unbox f

type ErrorRequestHandler =
    ErrorRequestHandler<ParamsDictionary, obj option, obj option, ParsedQs, Dictionary<obj option>>

type ErrorRequestHandler<'P> =
    ErrorRequestHandler<'P, obj option, obj option, ParsedQs, Dictionary<obj option>>

type ErrorRequestHandler<'P, 'ResBody> =
    ErrorRequestHandler<'P, 'ResBody, obj option, ParsedQs, Dictionary<obj option>>

type ErrorRequestHandler<'P, 'ResBody, 'ReqBody> =
    ErrorRequestHandler<'P, 'ResBody, 'ReqBody, ParsedQs, Dictionary<obj option>>

type ErrorRequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery> =
    ErrorRequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery, Dictionary<obj option>>

type ErrorRequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals when 'Locals :> Dictionary<obj option>> =
    System.Func<obj option, Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>

type PathParams =
    U3<string, RegExp, Array<U2<string, RegExp>>>

type RequestHandlerParams =
    RequestHandlerParams<ParamsDictionary, obj option, obj option, ParsedQs, Dictionary<obj option>>

type RequestHandlerParams<'P> =
    RequestHandlerParams<'P, obj option, obj option, ParsedQs, Dictionary<obj option>>

type RequestHandlerParams<'P, 'ResBody> =
    RequestHandlerParams<'P, 'ResBody, obj option, ParsedQs, Dictionary<obj option>>

type RequestHandlerParams<'P, 'ResBody, 'ReqBody> =
    RequestHandlerParams<'P, 'ResBody, 'ReqBody, ParsedQs, Dictionary<obj option>>

type RequestHandlerParams<'P, 'ResBody, 'ReqBody, 'ReqQuery> =
    RequestHandlerParams<'P, 'ResBody, 'ReqBody, 'ReqQuery, Dictionary<obj option>>

type RequestHandlerParams<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals when 'Locals :> Dictionary<obj option>> =
    U3<RequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, ErrorRequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Array<U2<RequestHandler<'P>, ErrorRequestHandler<'P>>>>

type IRouterMatcher<'T> =
    IRouterMatcher<'T, obj option>

type [<AllowNullLiteral>] IRouterMatcher<'T, 'Method> =
    [<Emit "$0($1...)">] abstract Invoke: path: PathParams * [<ParamArray>] handlers: Array<RequestHandler> -> 'T
    [<Emit "$0($1...)">] abstract Invoke: path: PathParams * [<ParamArray>] handlers: Array<RequestHandlerParams> -> 'T
    [<Emit "$0($1...)">] abstract Invoke: path: PathParams * subApplication: Application -> 'T

type [<AllowNullLiteral>] IRouterHandler<'T> =
    [<Emit "$0($1...)">] abstract Invoke: [<ParamArray>] handlers: RequestHandler[] -> 'T
    [<Emit "$0($1...)">] abstract Invoke: [<ParamArray>] handlers: RequestHandlerParams[] -> 'T
    [<Emit "$0($1...)">] abstract Invoke: [<ParamArray>] handlers: Array<RequestHandler> -> 'T
    [<Emit "$0($1...)">] abstract Invoke: [<ParamArray>] handlers: Array<RequestHandlerParams> -> 'T

type [<AllowNullLiteral>] IRouter =
    inherit RequestHandler
    /// Map the given param placeholder `name`(s) to the given callback(s).
    ///
    /// Parameter mapping is used to provide pre-conditions to routes
    /// which use normalized placeholders. For example a _:user_id_ parameter
    /// could automatically load a user's information from the database without
    /// any additional code,
    ///
    /// The callback uses the samesignature as middleware, the only differencing
    /// being that the value of the placeholder is passed, in this case the _id_
    /// of the user. Once the `next()` function is invoked, just like middleware
    /// it will continue on to execute the route, or subsequent parameter functions.
    ///
    ///       app.param('user_id', function(req, res, next, id){
    ///         User.find(id, function(err, user){
    ///           if (err) {
    ///             next(err);
    ///           } else if (user) {
    ///             req.user = user;
    ///             next();
    ///           } else {
    ///             next(new Error('failed to load user'));
    ///           }
    ///         });
    ///       });
    abstract param: name: string * handler: Func<Request, Response, NextFunction, obj, string, unit> -> IRouter
    abstract param: name: ResizeArray<string> * handler: Func<Request, Response, NextFunction, obj, string, unit> -> IRouter
    abstract param: name: string * regexp : RegExp -> 'T
    /// Alternatively, you can pass only a callback, in which case you have the opportunity to alter the app.param()
    abstract param: callback: (string -> RegExp -> RequestParamHandler) -> IRouter
    /// Special-cased "all" method, applying the given route `path`,
    /// middleware, and callback to _every_ HTTP method.
    // abstract all: IRouterMatcher<IRouter, string> with get, set
    abstract all: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>) array -> 'T
    abstract all: path: string * [<ParamArray>] handlers: (Func<Request, Response, NextFunction, unit>) array -> 'T
    abstract all: path: string * [<ParamArray>] handlers: (Func<Request, Response, unit>) array -> 'T
    abstract all: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, unit>) array -> 'T
    // abstract get: IRouterMatcher<IRouter, string> with get, set
    abstract get: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>) array -> 'T
    abstract get: path: RegExp * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>) array -> 'T
    abstract get: path: string * [<ParamArray>] handlers: (Func<Request, Response, NextFunction, unit>) array -> 'T
    abstract get: path: RegExp * [<ParamArray>] handlers: (Func<Request, Response, NextFunction, unit>) array -> 'T
    abstract get: path: string * [<ParamArray>] handlers: #RequestHandler array -> 'T
    [<Emit("$0.get($1...)")>]
    abstract get_: path: string * [<ParamArray>] handlers: #RequestHandler array -> 'T
//    abstract get: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, unit>) array -> 'T
    abstract get: path: string * [<ParamArray>] handlers: (Func<Request, Response, unit>) array -> 'T
    abstract get: path: RegExp * [<ParamArray>] handlers: (Func<Request, Response, unit>) array -> 'T
    
    // abstract post: IRouterMatcher<IRouter, string> with get, set
    abstract post:path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>) array -> 'T
    abstract post: path: string * [<ParamArray>] handlers: (Func<Request, Response, NextFunction, unit>) array -> 'T
    // abstract put: IRouterMatcher<IRouter, string> with get, set
    abstract put: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>) array -> 'T
    abstract put: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, unit>) array -> 'T
    // abstract delete: IRouterMatcher<IRouter, string> with get, set
    abstract delete: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>) array -> 'T
    abstract delete: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, unit>) array -> 'T
    abstract del: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>) array -> 'T
    abstract del: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, unit>) array -> 'T
    abstract del: path: string * [<ParamArray>] handlers: (Func<Request, Response, unit>) array -> 'T

    // abstract delete: path: string * [<ParamArray>] handlers: (obj -> unit) array -> 'T
    // abstract patch: IRouterMatcher<IRouter, string> with get, set
    abstract patch: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>) array -> 'T
    // abstract options: IRouterMatcher<IRouter, string> with get, set
    abstract options: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>) array -> 'T
    abstract options: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, unit>) array -> 'T
    // abstract head: IRouterMatcher<IRouter, string> with get, set
    abstract head: path: string * [<ParamArray>] handlers: (Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>) array -> 'T
    abstract checkout: IRouterMatcher<IRouter> with get, set
    abstract connect: IRouterMatcher<IRouter> with get, set
    abstract copy: IRouterMatcher<IRouter> with get, set
    abstract lock: IRouterMatcher<IRouter> with get, set
    abstract merge: IRouterMatcher<IRouter> with get, set
    abstract mkactivity: IRouterMatcher<IRouter> with get, set
    abstract mkcol: IRouterMatcher<IRouter> with get, set
    abstract move: IRouterMatcher<IRouter> with get, set
    abstract ``m-search``: IRouterMatcher<IRouter> with get, set
    abstract notify: IRouterMatcher<IRouter> with get, set
    abstract propfind: IRouterMatcher<IRouter> with get, set
    abstract proppatch: IRouterMatcher<IRouter> with get, set
    abstract purge: IRouterMatcher<IRouter> with get, set
    abstract report: IRouterMatcher<IRouter> with get, set
    abstract search: IRouterMatcher<IRouter> with get, set
    abstract subscribe: IRouterMatcher<IRouter> with get, set
    abstract trace: IRouterMatcher<IRouter> with get, set
    abstract unlock: IRouterMatcher<IRouter> with get, set
    abstract unsubscribe: IRouterMatcher<IRouter> with get, set
    abstract member ``use``: #IRouter -> unit
    
    abstract member ``use``: System.Func<Request<ParamsDictionary, obj option, obj option, ParsedQs, Dictionary<obj option>>, Response<obj option, Dictionary<obj option>>, unit> -> unit
    abstract member ``use``: System.Func<Request<ParamsDictionary, obj option, obj option, ParsedQs, Dictionary<obj option>>, Response<obj option, Dictionary<obj option>>, NextFunction, unit> -> unit
//    abstract member ``use``: System.Func<Error option, Request<ParamsDictionary, obj option, obj option, ParsedQs, Dictionary<obj option>>, Response<obj option, Dictionary<obj option>>, NextFunction, unit> -> unit
    abstract member ``use``: System.Func<Error option, Request, Response, NextFunction, unit> -> unit
    abstract member ``use``: System.Func<Request, Response, NextFunction, unit> -> unit
//    abstract route: prefix: PathParams -> IRoute
    abstract route: prefix: string -> IRoute
    abstract route: prefix: RegExp -> IRoute
    abstract route: prefix: string array -> IRoute
    abstract route: prefix: RegExp array -> IRoute
    /// Stack of configured routes
    abstract stack: ResizeArray<obj option> with get, set
    // Mimic `inherit RequestHandler` by adding the Invoke method
    // [<Emit("$0($1...)")>]
    // abstract Invoke : Func<ParamsDictionary, obj option, obj option, ParsedQs, Dictionary<obj option>, unit>

type [<AllowNullLiteral>] IRoute =
    abstract path: string with get, set
    abstract stack: obj option with get, set
//    abstract all: IRouterHandler<IRoute> with get, set
    abstract all: Func<Request, Response, unit> -> IRoute
    abstract all: Func<Request, Response, NextFunction, unit> -> IRoute
    //abstract get: IRouterHandler<IRoute> with get, set
    abstract get: Func<Request, Response, unit> -> IRoute
    abstract get: Func<Request, Response, NextFunction, unit> -> IRoute
//    abstract post: IRouterHandler<IRoute> with get, set
    abstract post: Func<Request, Response, unit> -> IRoute
    abstract post: Func<Request, Response, NextFunction, unit> -> IRoute
    abstract put: IRouterHandler<IRoute> with get, set
    abstract delete: IRouterHandler<IRoute> with get, set
    abstract patch: IRouterHandler<IRoute> with get, set
    abstract options: IRouterHandler<IRoute> with get, set
    abstract head: IRouterHandler<IRoute> with get, set
    abstract checkout: IRouterHandler<IRoute> with get, set
    abstract copy: IRouterHandler<IRoute> with get, set
    abstract lock: IRouterHandler<IRoute> with get, set
    abstract merge: IRouterHandler<IRoute> with get, set
    abstract mkactivity: IRouterHandler<IRoute> with get, set
    abstract mkcol: IRouterHandler<IRoute> with get, set
    abstract move: IRouterHandler<IRoute> with get, set
    abstract ``m-search``: IRouterHandler<IRoute> with get, set
    abstract notify: IRouterHandler<IRoute> with get, set
    abstract purge: IRouterHandler<IRoute> with get, set
    abstract report: IRouterHandler<IRoute> with get, set
    abstract search: IRouterHandler<IRoute> with get, set
    abstract subscribe: IRouterHandler<IRoute> with get, set
    abstract trace: IRouterHandler<IRoute> with get, set
    abstract unlock: IRouterHandler<IRoute> with get, set
    abstract unsubscribe: IRouterHandler<IRoute> with get, set

type [<AllowNullLiteral>] Router =
    inherit IRouter

type [<AllowNullLiteral>] CookieOptions =
    abstract maxAge: float option with get, set
    abstract signed: bool option with get, set
    abstract expires: DateTime option with get, set
    abstract httpOnly: bool option with get, set
    abstract path: string option with get, set
    abstract domain: string option with get, set
    abstract secure: bool option with get, set
    abstract encode: (string -> string) option with get, set
    abstract sameSite: U2<bool, string> option with get, set

type [<AllowNullLiteral>] ByteRange =
    abstract start: float with get, set
    abstract ``end``: float with get, set

type [<AllowNullLiteral>] RequestRanges =
    inherit RangeParserRanges

type [<AllowNullLiteral>] Errback =
    [<Emit "$0($1...)">] abstract Invoke: err: Error -> unit

type Request =
    Request<ParamsDictionary, obj option, obj option, ParsedQs, Dictionary<obj option>>

type Request<'P> =
    Request<'P, obj option, obj option, ParsedQs, Dictionary<obj option>>

type Request<'P, 'ResBody> =
    Request<'P, 'ResBody, obj option, ParsedQs, Dictionary<obj option>>

type Request<'P, 'ResBody, 'ReqBody> =
    Request<'P, 'ResBody, 'ReqBody, ParsedQs, Dictionary<obj option>>

type Request<'P, 'ResBody, 'ReqBody, 'ReqQuery> =
    Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, Dictionary<obj option>>

type [<AllowNullLiteral>] Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals when 'Locals :> Dictionary<obj option>> =
    inherit Http.IncomingMessage
    inherit Express.Request
    /// Return request header.
    ///
    /// The `Referrer` header field is special-cased,
    /// both `Referrer` and `Referer` are interchangeable.
    ///
    /// Examples:
    ///
    ///      req.get('Content-Type');
    ///      // => "text/plain"
    ///
    ///      req.get('content-type');
    ///      // => "text/plain"
    ///
    ///      req.get('Something');
    ///      // => undefined
    ///
    /// Aliased as `req.header()`.
    [<Emit "$0.get('set-cookie')">] abstract ``get_set-cookie``: unit -> ResizeArray<string> option
    abstract get: name: string -> string option
    [<Emit "$0.header('set-cookie')">] abstract ``header_set-cookie``: unit -> ResizeArray<string> option
    abstract header: name: string -> string option
    /// Check if the given `type(s)` is acceptable, returning
    /// the best match when true, otherwise `undefined`, in which
    /// case you should respond with 406 "Not Acceptable".
    ///
    /// The `type` value may be a single mime type string
    /// such as "application/json", the extension name
    /// such as "json", a comma-delimted list such as "json, html, text/plain",
    /// or an array `["json", "html", "text/plain"]`. When a list
    /// or array is given the _best_ match, if any is returned.
    ///
    /// Examples:
    ///
    ///      // Accept: text/html
    ///      req.accepts('html');
    ///      // => "html"
    ///
    ///      // Accept: text/*, application/json
    ///      req.accepts('html');
    ///      // => "html"
    ///      req.accepts('text/html');
    ///      // => "text/html"
    ///      req.accepts('json, text');
    ///      // => "json"
    ///      req.accepts('application/json');
    ///      // => "application/json"
    ///
    ///      // Accept: text/*, application/json
    ///      req.accepts('image/png');
    ///      req.accepts('png');
    ///      // => undefined
    ///
    ///      // Accept: text/*;q=.5, application/json
    ///      req.accepts(['html', 'json']);
    ///      req.accepts('html, json');
    ///      // => "json"
    abstract accepts: unit -> ResizeArray<string>
    abstract accepts: ``type``: string -> string
    abstract accepts: ``type``: ResizeArray<string> -> string
    abstract accepts: [<ParamArray>] ``type``: string[] -> string
    /// Returns the first accepted charset of the specified character sets,
    /// based on the request's Accept-Charset HTTP header field.
    /// If none of the specified charsets is accepted, returns false.
    ///
    /// For more information, or if you have issues or concerns, see accepts.
    abstract acceptsCharsets: unit -> ResizeArray<string>
    abstract acceptsCharsets: charset: string -> string
    abstract acceptsCharsets: charset: ResizeArray<string> -> string
    abstract acceptsCharsets: [<ParamArray>] charset: string[] -> string
    /// Returns the first accepted encoding of the specified encodings,
    /// based on the request's Accept-Encoding HTTP header field.
    /// If none of the specified encodings is accepted, returns false.
    ///
    /// For more information, or if you have issues or concerns, see accepts.
    abstract acceptsEncodings: unit -> ResizeArray<string>
    abstract acceptsEncodings: encoding: string -> string
    abstract acceptsEncodings: encoding: ResizeArray<string> -> string
    abstract acceptsEncodings: [<ParamArray>] encoding: string[] -> string
    /// Returns the first accepted language of the specified languages,
    /// based on the request's Accept-Language HTTP header field.
    /// If none of the specified languages is accepted, returns false.
    ///
    /// For more information, or if you have issues or concerns, see accepts.
    abstract acceptsLanguages: unit -> ResizeArray<string>
    abstract acceptsLanguages: lang: string -> string
    abstract acceptsLanguages: lang: ResizeArray<string> -> string
    abstract acceptsLanguages: [<ParamArray>] lang: string[] -> string
    /// Parse Range header field, capping to the given `size`.
    ///
    /// Unspecified ranges such as "0-" require knowledge of your resource length. In
    /// the case of a byte range this is of course the total number of bytes.
    /// If the Range header field is not given `undefined` is returned.
    /// If the Range header field is given, return value is a result of range-parser.
    /// See more ./types/range-parser/index.d.ts
    ///
    /// NOTE: remember that ranges are inclusive, so for example "Range: users=0-3"
    /// should respond with 4 users when available, not 3.
    abstract range: size: float * ?options: RangeParserOptions -> U2<RangeParserRanges, RangeParserResult> option
    /// Return an array of Accepted media types
    /// ordered from highest quality to lowest.
    abstract accepted: ResizeArray<MediaType> with get, set
    abstract param: name: string * ?defaultValue: obj -> string
    /// Check if the incoming request contains the "Content-Type"
    /// header field, and it contains the give mime `type`.
    ///
    /// Examples:
    ///
    ///       // With Content-Type: text/html; charset=utf-8
    ///       req.is('html');
    ///       req.is('text/html');
    ///       req.is('text/*');
    ///       // => true
    ///
    ///       // When Content-Type is application/json
    ///       req.is('json');
    ///       req.is('application/json');
    ///       req.is('application/*');
    ///       // => true
    ///
    ///       req.is('html');
    ///       // => false
    abstract is: ``type``: U2<string, ResizeArray<string>> -> string option
    /// Return the protocol string "http" or "https"
    /// when requested with TLS. When the "trust proxy"
    /// setting is enabled the "X-Forwarded-Proto" header
    /// field will be trusted. If you're running behind
    /// a reverse proxy that supplies https for you this
    /// may be enabled.
    abstract protocol: string with get, set
    /// Short-hand for:
    ///
    ///     req.protocol == 'https'
    abstract secure: bool with get, set
    /// Return the remote address, or when
    /// "trust proxy" is `true` return
    /// the upstream addr.
    abstract ip: string with get, set
    /// When "trust proxy" is `true`, parse
    /// the "X-Forwarded-For" ip address list.
    ///
    /// For example if the value were "client, proxy1, proxy2"
    /// you would receive the array `["client", "proxy1", "proxy2"]`
    /// where "proxy2" is the furthest down-stream.
    abstract ips: ResizeArray<string> with get, set
    /// Return subdomains as an array.
    ///
    /// Subdomains are the dot-separated parts of the host before the main domain of
    /// the app. By default, the domain of the app is assumed to be the last two
    /// parts of the host. This can be changed by setting "subdomain offset".
    ///
    /// For example, if the domain is "tobi.ferrets.example.com":
    /// If "subdomain offset" is not set, req.subdomains is `["ferrets", "tobi"]`.
    /// If "subdomain offset" is 3, req.subdomains is `["tobi"]`.
    abstract subdomains: ResizeArray<string> with get, set
    /// Short-hand for `url.parse(req.url).pathname`.
    abstract path: string with get, set
    /// Parse the "Host" header field hostname.
    abstract hostname: string with get, set
    abstract host: string with get, set
    /// Check if the request is fresh, aka
    /// Last-Modified and/or the ETag
    /// still match.
    abstract fresh: bool with get, set
    /// Check if the request is stale, aka
    /// "Last-Modified" and / or the "ETag" for the
    /// resource has changed.
    abstract stale: bool with get, set
    /// Check if the request was an _XMLHttpRequest_.
    abstract xhr: bool with get, set
    abstract body: 'ReqBody with get, set
    abstract cookies: obj option with get, set
    abstract ``method``: string with get, set
    abstract ``params``: 'P with get, set
    abstract query: 'ReqQuery with get, set
    abstract route: obj option with get, set
    abstract signedCookies: obj option with get, set
    abstract originalUrl: string with get, set
    abstract url: string with get, set
    abstract baseUrl: string with get, set
    abstract app: Application with get, set
    /// After middleware.init executed, Request will contain res and next properties
    /// See: express/lib/middleware/init.js
    abstract res: Response<'ResBody, 'Locals> option with get, set
    abstract next: NextFunction option with get, set

type [<AllowNullLiteral>] MediaType =
    abstract value: string with get, set
    abstract quality: float with get, set
    abstract ``type``: string with get, set
    abstract subtype: string with get, set

type Send =
    Send<obj option, Response<obj option>>

type Send<'ResBody> =
    Send<'ResBody, Response<'ResBody>>

type [<AllowNullLiteral>] Send<'ResBody, 'T> =
    [<Emit "$0($1...)">] abstract Invoke: ?body: 'ResBody -> 'T

type Response =
    Response<obj, Dictionary<obj option>, int>

type Response<'ResBody> =
    Response<'ResBody, Dictionary<obj option>, int>

type Response<'ResBody, 'Locals when 'Locals :> Dictionary<obj option>> =
    Response<'ResBody, 'Locals, int>

type [<AllowNullLiteral>] Response<'ResBody, 'Locals, 'StatusCode when 'Locals :> Dictionary<obj option>> =
    inherit Http.ServerResponse
    inherit Express.Response
    /// Set status `code`.
//    abstract status: code: 'StatusCode -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract status: code: 'StatusCode -> unit
    /// Set the response HTTP status code to `statusCode` and send its string representation as the response body.
    abstract sendStatus: code: 'StatusCode -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Set Link header field with the given `links`.
    ///
    /// Examples:
    ///
    ///     res.links({
    ///       next: 'http://api.example.com/users?page=2',
    ///       last: 'http://api.example.com/users?page=5'
    ///     });
    abstract links: links: obj option -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Send a response.
    ///
    /// Examples:
    ///
    ///      res.send(new Buffer('wahoo'));
    ///      res.send({ some: 'json' });
    ///      res.send('<p>some html</p>');
    ///      res.status(404).send('Sorry, cant find that');
    // abstract send: Send<'ResBody, Response<'ResBody, 'Locals, 'StatusCode>> with get, set
    abstract send: ?body: 'ResBody -> unit
    /// Send JSON response.
    ///
    /// Examples:
    ///
    ///      res.json(null);
    ///      res.json({ user: 'tj' });
    ///      res.status(500).json('oh noes!');
    ///      res.status(404).json('I dont have that');
//    abstract json: Send<'ResBody, Response<'ResBody, 'Locals, 'StatusCode>> with get, set
    abstract json: ?body: 'ResBody -> 'T
    /// Send JSON response with JSONP callback support.
    ///
    /// Examples:
    ///
    ///      res.jsonp(null);
    ///      res.jsonp({ user: 'tj' });
    ///      res.status(500).jsonp('oh noes!');
    ///      res.status(404).jsonp('I dont have that');
    abstract jsonp: Send<'ResBody, Response<'ResBody, 'Locals, 'StatusCode>> with get, set
    /// Transfer the file at the given `path`.
    ///
    /// Automatically sets the _Content-Type_ response header field.
    /// The callback `fn(err)` is invoked when the transfer is complete
    /// or when an error occurs. Be sure to check `res.headersSent`
    /// if you wish to attempt responding, as the header and some data
    /// may have already been transferred.
    ///
    /// Options:
    ///
    ///    - `maxAge`   defaulting to 0 (can be string converted by `ms`)
    ///    - `root`     root directory for relative filenames
    ///    - `headers`  object of headers to serve with file
    ///    - `dotfiles` serve dotfiles, defaulting to false; can be `"allow"` to send them
    ///
    /// Other options are passed along to `send`.
    ///
    /// Examples:
    ///
    ///   The following example illustrates how `res.sendFile()` may
    ///   be used as an alternative for the `static()` middleware for
    ///   dynamic situations. The code backing `res.sendFile()` is actually
    ///   the same code, so HTTP cache support etc is identical.
    ///
    ///      app.get('/user/:uid/photos/:file', function(req, res){
    ///        var uid = req.params.uid
    ///          , file = req.params.file;
    ///
    ///        req.user.mayViewFilesFrom(uid, function(yes){
    ///          if (yes) {
    ///            res.sendFile('/uploads/' + uid + '/' + file);
    ///          } else {
    ///            res.send(403, 'Sorry! you cant see that.');
    ///          }
    ///        });
    ///      });
    abstract sendFile: path: string * ?fn: Errback -> unit
    abstract sendFile: path: string * options: obj option * ?fn: Errback -> unit
    abstract sendfile: path: string -> unit
    abstract sendfile: path: string * options: obj option -> unit
    abstract sendfile: path: string * fn: Errback -> unit
    abstract sendfile: path: string * options: obj option * fn: Errback -> unit
    /// Transfer the file at the given `path` as an attachment.
    ///
    /// Optionally providing an alternate attachment `filename`,
    /// and optional callback `fn(err)`. The callback is invoked
    /// when the data transfer is complete, or when an error has
    /// ocurred. Be sure to check `res.headersSent` if you plan to respond.
    ///
    /// The optional options argument passes through to the underlying
    /// res.sendFile() call, and takes the exact same parameters.
    ///
    /// This method uses `res.sendfile()`.
    abstract download: path: string * ?fn: Errback -> unit
    abstract download: path: string * filename: string * ?fn: Errback -> unit
    abstract download: path: string * filename: string * options: obj option * ?fn: Errback -> unit
    /// Set _Content-Type_ response header with `type` through `mime.lookup()`
    /// when it does not contain "/", or set the Content-Type to `type` otherwise.
    ///
    /// Examples:
    ///
    ///      res.type('.html');
    ///      res.type('html');
    ///      res.type('json');
    ///      res.type('application/json');
    ///      res.type('png');
    abstract contentType: ``type``: string -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Set _Content-Type_ response header with `type` through `mime.lookup()`
    /// when it does not contain "/", or set the Content-Type to `type` otherwise.
    ///
    /// Examples:
    ///
    ///      res.type('.html');
    ///      res.type('html');
    ///      res.type('json');
    ///      res.type('application/json');
    ///      res.type('png');
    abstract ``type``: ``type``: string -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Respond to the Acceptable formats using an `obj`
    /// of mime-type callbacks.
    ///
    /// This method uses `req.accepted`, an array of
    /// acceptable types ordered by their quality values.
    /// When "Accept" is not present the _first_ callback
    /// is invoked, otherwise the first match is used. When
    /// no match is performed the server responds with
    /// 406 "Not Acceptable".
    ///
    /// Content-Type is set for you, however if you choose
    /// you may alter this within the callback using `res.type()`
    /// or `res.set('Content-Type', ...)`.
    ///
    ///     res.format({
    ///       'text/plain': function(){
    ///         res.send('hey');
    ///       },
    ///
    ///       'text/html': function(){
    ///         res.send('<p>hey</p>');
    ///       },
    ///
    ///       'appliation/json': function(){
    ///         res.send({ message: 'hey' });
    ///       }
    ///     });
    ///
    /// In addition to canonicalized MIME types you may
    /// also use extnames mapped to these types:
    ///
    ///     res.format({
    ///       text: function(){
    ///         res.send('hey');
    ///       },
    ///
    ///       html: function(){
    ///         res.send('<p>hey</p>');
    ///       },
    ///
    ///       json: function(){
    ///         res.send({ message: 'hey' });
    ///       }
    ///     });
    ///
    /// By default Express passes an `Error`
    /// with a `.status` of 406 to `next(err)`
    /// if a match is not made. If you provide
    /// a `.default` callback it will be invoked
    /// instead.
    abstract format: obj: obj option -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Set _Content-Disposition_ header to _attachment_ with optional `filename`.
    abstract attachment: ?filename: string -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Set header `field` to `val`, or pass
    /// an object of header fields.
    ///
    /// Examples:
    ///
    ///     res.set('Foo', ['bar', 'baz']);
    ///     res.set('Accept', 'application/json');
    ///     res.set({ Accept: 'text/plain', 'X-API-Key': 'tobi' });
    ///
    /// Aliased as `res.header()`.
    abstract set: field: obj option -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract set: field: string * ?value: U2<string, ResizeArray<string>> -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract set: field: string * ?value: ResizeArray<string> -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract set: field: string * ?value: string-> Response<'ResBody, 'Locals, 'StatusCode>
    abstract header: field: obj option -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract header: field: string * ?value: U2<string, ResizeArray<string>> -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract headersSent: bool with get, set
    /// Get value for header `field`.
    abstract get: field: string -> string
    /// Clear cookie `name`.
    abstract clearCookie: name: string * ?options: obj -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Set cookie `name` to `val`, with the given `options`.
    ///
    /// Options:
    ///
    ///     - `maxAge`   max-age in milliseconds, converted to `expires`
    ///     - `signed`   sign the cookie
    ///     - `path`     defaults to "/"
    ///
    /// Examples:
    ///
    ///     // "Remember Me" for 15 minutes
    ///     res.cookie('rememberme', '1', { expires: new Date(Date.now() + 900000), httpOnly: true });
    ///
    ///     // save as above
    ///     res.cookie('rememberme', '1', { maxAge: 900000, httpOnly: true })
    abstract cookie: name: string * ``val``: string * options: CookieOptions -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract cookie: name: string * ``val``: obj option * options: CookieOptions -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract cookie: name: string * ``val``: obj option -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Set the location header to `url`.
    ///
    /// The given `url` can also be the name of a mapped url, for
    /// example by default express supports "back" which redirects
    /// to the _Referrer_ or _Referer_ headers or "/".
    ///
    /// Examples:
    ///
    ///     res.location('/foo/bar').;
    ///     res.location('http://example.com');
    ///     res.location('../login'); // /blog/post/1 -> /blog/login
    ///
    /// Mounting:
    ///
    ///    When an application is mounted and `res.location()`
    ///    is given a path that does _not_ lead with "/" it becomes
    ///    relative to the mount-point. For example if the application
    ///    is mounted at "/blog", the following would become "/blog/login".
    ///
    ///       res.location('login');
    ///
    ///    While the leading slash would result in a location of "/login":
    ///
    ///       res.location('/login');
    abstract location: url: string -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Redirect to the given `url` with optional response `status`
    /// defaulting to 302.
    ///
    /// The resulting `url` is determined by `res.location()`, so
    /// it will play nicely with mounted apps, relative paths,
    /// `"back"` etc.
    ///
    /// Examples:
    ///
    ///     res.redirect('/foo/bar');
    ///     res.redirect('http://example.com');
    ///     res.redirect(301, 'http://example.com');
    ///     res.redirect('http://example.com', 301);
    ///     res.redirect('../login'); // /blog/post/1 -> /blog/login
    abstract redirect: url: string -> unit
    abstract redirect: status: float * url: string -> unit
    abstract redirect: url: string * status: float -> unit
    /// Render `view` with the given `options` and optional callback `fn`.
    /// When a callback function is given a response will _not_ be made
    /// automatically, otherwise a response of _200_ and _text/html_ is given.
    ///
    /// Options:
    ///
    ///   - `cache`     boolean hinting to the engine it should cache
    ///   - `filename`  filename of the view being rendered
    abstract render: view: string * ?options: obj * ?callback: (Error -> string -> unit) -> unit
    abstract render: view: string * ?callback: (Error -> string -> unit) -> unit
    abstract locals: 'Locals with get, set
    abstract charset: string with get, set
    /// Adds the field to the Vary response header, if it is not there already.
    /// Examples:
    ///
    ///      res.vary('User-Agent').render('docs');
    abstract vary: field: string -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract app: Application with get, set
    /// Appends the specified value to the HTTP response header field.
    /// If the header is not already set, it creates the header with the specified value.
    /// The value parameter can be a string or an array.
    ///
    /// Note: calling res.set() after res.append() will reset the previously-set header value.
    abstract append: field: string * ?value: U2<ResizeArray<string>, string> -> Response<'ResBody, 'Locals, 'StatusCode>
    /// After middleware.init executed, Response will contain req property
    /// See: express/lib/middleware/init.js
    abstract req: Request option with get, set

type Handler = RequestHandler
    // inherit RequestHandler

type RequestParamHandler =
    Func<Request, Response, NextFunction, obj, string, unit>
//    [<Emit "$0($1...)">] abstract Invoke: req: Request * res: Response * next: NextFunction * value: obj option * name: string -> obj option

type [<AllowNullLiteral>] ApplicationRequestHandler<'T> =
    interface end

type EngineRenderFunc = Func<obj, string option, unit>

type [<AllowNullLiteral>] Application =
    inherit EventEmitter
    inherit IRouter
    inherit Express.Application
    /// Express instance itself is a request handler, which could be invoked without
    /// third argument.
    [<Emit "$0($1...)">] abstract Invoke: req: U2<Request, Http.IncomingMessage> * res: U2<Response, Http.ServerResponse> -> obj option
    /// Initialize the server.
    ///
    ///    - setup default configuration
    ///    - setup default middleware
    ///    - setup route reflection methods
    abstract init: unit -> unit
    /// Initialize application configuration.
    abstract defaultConfiguration: unit -> unit
    /// Register the given template engine callback `fn`
    /// as `ext`.
    ///
    /// By default will `require()` the engine based on the
    /// file extension. For example if you try to render
    /// a "foo.jade" file Express will invoke the following internally:
    ///
    ///      app.engine('jade', require('jade').__express);
    ///
    /// For engines that do not provide `.__express` out of the box,
    /// or if you wish to "map" a different extension to the template engine
    /// you may use this method. For example mapping the EJS template engine to
    /// ".html" files:
    ///
    ///      app.engine('html', require('ejs').renderFile);
    ///
    /// In this case EJS provides a `.renderFile()` method with
    /// the same signature that Express expects: `(path, options, callback)`,
    /// though note that it aliases this method as `ejs.__express` internally
    /// so if you're using ".ejs" extensions you dont need to do anything.
    ///
    /// Some template engines do not follow this convention, the
    /// [Consolidate.js](https://github.com/visionmedia/consolidate.js)
    /// library was created to map all of node's popular template
    /// engines to follow this convention, thus allowing them to
    /// work seamlessly within Express.
    // abstract engine: ext: string * fn: (string -> obj -> (obj option -> string -> unit) -> unit) -> Application

    abstract engine: ext : string * fn: (string -> #obj -> EngineRenderFunc -> unit) -> unit
    /// Assign `setting` to `val`, or return `setting`'s value.
    ///
    ///     app.set('foo', 'bar');
    ///     app.get('foo');
    ///     // => "bar"
    ///     app.set('foo', ['bar', 'baz']);
    ///     app.get('foo');
    ///     // => ["bar", "baz"]
    ///
    /// Mounted servers inherit their parent server's settings.
    abstract set: setting: string * ``val``: obj -> unit
    [<Emit("$0.get")>]
    /// <summary>
    /// This property map the <c>get</c> property in JavaScript
    ///
    /// <note>This is needed in order to have access to the <c>get</c> methods which maps the GET HTTP method</note>
    /// </summary>
    abstract Get: obj with get, set
    /// Map the given param placeholder `name`(s) to the given callback(s).
    ///
    /// Parameter mapping is used to provide pre-conditions to routes
    /// which use normalized placeholders. For example a _:user_id_ parameter
    /// could automatically load a user's information from the database without
    /// any additional code,
    ///
    /// The callback uses the samesignature as middleware, the only differencing
    /// being that the value of the placeholder is passed, in this case the _id_
    /// of the user. Once the `next()` function is invoked, just like middleware
    /// it will continue on to execute the route, or subsequent parameter functions.
    ///
    ///       app.param('user_id', function(req, res, next, id){
    ///         User.find(id, function(err, user){
    ///           if (err) {
    ///             next(err);
    ///           } else if (user) {
    ///             req.user = user;
    ///             next();
    ///           } else {
    ///             next(new Error('failed to load user'));
    ///           }
    ///         });
    ///       });
    /// Alternatively, you can pass only a callback, in which case you have the opportunity to alter the app.param()
//    abstract param: name: U2<string, ResizeArray<string>> * handler: RequestParamHandler -> Application
    abstract param: name: string * handler: Func<Request, Response, NextFunction, obj, string, unit> -> unit
    abstract param: name: string * handler: Func<Request, Response, NextFunction, obj, unit> -> unit
    abstract param: name: ResizeArray<string> * handler: Func<Request, Response, NextFunction, obj, string, unit> -> unit
    abstract param: name: ResizeArray<string> * handler: Func<Request, Response, NextFunction, obj, unit> -> unit
    /// Alternatively, you can pass only a callback, in which case you have the opportunity to alter the app.param()
    abstract param: callback: (string -> RegExp -> RequestParamHandler) -> unit
    /// Return the app's absolute pathname
    /// based on the parent(s) that have
    /// mounted it.
    ///
    /// For example if the application was
    /// mounted as "/admin", which itself
    /// was mounted as "/blog" then the
    /// return value would be "/blog/admin".
    abstract path: unit -> string
    /// Check if `setting` is enabled (truthy).
    ///
    ///     app.enabled('foo')
    ///     // => false
    ///
    ///     app.enable('foo')
    ///     app.enabled('foo')
    ///     // => true
    abstract enabled: setting: string -> bool
    /// Check if `setting` is disabled.
    ///
    ///     app.disabled('foo')
    ///     // => true
    ///
    ///     app.enable('foo')
    ///     app.disabled('foo')
    ///     // => false
    abstract disabled: setting: string -> bool
    /// Enable `setting`.
    abstract enable: setting: string -> Application
    /// Disable `setting`.
    abstract disable: setting: string -> Application
    /// Render the given view `name` name with `options`
    /// and a callback accepting an error and the
    /// rendered template string.
    ///
    /// Example:
    ///
    ///     app.render('email', { name: 'Tobi' }, function(err, html){
    ///       // ...
    ///     })
    abstract render: name: string * ?options: obj * ?callback: (Error option -> string -> unit) -> unit
    abstract render: name: string * callback: (Error option -> string -> unit) -> unit
    /// Listen for connections.
    ///
    /// A node `http.Server` is returned, with this
    /// application (which is a `Function`) as its
    /// callback. If you wish to create both an HTTP
    /// and HTTPS server you may do so with the "http"
    /// and "https" modules as shown here:
    ///
    ///     var http = require('http')
    ///       , https = require('https')
    ///       , express = require('express')
    ///       , app = express();
    ///
    ///     http.createServer(app).listen(80);
    ///     https.createServer({ ... }, app).listen(443);
    abstract listen: port: int * hostname: string * backlog: int * ?callback: (unit -> unit) -> Http.Server
    abstract listen: port: int * hostname: string * ?callback: (unit -> unit) -> Http.Server
    abstract listen: port: int * ?callback: (unit -> unit) -> Http.Server
    abstract listen: ?callback: (unit -> unit) -> Http.Server
    abstract listen: path: string * ?callback: (unit -> unit) -> Http.Server
    abstract listen: handle: obj option * ?listeningListener: (unit -> unit) -> Http.Server
    abstract router: string with get, set
    abstract settings: obj option with get, set
    abstract resource: obj option with get, set
    abstract map: obj option with get, set
    abstract locals: Dictionary<obj> with get, set
    /// The app.routes object houses all of the routes defined mapped by the
    /// associated HTTP verb. This object may be used for introspection
    /// capabilities, for example Express uses this internally not only for
    /// routing but to provide default OPTIONS behaviour unless app.options()
    /// is used. Your application or framework may also remove routes by
    /// simply by removing them from this object.
    abstract routes: obj option with get, set
    /// Used to get all registered routes in Express Application
    abstract _router: obj option with get, set
//    abstract ``use``: ApplicationRequestHandler<Application> with get, set
    /// The mount event is fired on a sub-app, when it is mounted on a parent app.
    /// The parent app is passed to the callback function.
    ///
    /// NOTE:
    /// Sub-apps will:
    ///   - Not inherit the value of settings that have a default value. You must set the value in the sub-app.
    ///   - Inherit the value of settings with no default value.
    abstract on: string * (Application -> unit) -> Application
    /// The app.mountpath property contains one or more path patterns on which a sub-app was mounted.
    abstract mountpath: U2<string, ResizeArray<string>> with get, set

type [<AllowNullLiteral>] Express =
    inherit Application
    abstract request: Request with get, set
    abstract response: Response with get, set
