namespace rec Connect.Types

open System
open Fable.Core
open Fable.Core.JS
open Node

type Function = System.Action

type [<AllowNullLiteral>] IExports =
    /// Create a new connect server.
    [<Emit("$0()")>]
    abstract Invoke: unit -> CreateServer.Server

module CreateServer =

    // type [<AllowNullLiteral>] IExports =
    //     abstract IncomingMessage: IncomingMessageStatic

    type ServerHandle =
        U2<HandleFunction, Http.Server>

    type [<AllowNullLiteral>] IncomingMessage =
        inherit Http.IncomingMessage
        abstract originalUrl: Http.IncomingMessage option with get, set

    type [<AllowNullLiteral>] IncomingMessageStatic =
        [<EmitConstructor>] abstract Create: unit -> IncomingMessage

    type [<AllowNullLiteral>] NextFunction =
        [<Emit "$0($1...)">] abstract Invoke: ?err: obj -> unit

    type SimpleHandleFunction =
         IncomingMessage -> Http.ServerResponse -> unit

    type NextHandleFunction =
        IncomingMessage -> Http.ServerResponse -> NextFunction -> unit

    type ErrorHandleFunction =
        obj option -> IncomingMessage -> Http.ServerResponse -> NextFunction -> unit

    type HandleFunction =
        U3<SimpleHandleFunction, NextHandleFunction, ErrorHandleFunction>

    type [<AllowNullLiteral>] ServerStackItem =
        abstract route: string with get, set
        abstract handle: ServerHandle with get, set

    type [<AllowNullLiteral>] Server =
        inherit Events.EventEmitter
        [<Emit "$0($1...)">] abstract Invoke: req: Http.IncomingMessage * res: Http.ServerResponse * ?next: Function -> unit
        abstract route: string with get, set
        abstract stack: ResizeArray<ServerStackItem> with get, set
        /// Utilize the given middleware `handle` to the given `route`,
        /// defaulting to _/_. This "route" is the mount-point for the
        /// middleware, when given a value other than _/_ the middleware
        /// is only effective when that segment is present in the request's
        /// pathname.
        ///
        /// For example if we were to mount a function at _/admin_, it would
        /// be invoked on _/admin_, and _/admin/settings_, however it would
        /// not be invoked for _/_, or _/posts_.
        // abstract ``use``: fn: NextHandleFunction -> Server
        abstract ``use``: fn: SimpleHandleFunction -> Server
        abstract ``use``: fn: NextHandleFunction -> Server
        abstract ``use``: fn: ErrorHandleFunction -> Server
        abstract ``use``: fn: HandleFunction -> Server
        // abstract ``use``: route: string * fn: NextHandleFunction -> Server
        abstract ``use``: route: string * fn: HandleFunction -> Server
        abstract ``use``: route: string * fn: SimpleHandleFunction -> Server
        abstract ``use``: route: string * fn: NextHandleFunction -> Server
        abstract ``use``: route: string * fn: ErrorHandleFunction -> Server
        /// Handle server requests, punting them down
        /// the middleware stack.
        abstract handle: req: Http.IncomingMessage * res: Http.ServerResponse * next: Function -> unit
        /// Listen for connections.
        ///
        /// This method takes the same arguments
        /// as node's `http.Server#listen()`.
        ///
        /// HTTP and HTTPS:
        ///
        /// If you run your application both as HTTP
        /// and HTTPS you may wrap them individually,
        /// since your Connect "server" is really just
        /// a JavaScript `Function`.
        ///
        ///       var connect = require('connect')
        ///         , http = require('http')
        ///         , https = require('https');
        ///
        ///       var app = connect();
        ///
        ///       http.createServer(app).listen(80);
        ///       https.createServer(options, app).listen(443);
        abstract listen: port: float * ?hostname: string * ?backlog: float * ?callback: Function -> Http.Server
        abstract listen: port: float * ?hostname: string * ?callback: Function -> Http.Server
        abstract listen: path: string * ?callback: Function -> Http.Server
        abstract listen: handle: obj option * ?listeningListener: Function -> Http.Server
