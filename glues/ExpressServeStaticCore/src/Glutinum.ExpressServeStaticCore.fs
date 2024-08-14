// ts2fable 0.8.0
module rec Glutinum.ExpressServeStaticCore

open System
open Glutinum.ExpressServeStaticCore
open Fable.Core
open Node
open Qs
open Glutinum.RangeParser

type Array<'T> = System.Collections.Generic.IList<'T>
type Error = System.Exception
type RegExp = System.Text.RegularExpressions.Regex

type EventEmitter = Events.EventEmitter
type RangeParserOptions = RangeParser.Options
type RangeParserResult = RangeParser.ParseRangeResult
type RangeParserRanges = RangeParser.Ranges
type ParsedQs = Qs.ParsedQs

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
    /// <summary>
    /// "Break-out" of a router by calling {next('route')};
    /// </summary>
    [<Emit "$0('route')">] abstract Invoke_route: unit -> unit
    /// <summary>
    /// "Break-out" of a router by calling {next('router')};
    /// </summary>
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
    [<Emit "$0($1...)">] abstract Invoke: req: Request * res: Response * next: NextFunction -> unit
    //Func<Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>

/// <summary>
/// Adapters are used to make F# type system "happy" with the code you write in certain cases
/// </summary>
[<Erase>]
type Adapter =
    /// <summary>
    /// Adapter used to create a RequestHandler compatible function
    /// </summary>
    static member inline RequestHandler (f : Func<Request, Response, NextFunction, unit>) : RequestHandler =
        unbox f

    /// <summary>
    /// Adapter used to create a RequestHandler compatible function
    /// </summary>
    static member inline RequestHandler (f : Func<Error option, Request, Response, NextFunction, unit>) : RequestHandler =
        unbox f

    /// <summary>
    /// Adapter used to create a RequestHandler compatible function
    /// </summary>
    static member inline RequestHandler (f : Func<Request, Response, unit>) : RequestHandler =
        unbox f

    /// <summary>
    /// Adapter used to create a NextFunction compatible function
    /// </summary>
    static member inline NextFunction (f : Func<obj option, unit>) : NextFunction =
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
    Func<obj option, Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>, Response<'ResBody, 'Locals>, NextFunction, unit>

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
    /// <summary>
    /// Map the given param placeholder <c>name</c>(s) to the given callback(s).
    ///
    /// Parameter mapping is used to provide pre-conditions to routes
    /// which use normalized placeholders. For example a _:user_id_ parameter
    /// could automatically load a user's information from the database without
    /// any additional code,
    ///
    /// The callback uses the samesignature as middleware, the only differencing
    /// being that the value of the placeholder is passed, in this case the _id_
    /// of the user. Once the <c>next()</c> function is invoked, just like middleware
    /// it will continue on to execute the route, or subsequent parameter functions.
    /// </summary>
    /// <code lang="fsharp">
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
    /// </code>
    abstract param: name: string * handler: Func<Request, Response, NextFunction, obj, string, unit> -> unit
    abstract param: name: ResizeArray<string> * handler: Func<Request, Response, NextFunction, obj, string, unit> -> unit
    abstract param: name: string * regexp : RegExp -> unit
    /// <summary>
    /// Alternatively, you can pass only a callback, in which case you have the opportunity to alter the app.param()
    /// </summary>
    abstract param: callback: (string -> RegExp -> RequestParamHandler) -> unit
    ///////////////////////////
    /// all method

    // abstract all: IRouterMatcher<IRouter, string> with get, set
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit

    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit

    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : string * [<ParamArray>] handlers: #RequestHandler array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : RegExp * [<ParamArray>] handlers: #RequestHandler array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array -> unit

    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    /// <summary>
    /// Special-cased "all" method, applying the given route <c>path</c>,
    /// middleware, and callback to _every_ HTTP method.
    /// </summary>
    abstract all : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit

    ///////////////////////////
    /// get method

    // abstract get: IRouterMatcher<IRouter, string> with get, set
    abstract get : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract get : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract get : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract get : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract get : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract get : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit

    abstract get : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract get : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract get : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract get : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract get : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract get : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit

    abstract get : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract get : path : string * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract get : path : RegExp * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract get : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract get : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract get : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array -> unit

    abstract get : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract get : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract get : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract get : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract get : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract get : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit

    ///////////////////////////
    /// post method

    // abstract post: IRouterMatcher<IRouter, string> with get, set
    abstract post : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract post : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract post : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract post : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract post : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract post : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit

    abstract post : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract post : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract post : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract post : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract post : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract post : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit

    abstract post : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract post : path : string * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract post : path : RegExp * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract post : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract post : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract post : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array -> unit

    abstract post : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract post : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract post : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract post : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract post : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract post : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit

    ///////////////////////////
    /// put method

    // abstract put: IRouterMatcher<IRouter, string> with get, set
    abstract put : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract put : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract put : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract put : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract put : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract put : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit

    abstract put : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract put : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract put : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract put : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract put : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract put : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit

    abstract put : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract put : path : string * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract put : path : RegExp * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract put : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract put : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract put : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array -> unit

    abstract put : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract put : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract put : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract put : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract put : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract put : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit

    ///////////////////////////
    /// delete method

    // abstract delete: IRouterMatcher<IRouter, string> with get, set
    abstract delete : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract delete : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract delete : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract delete : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract delete : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract delete : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit

    abstract delete : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract delete : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract delete : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract delete : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract delete : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract delete : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit

    abstract delete : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract delete : path : string * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract delete : path : RegExp * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract delete : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract delete : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract delete : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array -> unit

    abstract delete : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract delete : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract delete : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract delete : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract delete : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract delete : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit

    ////////////////////////////////////
    /// del methods

    abstract del : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract del : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract del : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract del : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract del : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract del : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit

    abstract del : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract del : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract del : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract del : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract del : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract del : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit

    abstract del : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract del : path : string * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract del : path : RegExp * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract del : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract del : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract del : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array -> unit

    abstract del : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract del : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract del : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract del : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract del : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract del : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit

    ///////////////////////////
    /// patch method

//    abstract patch: IRouterMatcher<IRouter, string> with get, set
    abstract patch : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract patch : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract patch : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract patch : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract patch : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract patch : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit

    abstract patch : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract patch : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract patch : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract patch : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract patch : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract patch : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit

    abstract patch : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract patch : path : string * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract patch : path : RegExp * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract patch : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract patch : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract patch : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array -> unit

    abstract patch : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract patch : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract patch : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract patch : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract patch : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract patch : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit

    ///////////////////////////
    /// options method

    // abstract options: IRouterMatcher<IRouter, string> with get, set
    abstract options : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract options : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract options : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract options : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract options : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract options : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit

    abstract options : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract options : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract options : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract options : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract options : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract options : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit

    abstract options : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract options : path : string * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract options : path : RegExp * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract options : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract options : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract options : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array -> unit

    abstract options : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract options : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract options : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract options : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract options : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract options : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit

    ///////////////////////////
    /// head method

    // abstract head: IRouterMatcher<IRouter, string> with get, set
    abstract head : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract head : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract head : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract head : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract head : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract head : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit

    abstract head : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract head : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract head : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract head : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract head : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract head : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit

    abstract head : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract head : path : string * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract head : path : RegExp * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract head : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract head : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract head : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array -> unit

    abstract head : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract head : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract head : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract head : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract head : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract head : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit

    // abstract checkout: IRouterMatcher<IRouter> with get, set
    abstract checkout : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract checkout : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract checkout : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract checkout : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract checkout : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit
    abstract checkout : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array -> unit

    abstract checkout : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract checkout : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract checkout : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract checkout : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract checkout : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit
    abstract checkout : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array -> unit

    abstract checkout : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract checkout : path : string * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract checkout : path : RegExp * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract checkout : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract checkout : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array -> unit
    abstract checkout : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array -> unit

    abstract checkout : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract checkout : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract checkout : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract checkout : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract checkout : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit
    abstract checkout : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array -> unit

    // abstract connect: IRouterMatcher<IRouter> with get, set
    abstract connect : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract connect : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract connect : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract connect : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract connect : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract connect : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract connect : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract connect : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract connect : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract connect : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract connect : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract connect : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract connect : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract connect : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract connect : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract connect : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract connect : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract connect : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract connect : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract connect : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract connect : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract connect : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract connect : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract connect : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract copy: IRouterMatcher<IRouter> with get, set
    abstract copy : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract copy : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract copy : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract copy : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract copy : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract copy : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract copy : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract copy : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract copy : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract copy : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract copy : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract copy : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract copy : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract copy : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract copy : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract copy : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract copy : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract copy : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract copy : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract copy : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract copy : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract copy : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract copy : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract copy : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract lock: IRouterMatcher<IRouter> with get, set
    abstract lock : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract lock : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract lock : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract lock : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract lock : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract lock : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract lock : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract lock : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract lock : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract lock : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract lock : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract lock : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract lock : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract lock : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract lock : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract lock : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract lock : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract lock : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract lock : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract lock : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract lock : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract lock : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract lock : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract lock : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract merge: IRouterMatcher<IRouter> with get, set
    abstract merge : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract merge : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract merge : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract merge : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract merge : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract merge : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract merge : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract merge : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract merge : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract merge : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract merge : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract merge : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract merge : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract merge : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract merge : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract merge : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract merge : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract merge : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract merge : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract merge : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract merge : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract merge : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract merge : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract merge : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit


    // abstract mkactivity: IRouterMatcher<IRouter> with get, set
    abstract mkactivity : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract mkactivity : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract mkactivity : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract mkactivity : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract mkactivity : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract mkactivity : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract mkactivity : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract mkactivity : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract mkactivity : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract mkactivity : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract mkactivity : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract mkactivity : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract mkactivity : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract mkactivity : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract mkactivity : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract mkactivity : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract mkactivity : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract mkactivity : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract mkactivity : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract mkactivity : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract mkactivity : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract mkactivity : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract mkactivity : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract mkactivity : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract mkcol: IRouterMatcher<IRouter> with get, set
    abstract mkcol : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract mkcol : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract mkcol : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract mkcol : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract mkcol : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract mkcol : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract mkcol : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract mkcol : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract mkcol : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract mkcol : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract mkcol : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract mkcol : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract mkcol : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract mkcol : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract mkcol : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract mkcol : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract mkcol : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract mkcol : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract mkcol : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract mkcol : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract mkcol : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract mkcol : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract mkcol : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract mkcol : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract move: IRouterMatcher<IRouter> with get, set
    abstract move : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract move : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract move : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract move : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract move : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract move : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract move : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract move : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract move : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract move : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract move : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract move : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract move : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract move : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract move : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract move : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract move : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract move : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract move : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract move : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract move : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract move : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract move : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract move : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract ``m-search``: IRouterMatcher<IRouter> with get, set
    abstract ``m-search`` : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract ``m-search`` : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract ``m-search`` : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract ``m-search`` : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract ``m-search`` : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract ``m-search`` : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract ``m-search`` : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract ``m-search`` : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract ``m-search`` : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract ``m-search`` : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract ``m-search`` : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract ``m-search`` : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract ``m-search`` : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract ``m-search`` : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract ``m-search`` : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract ``m-search`` : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract ``m-search`` : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract ``m-search`` : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract ``m-search`` : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract ``m-search`` : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract ``m-search`` : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract ``m-search`` : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract ``m-search`` : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract ``m-search`` : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract notify: IRouterMatcher<IRouter> with get, set
    abstract notify : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract notify : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract notify : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract notify : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract notify : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract notify : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract notify : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract notify : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract notify : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract notify : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract notify : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract notify : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract notify : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract notify : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract notify : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract notify : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract notify : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract notify : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract notify : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract notify : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract notify : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract notify : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract notify : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract notify : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract propfind: IRouterMatcher<IRouter> with get, set
    abstract propfind : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract propfind : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract propfind : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract propfind : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract propfind : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract propfind : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract propfind : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract propfind : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract propfind : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract propfind : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract propfind : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract propfind : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract propfind : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract propfind : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract propfind : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract propfind : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract propfind : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract propfind : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract propfind : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract propfind : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract propfind : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract propfind : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract propfind : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract propfind : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract proppatch: IRouterMatcher<IRouter> with get, set
    abstract proppatch : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract proppatch : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract proppatch : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract proppatch : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract proppatch : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract proppatch : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract proppatch : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract proppatch : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract proppatch : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract proppatch : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract proppatch : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract proppatch : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract proppatch : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract proppatch : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract proppatch : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract proppatch : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract proppatch : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract proppatch : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract proppatch : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract proppatch : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract proppatch : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract proppatch : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract proppatch : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract proppatch : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract purge: IRouterMatcher<IRouter> with get, set
    abstract purge : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract purge : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract purge : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract purge : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract purge : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract purge : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract purge : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract purge : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract purge : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract purge : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract purge : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract purge : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract purge : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract purge : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract purge : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract purge : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract purge : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract purge : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract purge : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract purge : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract purge : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract purge : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract purge : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract purge : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract report: IRouterMatcher<IRouter> with get, set
    abstract report : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract report : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract report : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract report : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract report : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract report : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract report : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract report : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract report : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract report : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract report : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract report : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract report : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract report : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract report : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract report : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract report : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract report : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract report : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract report : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract report : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract report : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract report : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract report : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract search: IRouterMatcher<IRouter> with get, set
    abstract search : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract search : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract search : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract search : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract search : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract search : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract search : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract search : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract search : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract search : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract search : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract search : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract search : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract search : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract search : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract search : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract search : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract search : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract search : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract search : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract search : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract search : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract search : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract search : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract subscribe: IRouterMatcher<IRouter> with get, set
    abstract subscribe : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract subscribe : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract subscribe : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract subscribe : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract subscribe : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract subscribe : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract subscribe : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract subscribe : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract subscribe : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract subscribe : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract subscribe : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract subscribe : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract subscribe : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract subscribe : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract subscribe : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract subscribe : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract subscribe : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract subscribe : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract subscribe : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract subscribe : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract subscribe : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract subscribe : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract subscribe : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract subscribe : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract trace: IRouterMatcher<IRouter> with get, set
    abstract trace : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract trace : path : string * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract trace : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract trace : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract trace : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract trace : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract trace : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract trace : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract trace : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract trace : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract trace : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract trace : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract trace : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract trace : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract trace : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract trace : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract trace : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract trace : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract trace : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract trace : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract trace : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract trace : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract trace : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract trace : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract unlock: IRouterMatcher<IRouter> with get, set
    abstract unlock : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract unlock : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract unlock : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract unlock : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract unlock : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract unlock : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract unlock : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract unlock : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract unlock : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract unlock : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract unlock : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract unlock : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract unlock : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract unlock : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract unlock : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract unlock : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract unlock : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract unlock : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract unlock : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract unlock : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract unlock : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract unlock : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract unlock : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    // abstract unsubscribe: IRouterMatcher<IRouter> with get, set
    abstract unsubscribe : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract unsubscribe : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract unsubscribe : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract unsubscribe : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit
    abstract unsubscribe : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, unit>) array-> unit

    abstract unsubscribe : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract unsubscribe : path : string * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract unsubscribe : path : RegExp * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract unsubscribe : path : ResizeArray<string> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract unsubscribe : path : ResizeArray<RegExp> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit
    abstract unsubscribe : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers : (Func<Request, Response, NextFunction, unit>) array-> unit

    abstract unsubscribe : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract unsubscribe : path : string * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract unsubscribe : path : RegExp * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract unsubscribe : path : ResizeArray<string> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract unsubscribe : path : ResizeArray<RegExp> * [<ParamArray>] handlers: #RequestHandler array-> unit
    abstract unsubscribe : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: #RequestHandler array-> unit

    abstract unsubscribe : path : U3<string, RegExp, Array<U2<string, RegExp>>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract unsubscribe : path : string * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract unsubscribe : path : RegExp * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract unsubscribe : path : ResizeArray<string> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract unsubscribe : path : ResizeArray<RegExp> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit
    abstract unsubscribe : path : ResizeArray<U2<string, RegExp>> * [<ParamArray>] handlers: Func<Error option, Request, Response, NextFunction, unit> array-> unit

    abstract member ``use``: #IRouter -> unit
    abstract member ``use``: path : string * router : #IRouter -> unit
    abstract member ``use``: Func<Error option, Request, Response, NextFunction, unit> -> unit
    abstract member ``use``: Func<Request, Response, unit> -> unit
    abstract member ``use``: Func<Request, Response, NextFunction, unit> -> unit
    abstract member ``use``: path : string * Func<Error option, Request, Response, NextFunction, unit> -> unit
    abstract member ``use``: path : string * Func<Request, Response, NextFunction, unit> -> unit
    abstract member ``use``: path : string * Func<Request, Response, unit> -> unit
    // Overload to support APIs defined in the style of the Connect binding
    // For example, BodyParser, CookieParser
    abstract member ``use``: Func<#Http.IncomingMessage, #Http.ServerResponse, Func<obj, unit>, unit> -> unit
    abstract member ``use``: path : string * Func<#Http.IncomingMessage, #Http.ServerResponse, Func<obj, unit>, unit> -> unit
    // Original: abstract route: prefix: PathParams -> IRoute
    abstract route: prefix: string -> IRoute
    abstract route: prefix: RegExp -> IRoute
    abstract route: prefix: string array -> IRoute
    abstract route: prefix: RegExp array -> IRoute
    /// Stack of configured routes
    abstract stack: ResizeArray<obj option> with get, set

type [<AllowNullLiteral>] IRoute =
    abstract path: string with get, set
    abstract stack: obj option with get, set
    // Original: abstract all: IRouterHandler<IRoute> with get, set
    abstract all: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract all: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract all: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract get: IRouterHandler<IRoute> with get, set
    abstract get: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract get: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract get: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract post: IRouterHandler<IRoute> with get, set
    abstract post: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract post: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract post: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract put: IRouterHandler<IRoute> with get, set
    abstract put: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract put: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract put: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract delete: IRouterHandler<IRoute> with get, set
    abstract delete: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract delete: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract delete: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract patch: IRouterHandler<IRoute> with get, set
    abstract patch: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract patch: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract patch: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract options: IRouterHandler<IRoute> with get, set
    abstract options: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract options: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract options: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract head: IRouterHandler<IRoute> with get, set
    abstract head: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract head: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract head: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract checkout: IRouterHandler<IRoute> with get, set
    abstract checkout: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract checkout: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract checkout: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract copy: IRouterHandler<IRoute> with get, set
    abstract copy: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract copy: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract copy: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract lock: IRouterHandler<IRoute> with get, set
    abstract lock: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract lock: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract lock: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract merge: IRouterHandler<IRoute> with get, set
    abstract merge: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract merge: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract merge: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract mkactivity: IRouterHandler<IRoute> with get, set
    abstract mkactivity: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract mkactivity: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract mkactivity: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract mkcol: IRouterHandler<IRoute> with get, set
    abstract mkcol: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract mkcol: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract mkcol: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract move: IRouterHandler<IRoute> with get, set
    abstract move: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract move: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract move: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract ``m-search``: IRouterHandler<IRoute> with get, set
    abstract ``m-search``: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract ``m-search``: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract ``m-search``: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract notify: IRouterHandler<IRoute> with get, set
    abstract notify: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract notify: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract notify: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract purge: IRouterHandler<IRoute> with get, set
    abstract purge: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract purge: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract purge: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract report: IRouterHandler<IRoute> with get, set
    abstract report: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract report: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract report: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract search: IRouterHandler<IRoute> with get, set
    abstract search: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract search: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract search: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract subscribe: IRouterHandler<IRoute> with get, set
    abstract subscribe: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract subscribe: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract subscribe: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract trace: IRouterHandler<IRoute> with get, set
    abstract trace: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract trace: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract trace: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract unlock: IRouterHandler<IRoute> with get, set
    abstract unlock: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract unlock: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract unlock: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute
    // Original: abstract unsubscribe: IRouterHandler<IRoute> with get, set
    abstract unsubscribe: [<ParamArray>] handlers : Func<Request, Response, unit> array -> IRoute
    abstract unsubscribe: [<ParamArray>] handlers : Func<Request, Response, NextFunction, unit> array -> IRoute
    abstract unsubscribe: [<ParamArray>] handlers : Func<Error option, Request, Response, NextFunction, unit> array -> IRoute

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
    /// <summary>
    /// Return request header.
    ///
    /// The <c>Referrer</c> header field is special-cased,
    /// both `Referrer` and <c>Referer</c> are interchangeable.
    /// </summary>
    /// <example>
    ///
    ///      req.get('Content-Type');
    ///      // => "text/plain"
    ///
    ///      req.get('content-type');
    ///      // => "text/plain"
    ///
    ///      req.get('Something');
    ///      // => undefined
    /// </example>
    /// <remarks>
    /// Aliased as <c>req.header()</c>.
    /// </remarks>
    [<Emit "$0.get('set-cookie')">] abstract ``get_set-cookie``: unit -> ResizeArray<string> option
    abstract get: name: string -> string option
    [<Emit "$0.header('set-cookie')">] abstract ``header_set-cookie``: unit -> ResizeArray<string> option
    abstract header: name: string -> string option
    /// <summary>
    /// Check if the given <c>type(s)</c> is acceptable, returning
    /// the best match when true, otherwise <c>undefined</c>, in which
    /// case you should respond with 406 "Not Acceptable".
    ///
    /// The <c>type</c> value may be a single mime type string
    /// such as "application/json", the extension name
    /// such as "json", a comma-delimted list such as "json, html, text/plain",
    /// or an array <c>["json", "html", "text/plain"]</c>. When a list
    /// or array is given the _best_ match, if any is returned.
    ///
    /// Examples:
    ///
    /// <code lang="js">
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
    /// </code>
    /// </summary>
    abstract accepts: unit -> ResizeArray<string>
    abstract accepts: ``type``: string -> string
    abstract accepts: ``type``: ResizeArray<string> -> string
    abstract accepts: [<ParamArray>] ``type``: string[] -> string
    /// <summary>
    /// Returns the first accepted charset of the specified character sets,
    /// based on the request's Accept-Charset HTTP header field.
    /// If none of the specified charsets is accepted, returns false.
    ///
    /// For more information, or if you have issues or concerns, see accepts.
    /// </summary>
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
    /// <summary>
    /// Returns the first accepted language of the specified languages,
    /// based on the request's Accept-Language HTTP header field.
    /// If none of the specified languages is accepted, returns false.
    ///
    /// For more information, or if you have issues or concerns, see accepts.
    /// </summary>
    abstract acceptsLanguages: unit -> ResizeArray<string>
    abstract acceptsLanguages: lang: string -> string
    abstract acceptsLanguages: lang: ResizeArray<string> -> string
    abstract acceptsLanguages: [<ParamArray>] lang: string[] -> string
    /// <summary>
    /// Parse Range header field, capping to the given <c>size</c>.
    ///
    /// Unspecified ranges such as "0-" require knowledge of your resource length. In
    /// the case of a byte range this is of course the total number of bytes.
    /// If the Range header field is not given <c>undefined</c> is returned.
    /// If the Range header field is given, return value is a result of range-parser.
    /// See more ./types/range-parser/index.d.ts
    /// <remarks>
    /// Remember that ranges are inclusive, so for example "Range: users=0-3"
    /// should respond with 4 users when available, not 3.
    /// </remarks>
    /// </summary>
    abstract range: size: float * ?options: RangeParserOptions -> U2<RangeParserRanges, RangeParserResult> option
    /// <summary>
    /// Return an array of Accepted media types
    /// ordered from highest quality to lowest.
    /// </summary>
    abstract accepted: ResizeArray<MediaType> with get, set
    [<Obsolete("since 4.11.0 - Use req.params, req.body, or req.query instead")>]
    abstract param: name: string * ?defaultValue: obj -> string
    /// <summary>
    /// Check if the incoming request contains the "Content-Type"
    /// header field, and it contains the give mime <c>type</c>.
    /// </summary>
    /// <example>
    ///
    /// <code lang="js">
    ///
    /// // With Content-Type: text/html; charset=utf-8
    /// req.is('html');
    /// req.is('text/html');
    /// req.is('text/*');
    /// // => true
    ///
    /// // When Content-Type is application/json
    /// req.is('json');
    /// req.is('application/json');
    /// req.is('application/*');
    /// // => true
    ///
    /// req.is('html');
    /// // => false
    /// </code>
    /// </example>
    abstract is: ``type``: U2<string, ResizeArray<string>> -> U2<string option, bool>
    /// <summary>
    /// Return the protocol string "http" or "https"
    /// when requested with TLS. When the "trust proxy"
    /// setting is enabled the "X-Forwarded-Proto" header
    /// field will be trusted. If you're running behind
    /// a reverse proxy that supplies https for you this
    /// may be enabled.
    /// </summary>
    abstract protocol: string with get, set
    /// <summary>
    /// Short-hand for:
    ///
    ///     req.protocol == 'https'
    /// </summary>
    abstract secure: bool with get, set
    /// <summary>
    /// Return the remote address, or when
    /// "trust proxy" is <c>true</c> return
    /// the upstream addr.
    /// </summary>
    abstract ip: string with get, set
    /// <summary>
    /// When "trust proxy" is <c>true</c>, parse the "X-Forwarded-For" ip address list.
    ///
    /// For example if the value were "client, proxy1, proxy2"
    /// you would receive the array <c>["client", "proxy1", "proxy2"]</c>
    /// where "proxy2" is the furthest down-stream.
    /// </summary>
    abstract ips: ResizeArray<string> with get, set
    /// <summary>
    /// Return subdomains as an array.
    ///
    /// Subdomains are the dot-separated parts of the host before the main domain of
    /// the app. By default, the domain of the app is assumed to be the last two
    /// parts of the host. This can be changed by setting "subdomain offset".
    ///
    /// For example, if the domain is "tobi.ferrets.example.com":
    /// If "subdomain offset" is not set, req.subdomains is <c>["ferrets", "tobi"]</c>.
    /// If "subdomain offset" is 3, req.subdomains is <c>["tobi"]</c>.
    /// </summary>
    abstract subdomains: ResizeArray<string> with get, set
    /// <summary>
    /// Short-hand for <c>url.parse(req.url).pathname</c>.
    /// </summary>
    abstract path: string with get, set
    /// <summary>
    /// Parse the "Host" header field hostname.
    /// </summary>
    abstract hostname: string with get, set
    abstract host: string with get, set
    /// <summary>
    /// Check if the request is fresh, aka
    /// Last-Modified and/or the ETag
    /// still match.
    /// </summary>
    abstract fresh: bool with get, set
    /// <summary>
    /// Check if the request is stale, aka
    /// "Last-Modified" and / or the "ETag" for the
    /// resource has changed.
    /// </summary>
    abstract stale: bool with get, set
    /// <summary>
    /// Check if the request was an _XMLHttpRequest_.
    /// </summary>
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
    /// <summary>
    /// After middleware.init executed, Request will contain res and next properties
    /// See: express/lib/middleware/init.js
    /// </summary>
    abstract res: Response<'ResBody, 'Locals> option with get, set
    abstract next: NextFunction option with get, set

type [<AllowNullLiteral>] MediaType =
    abstract value: string with get, set
    abstract quality: float with get, set
    abstract ``type``: string with get, set
    abstract subtype: string with get, set

// type Send =
//     Send<obj option, Response<obj option>>

// type Send<'ResBody> =
//     Send<'ResBody, Response<'ResBody>>

// type [<AllowNullLiteral>] Send<'ResBody, 'T> =
//     [<Emit "$0($1...)">] abstract Invoke: ?body: 'ResBody -> 'T

type Response =
    Response<obj, Dictionary<obj option>, int>

type Response<'ResBody> =
    Response<'ResBody, Dictionary<obj option>, int>

type Response<'ResBody, 'Locals when 'Locals :> Dictionary<obj option>> =
    Response<'ResBody, 'Locals, int>

type [<AllowNullLiteral>] Response<'ResBody, 'Locals, 'StatusCode when 'Locals :> Dictionary<obj option>> =
    inherit Http.ServerResponse
    inherit Express.Response
    /// Set status <c>code</c>.
//    abstract status: code: 'StatusCode -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract status: code: 'StatusCode -> unit
    /// Set the response HTTP status code to <c>statusCode</c> and send its string representation as the response body.
    abstract sendStatus: code: 'StatusCode -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Set Link header field with the given <c>links</c>.
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
    abstract json: ?body: 'ResBody -> unit
    /// Send JSON response with JSONP callback support.
    ///
    /// Examples:
    ///
    ///      res.jsonp(null);
    ///      res.jsonp({ user: 'tj' });
    ///      res.status(500).jsonp('oh noes!');
    ///      res.status(404).jsonp('I dont have that');
    abstract jsonp : ?body: 'ResBody -> unit
    /// Transfer the file at the given <c>path</c>.
    ///
    /// Automatically sets the _Content-Type_ response header field.
    /// The callback <c>fn(err)</c> is invoked when the transfer is complete
    /// or when an error occurs. Be sure to check <c>res.headersSent</c>
    /// if you wish to attempt responding, as the header and some data
    /// may have already been transferred.
    ///
    /// Options:
    ///
    ///    - `maxAge`   defaulting to 0 (can be string converted by <c>ms</c>)
    ///    - <c>root</c>     root directory for relative filenames
    ///    - <c>headers</c>  object of headers to serve with file
    ///    - `dotfiles` serve dotfiles, defaulting to false; can be <c>"allow"</c> to send them
    ///
    /// Other options are passed along to <c>send</c>.
    ///
    /// Examples:
    ///
    ///   The following example illustrates how <c>res.sendFile()</c> may
    ///   be used as an alternative for the <c>static()</c> middleware for
    ///   dynamic situations. The code backing <c>res.sendFile()</c> is actually
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
    /// Transfer the file at the given <c>path</c> as an attachment.
    ///
    /// Optionally providing an alternate attachment <c>filename</c>,
    /// and optional callback <c>fn(err)</c>. The callback is invoked
    /// when the data transfer is complete, or when an error has
    /// ocurred. Be sure to check <c>res.headersSent</c> if you plan to respond.
    ///
    /// The optional options argument passes through to the underlying
    /// res.sendFile() call, and takes the exact same parameters.
    ///
    /// This method uses <c>res.sendfile()</c>.
    abstract download: path: string * ?fn: Errback -> unit
    abstract download: path: string * filename: string * ?fn: Errback -> unit
    abstract download: path: string * filename: string * options: obj option * ?fn: Errback -> unit
    /// Set _Content-Type_ response header with `type` through <c>mime.lookup()</c>
    /// when it does not contain "/", or set the Content-Type to <c>type</c> otherwise.
    ///
    /// Examples:
    ///
    ///      res.type('.html');
    ///      res.type('html');
    ///      res.type('json');
    ///      res.type('application/json');
    ///      res.type('png');
    abstract contentType: ``type``: string -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Set _Content-Type_ response header with `type` through <c>mime.lookup()</c>
    /// when it does not contain "/", or set the Content-Type to <c>type</c> otherwise.
    ///
    /// Examples:
    ///
    ///      res.type('.html');
    ///      res.type('html');
    ///      res.type('json');
    ///      res.type('application/json');
    ///      res.type('png');
    abstract ``type``: ``type``: string -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Respond to the Acceptable formats using an <c>obj</c>
    /// of mime-type callbacks.
    ///
    /// This method uses <c>req.accepted</c>, an array of
    /// acceptable types ordered by their quality values.
    /// When "Accept" is not present the _first_ callback
    /// is invoked, otherwise the first match is used. When
    /// no match is performed the server responds with
    /// 406 "Not Acceptable".
    ///
    /// Content-Type is set for you, however if you choose
    /// you may alter this within the callback using <c>res.type()</c>
    /// or <c>res.set('Content-Type', ...)</c>.
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
    /// By default Express passes an <c>Error</c>
    /// with a `.status` of 406 to <c>next(err)</c>
    /// if a match is not made. If you provide
    /// a <c>.default</c> callback it will be invoked
    /// instead.
    abstract format: obj: obj option -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Set _Content-Disposition_ header to _attachment_ with optional <c>filename</c>.
    abstract attachment: ?filename: string -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Set header `field` to <c>val</c>, or pass
    /// an object of header fields.
    ///
    /// Examples:
    ///
    ///     res.set('Foo', ['bar', 'baz']);
    ///     res.set('Accept', 'application/json');
    ///     res.set({ Accept: 'text/plain', 'X-API-Key': 'tobi' });
    ///
    /// Aliased as <c>res.header()</c>.
    abstract set: field: obj option -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract set: field: string * ?value: U2<string, ResizeArray<string>> -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract set: field: string * ?value: ResizeArray<string> -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract set: field: string * ?value: string-> Response<'ResBody, 'Locals, 'StatusCode>
    abstract header: field: obj option -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract header: field: string * ?value: U2<string, ResizeArray<string>> -> Response<'ResBody, 'Locals, 'StatusCode>
    abstract headersSent: bool with get, set
    /// Get value for header <c>field</c>.
    abstract get: field: string -> string
    /// Clear cookie <c>name</c>.
    abstract clearCookie: name: string * ?options: obj -> Response<'ResBody, 'Locals, 'StatusCode>
    /// Set cookie `name` to `val`, with the given <c>options</c>.
    ///
    /// Options:
    ///
    ///     - `maxAge`   max-age in milliseconds, converted to <c>expires</c>
    ///     - <c>signed</c>   sign the cookie
    ///     - <c>path</c>     defaults to "/"
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
    /// Set the location header to <c>url</c>.
    ///
    /// The given <c>url</c> can also be the name of a mapped url, for
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
    ///    When an application is mounted and <c>res.location()</c>
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
    /// Redirect to the given `url` with optional response <c>status</c>
    /// defaulting to 302.
    ///
    /// The resulting `url` is determined by <c>res.location()</c>, so
    /// it will play nicely with mounted apps, relative paths,
    /// <c>"back"</c> etc.
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
    /// Render `view` with the given `options` and optional callback <c>fn</c>.
    /// When a callback function is given a response will _not_ be made
    /// automatically, otherwise a response of _200_ and _text/html_ is given.
    ///
    /// Options:
    ///
    ///   - <c>cache</c>     boolean hinting to the engine it should cache
    ///   - <c>filename</c>  filename of the view being rendered
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
    /// Register the given template engine callback <c>fn</c>
    /// as <c>ext</c>.
    ///
    /// By default will <c>require()</c> the engine based on the
    /// file extension. For example if you try to render
    /// a "foo.jade" file Express will invoke the following internally:
    ///
    ///      app.engine('jade', require('jade').__express);
    ///
    /// For engines that do not provide <c>.__express</c> out of the box,
    /// or if you wish to "map" a different extension to the template engine
    /// you may use this method. For example mapping the EJS template engine to
    /// ".html" files:
    ///
    ///      app.engine('html', require('ejs').renderFile);
    ///
    /// In this case EJS provides a <c>.renderFile()</c> method with
    /// the same signature that Express expects: <c>(path, options, callback)</c>,
    /// though note that it aliases this method as <c>ejs.__express</c> internally
    /// so if you're using ".ejs" extensions you dont need to do anything.
    ///
    /// Some template engines do not follow this convention, the
    /// [Consolidate.js](https://github.com/visionmedia/consolidate.js)
    /// library was created to map all of node's popular template
    /// engines to follow this convention, thus allowing them to
    /// work seamlessly within Express.
    // abstract engine: ext: string * fn: (string -> obj -> (obj option -> string -> unit) -> unit) -> Application

    abstract engine: ext : string * fn: (string -> #obj -> EngineRenderFunc -> unit) -> unit
    /// Assign `setting` to `val`, or return <c>setting</c>'s value.
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
    /// Map the given param placeholder <c>name</c>(s) to the given callback(s).
    ///
    /// Parameter mapping is used to provide pre-conditions to routes
    /// which use normalized placeholders. For example a _:user_id_ parameter
    /// could automatically load a user's information from the database without
    /// any additional code,
    ///
    /// The callback uses the samesignature as middleware, the only differencing
    /// being that the value of the placeholder is passed, in this case the _id_
    /// of the user. Once the <c>next()</c> function is invoked, just like middleware
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
    /// <summary>
    /// Check if <c>setting</c> is enabled (truthy).
    ///
    /// <code lang="fsharp">
    /// app.enabled('foo')
    /// // => false
    ///
    /// app.enable('foo')
    /// app.enabled('foo')
    /// // => true
    /// </code>
    /// </summary>
    abstract enabled: setting: string -> bool
    /// <summary>
    /// Check if <c>setting</c> is disabled.
    ///
    /// <code lang="fsharp">
    /// app.disabled('foo')
    /// // => true
    ///
    /// app.enable('foo')
    /// app.disabled('foo')
    /// // => false
    /// </code>
    /// </summary>
    abstract disabled: setting: string -> bool
    /// <summary>
    /// Enable <c>setting</c>.
    /// </summary>
    abstract enable: setting: string -> Application
    /// <summary>
    /// Disable <c>setting</c>.
    /// </summary>
    abstract disable: setting: string -> Application
    /// <summary>
    /// Render the given view `name` name with <c>options</c>
    /// and a callback accepting an error and the
    /// rendered template string.
    /// </summary>
    /// <example>
    ///     app.render('email', { name: 'Tobi' }, function(err, html){
    ///       // ...
    ///     })
    /// </example>
    abstract render: name: string * ?options: obj * ?callback: (Error option -> string -> unit) -> unit
    abstract render: name: string * callback: (Error option -> string -> unit) -> unit
    /// <summary>
    /// Listen for connections.
    ///
    /// A node <c>http.Server</c> is returned, with this
    /// application (which is a <c>Function</c>) as its
    /// callback. If you wish to create both an HTTP
    /// and HTTPS server you may do so with the "http"
    /// and "https" modules as shown here:
    /// </summary>
    /// <example>
    ///     var http = require('http')
    ///       , https = require('https')
    ///       , express = require('express')
    ///       , app = express();
    ///
    ///     http.createServer(app).listen(80);
    ///     https.createServer({ ... }, app).listen(443);
    /// </example>
    abstract listen: port: int * hostname: string * backlog: int * ?callback: (unit -> unit) -> Http.Server
    /// <summary>
    /// Listen for connections.
    ///
    /// A node <c>http.Server</c> is returned, with this
    /// application (which is a <c>Function</c>) as its
    /// callback. If you wish to create both an HTTP
    /// and HTTPS server you may do so with the "http"
    /// and "https" modules as shown here:
    /// </summary>
    /// <example>
    ///     var http = require('http')
    ///       , https = require('https')
    ///       , express = require('express')
    ///       , app = express();
    ///
    ///     http.createServer(app).listen(80);
    ///     https.createServer({ ... }, app).listen(443);
    /// </example>
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
    /// <summary>
    /// The app.routes object houses all of the routes defined mapped by the
    /// associated HTTP verb. This object may be used for introspection
    /// capabilities, for example Express uses this internally not only for
    /// routing but to provide default OPTIONS behaviour unless app.options()
    /// is used. Your application or framework may also remove routes by
    /// simply by removing them from this object.
    /// </summary>
    abstract routes: obj option with get, set
    /// <summary>
    /// Used to get all registered routes in Express Application
    /// </summary>
    abstract _router: obj option with get, set
//    abstract ``use``: ApplicationRequestHandler<Application> with get, set
    /// <summary>
    /// The mount event is fired on a sub-app, when it is mounted on a parent app.
    /// The parent app is passed to the callback function.
    ///
    /// <remarks>
    /// Sub-apps will:
    ///   - Not inherit the value of settings that have a default value. You must set the value in the sub-app.
    ///   - Inherit the value of settings with no default value.
    /// </remarks>
    /// </summary>
    abstract on: string * (Application -> unit) -> Application
    /// <summary>
    /// The app.mountpath property contains one or more path patterns on which a sub-app was mounted.
    /// </summary>
    abstract mountpath: U2<string, ResizeArray<string>> with get, set

type [<AllowNullLiteral>] Express =
    inherit Application
    abstract request: Request with get, set
    abstract response: Response with get, set
