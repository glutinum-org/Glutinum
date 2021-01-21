namespace rec ServeStatic.Types

// Exported from: https://github.com/DefinitelyTyped/DefinitelyTyped/blob/0893371fea43bfdf1777b6d835424961ba0d1dbb/types/mime/index.d.ts

open System
open Fable.Core
open Fable.Core.JS
open Node

type [<AllowNullLiteral>] IExports =
    /// Create a new middleware function to serve files from within a given root directory.
    /// The file to serve will be determined by combining req.url with the provided root directory.
    /// When a file is not found, instead of sending a 404 response, this module will instead call next() to move on to the next middleware, allowing for stacking and fall-backs.
    [<Emit("$0($1...)")>]
    abstract serveStatic: root: string * ?options: ServeStatic.ServeStaticOptions<'R> -> ServeStatic.RequestHandler<'R> when 'R :> Http.ServerResponse

    abstract mime : Npm.Types.Mime.IExports


module ServeStatic =

    type ServeStaticOptions =
        ServeStaticOptions<Http.ServerResponse>

    type [<AllowNullLiteral>] ServeStaticOptions<'R when 'R :> Http.ServerResponse> =
        /// Enable or disable setting Cache-Control response header, defaults to true.
        /// Disabling this will ignore the immutable and maxAge options.
        abstract cacheControl: bool with get, set
        /// Set how "dotfiles" are treated when encountered. A dotfile is a file or directory that begins with a dot (".").
        /// Note this check is done on the path itself without checking if the path actually exists on the disk.
        /// If root is specified, only the dotfiles above the root are checked (i.e. the root itself can be within a dotfile when when set to "deny").
        /// The default value is 'ignore'.
        /// 'allow' No special treatment for dotfiles
        /// 'deny' Send a 403 for any request for a dotfile
        /// 'ignore' Pretend like the dotfile does not exist and call next()
        abstract dotfiles: string with get, set
        /// Enable or disable etag generation, defaults to true.
        abstract etag: bool with get, set
        /// Set file extension fallbacks. When set, if a file is not found, the given extensions will be added to the file name and search for.
        /// The first that exists will be served. Example: ['html', 'htm'].
        /// The default value is false.
        abstract extensions: ResizeArray<string> with get, set
        /// Let client errors fall-through as unhandled requests, otherwise forward a client error.
        /// The default value is true.
        abstract fallthrough: bool with get, set
        /// Enable or disable the immutable directive in the Cache-Control response header.
        /// If enabled, the maxAge option should also be specified to enable caching. The immutable directive will prevent supported clients from making conditional requests during the life of the maxAge option to check if the file has changed.
        abstract immutable: bool with get, set
        /// By default this module will send "index.html" files in response to a request on a directory.
        /// To disable this set false or to supply a new index pass a string or an array in preferred order.
        abstract index: U3<bool, string, ResizeArray<string>> with get, set
        /// Enable or disable Last-Modified header, defaults to true. Uses the file system's last modified value.
        abstract lastModified: bool with get, set
        /// Provide a max-age in milliseconds for http caching, defaults to 0. This can also be a string accepted by the ms module.
        abstract maxAge: U2<float, string> with get, set
        /// Redirect to trailing "/" when the pathname is a dir. Defaults to true.
        abstract redirect: bool with get, set
        /// Function to set custom headers on response. Alterations to the headers need to occur synchronously.
        /// The function is called as fn(res, path, stat), where the arguments are:
        /// res the response object
        /// path the file path that is being sent
        /// stat the stat object of the file that is being sent
        abstract setHeaders: ('R -> string -> obj option -> obj option) with get, set

    type [<AllowNullLiteral>] RequestHandler<'R when 'R :> Http.ServerResponse> =
        [<Emit "$0($1...)">] abstract Invoke: request: Http.IncomingMessage * response: 'R * next: Npm.Types.Connect.CreateServer.NextFunction -> obj option

    type [<AllowNullLiteral>] RequestHandlerConstructor<'R when 'R :> Http.ServerResponse> =
        [<Emit "$0($1...)">] abstract Invoke: root: string * ?options: ServeStaticOptions<'R> -> RequestHandler<'R>
        abstract mime: obj with get, set
