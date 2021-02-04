// ts2fable 0.8.0
module rec Express
open System
open Fable.Core
open Fable.Core.JS

module BodyParser = Npm.Types.BodyParser
module ServeStatic = ServeStatic.Types.ServeStatic
module Core = ExpressServeStaticCore

let [<Import("default","express")>] e : E.IExports = jsNative

module E =

    type [<AllowNullLiteral>] IExports =
        abstract json: obj
        abstract raw: obj
        abstract text: obj
        abstract application: Application
        abstract request: Request
        abstract response: Response
        abstract ``static``: ServeStatic.Types.ServeStatic.RequestHandlerConstructor<Response>
        abstract urlencoded: obj
        /// This is a built-in middleware function in Express. It parses incoming request query parameters.
        abstract query: options: U2<Npm.Types.Qs.IParseOptions, obj> -> Handler
        abstract Router: ?options: RouterOptions -> Core.Router
        /// <summary>
        /// Creates an Express application. The express() function is a top-level function exported by the express module.
        /// </summary>
        [<Emit("$0()")>]
        abstract express : unit -> Express

    type [<AllowNullLiteral>] RouterOptions =
        /// Enable case sensitivity.
        abstract caseSensitive: bool option with get, set
        /// <summary>
        /// Preserve the req.params values from the parent router.
        /// If the parent and the child have conflicting param names, the child’s value take precedence.
        /// </summary>
        /// <default>false</default>
        abstract mergeParams: bool option with get, set
        /// Enable strict routing.
        abstract strict: bool option with get, set

    type [<AllowNullLiteral>] Application =
        inherit Core.Application

    type [<AllowNullLiteral>] CookieOptions =
        inherit Core.CookieOptions

    type [<AllowNullLiteral>] Errback =
        inherit Core.Errback

    type ErrorRequestHandler =
        ErrorRequestHandler<Core.ParamsDictionary, obj option, obj option, Core.Query, Core.Dictionary<obj option>>

    type ErrorRequestHandler<'P> =
        ErrorRequestHandler<'P, obj option, obj option, Core.Query, Core.Dictionary<obj option>>

    type ErrorRequestHandler<'P, 'ResBody> =
        ErrorRequestHandler<'P, 'ResBody, obj option, Core.Query, Core.Dictionary<obj option>>

    type ErrorRequestHandler<'P, 'ResBody, 'ReqBody> =
        ErrorRequestHandler<'P, 'ResBody, 'ReqBody, Core.Query, Core.Dictionary<obj option>>

    type ErrorRequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery> =
        ErrorRequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery, Core.Dictionary<obj option>>

    type ErrorRequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals when 'Locals :> Core.Dictionary<obj option>> =
        Core.ErrorRequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>

    type Express =
        Core.Express

    type Handler =
        Core.Handler

    type [<AllowNullLiteral>] IRoute =
        inherit Core.IRoute

    type [<AllowNullLiteral>] IRouter =
        inherit Core.IRouter

    type [<AllowNullLiteral>] IRouterHandler<'T> =
        inherit Core.IRouterHandler<'T>

    type [<AllowNullLiteral>] IRouterMatcher<'T> =
        inherit Core.IRouterMatcher<'T>

    type [<AllowNullLiteral>] MediaType =
        inherit Core.MediaType

    type [<AllowNullLiteral>] NextFunction =
        inherit Core.NextFunction

    type Request =
        Request<Core.ParamsDictionary, obj option, obj option, Core.Query, Core.Dictionary<obj option>>

    type Request<'P> =
        Request<'P, obj option, obj option, Core.Query, Core.Dictionary<obj option>>

    type Request<'P, 'ResBody> =
        Request<'P, 'ResBody, obj option, Core.Query, Core.Dictionary<obj option>>

    type Request<'P, 'ResBody, 'ReqBody> =
        Request<'P, 'ResBody, 'ReqBody, Core.Query, Core.Dictionary<obj option>>

    type Request<'P, 'ResBody, 'ReqBody, 'ReqQuery> =
        Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, Core.Dictionary<obj option>>

    type [<AllowNullLiteral>] Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals when 'Locals :> Core.Dictionary<obj option>> =
        inherit Core.Request<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>

    type RequestHandler =
        RequestHandler<Core.ParamsDictionary, obj option, obj option, Core.Query, Core.Dictionary<obj option>>

    type RequestHandler<'P> =
        RequestHandler<'P, obj option, obj option, Core.Query, Core.Dictionary<obj option>>

    type RequestHandler<'P, 'ResBody> =
        RequestHandler<'P, 'ResBody, obj option, Core.Query, Core.Dictionary<obj option>>

    type RequestHandler<'P, 'ResBody, 'ReqBody> =
        RequestHandler<'P, 'ResBody, 'ReqBody, Core.Query, Core.Dictionary<obj option>>

    type RequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery> =
        RequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery, Core.Dictionary<obj option>>

    type RequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals when 'Locals :> Core.Dictionary<obj option>> =
        Core.RequestHandler<'P, 'ResBody, 'ReqBody, 'ReqQuery, 'Locals>

    type RequestParamHandler =
        Core.RequestParamHandler

    type Response =
        Response<obj option, Core.Dictionary<obj option>>

    type Response<'ResBody> =
        Response<'ResBody, Core.Dictionary<obj option>>

    type [<AllowNullLiteral>] Response<'ResBody, 'Locals when 'Locals :> Core.Dictionary<obj option>> =
        inherit Core.Response<'ResBody, 'Locals>

    type [<AllowNullLiteral>] Router =
        inherit Core.Router

    type [<AllowNullLiteral>] Send =
        inherit Core.Send
