module rec Glutinum.SuperAgent

open System
open Fable.Core
open Fable.Core.JS
open Node
open Browser

[<Import("*", "superagent")>]
let superagent : SuperAgent.Request.SuperAgentStatic = jsNative

type Error = Exception

type [<AllowNullLiteral>] CallbackHandler =
    [<Emit "$0($1...)">] abstract Invoke: err: obj option * res: Request.Response -> unit

type [<AllowNullLiteral>] Serializer =
    [<Emit "$0($1...)">] abstract Invoke: obj: obj option -> string

type [<AllowNullLiteral>] BrowserParser =
    [<Emit "$0($1...)">] abstract Invoke: str: string -> obj option

type [<AllowNullLiteral>] NodeParser =
    [<Emit "$0($1...)">] abstract Invoke: res: Request.Response * callback: (Error option -> obj option -> unit) -> unit

type Parser =
    U2<BrowserParser, NodeParser>

type MultipartValueSingle =
    U6<Types.Blob, Buffer, Fs.ReadStream<obj>, string, bool, float>

type MultipartValue =
    U2<MultipartValueSingle, ResizeArray<MultipartValueSingle>>

module Request =

    type [<AllowNullLiteral>] SuperAgentRequest =
        inherit Request
        abstract agent: ?agent: Http.Agent -> SuperAgentRequest
        abstract cookies: string with get, set
        abstract ``method``: string with get, set
        abstract url: string with get, set

    type [<AllowNullLiteral>] SuperAgentStatic =
        inherit SuperAgent<SuperAgentRequest>
        [<Emit "$0($1...)">] abstract Invoke: url: string -> SuperAgentRequest
        [<Emit "$0($1...)">] abstract Invoke: ``method``: string * url: string -> SuperAgentRequest
        abstract agent: unit -> obj
        abstract serialize: SuperAgentStaticSerialize with get, set
        abstract parse: SuperAgentStaticParse with get, set

    type [<AllowNullLiteral>] SuperAgent<'Req when 'Req :> SuperAgentRequest> =
        inherit Stream.Stream
        // abstract jar: Cookiejar.CookieJar with get, set
        abstract attachCookies: req: 'Req -> unit
        abstract checkout: url: string * ?callback: CallbackHandler -> 'Req
        abstract connect: url: string * ?callback: CallbackHandler -> 'Req
        abstract copy: url: string * ?callback: CallbackHandler -> 'Req
        abstract del: url: string * ?callback: CallbackHandler -> 'Req
        abstract delete: url: string * ?callback: CallbackHandler -> 'Req
        abstract get: url: string * ?callback: CallbackHandler -> 'Req
        abstract head: url: string * ?callback: CallbackHandler -> 'Req
        abstract lock: url: string * ?callback: CallbackHandler -> 'Req
        abstract merge: url: string * ?callback: CallbackHandler -> 'Req
        abstract mkactivity: url: string * ?callback: CallbackHandler -> 'Req
        abstract mkcol: url: string * ?callback: CallbackHandler -> 'Req
        abstract move: url: string * ?callback: CallbackHandler -> 'Req
        abstract notify: url: string * ?callback: CallbackHandler -> 'Req
        abstract options: url: string * ?callback: CallbackHandler -> 'Req
        abstract patch: url: string * ?callback: CallbackHandler -> 'Req
        abstract post: url: string * ?callback: CallbackHandler -> 'Req
        abstract propfind: url: string * ?callback: CallbackHandler -> 'Req
        abstract proppatch: url: string * ?callback: CallbackHandler -> 'Req
        abstract purge: url: string * ?callback: CallbackHandler -> 'Req
        abstract put: url: string * ?callback: CallbackHandler -> 'Req
        abstract report: url: string * ?callback: CallbackHandler -> 'Req
        abstract saveCookies: res: Response -> unit
        abstract search: url: string * ?callback: CallbackHandler -> 'Req
        abstract subscribe: url: string * ?callback: CallbackHandler -> 'Req
        abstract trace: url: string * ?callback: CallbackHandler -> 'Req
        abstract unlock: url: string * ?callback: CallbackHandler -> 'Req
        abstract unsubscribe: url: string * ?callback: CallbackHandler -> 'Req

    type [<AllowNullLiteral;AbstractClass>] ResponseError =
        inherit Error
        abstract status: float option with get, set
        abstract response: Response option with get, set

    type [<AllowNullLiteral;AbstractClass>] HTTPError =
        inherit Error
        abstract status: float with get, set
        abstract text: string with get, set
        abstract ``method``: string with get, set
        abstract path: string with get, set

    type [<AllowNullLiteral>] Response =
        inherit Stream.Stream
        abstract accepted: bool with get, set
        abstract badRequest: bool with get, set
        abstract body: obj option with get, set
        abstract charset: string with get, set
        abstract clientError: bool with get, set
        abstract error: HTTPError with get, set
        abstract files: obj option with get, set
        abstract forbidden: bool with get, set
        abstract get: header: string -> string
        [<Emit "$0.get('Set-Cookie')">] abstract ``get_Set-Cookie``: unit -> ResizeArray<string>
        abstract header: obj option with get, set
        abstract headers: obj option with get, set
        abstract info: bool with get, set
        abstract links: obj with get, set
        abstract noContent: bool with get, set
        abstract notAcceptable: bool with get, set
        abstract notFound: bool with get, set
        abstract ok: bool with get, set
        abstract redirect: bool with get, set
        abstract serverError: bool with get, set
        abstract status: float with get, set
        abstract statusType: float with get, set
        abstract text: string with get, set
        abstract ``type``: string with get, set
        abstract unauthorized: bool with get, set
        // abstract xhr: XMLHttpRequest with get, set
        abstract redirects: ResizeArray<string> with get, set

    type [<AllowNullLiteral>] Request =
        inherit Promise<Response>
        abstract abort: unit -> unit
        abstract accept: ``type``: string -> Request
        abstract attach: field: string * file: MultipartValueSingle * ?options: U2<string, RequestAttach> -> Request
        abstract auth: user: string * pass: string * ?options: RequestAuthOptions -> Request
        abstract auth: token: string * options: RequestAuthOptions_ -> Request
        abstract buffer: ?``val``: bool -> Request
        abstract ca: cert: U4<string, ResizeArray<string>, Buffer, ResizeArray<Buffer>> -> Request
        abstract cert: cert: U4<string, ResizeArray<string>, Buffer, ResizeArray<Buffer>> -> Request
        abstract clearTimeout: unit -> Request
        abstract disableTLSCerts: unit -> Request
        abstract ``end``: ?callback: CallbackHandler -> unit
        abstract field: name: string * ``val``: MultipartValue -> Request
        abstract field: fields: RequestFieldFields -> Request
        abstract get: field: string -> string
        abstract http2: ?enable: bool -> Request
        abstract key: cert: U4<string, ResizeArray<string>, Buffer, ResizeArray<Buffer>> -> Request
        abstract ok: callback: (Response -> bool) -> Request
        [<Emit "$0.on('error',$1)">] abstract on_error: handler: (obj option -> unit) -> Request
        [<Emit "$0.on('progress',$1)">] abstract on_progress: handler: (ProgressEvent -> unit) -> Request
        [<Emit "$0.on('response',$1)">] abstract on_response: handler: (Response -> unit) -> Request
        abstract on: name: string * handler: (obj option -> unit) -> Request
        abstract parse: parser: Parser -> Request
        abstract part: unit -> Request
        abstract pfx: cert: U5<string, ResizeArray<string>, Buffer, ResizeArray<Buffer>, RequestPfx> -> Request
        abstract pipe: stream: Stream.Stream * ?options: obj -> Stream.Stream
        abstract query: ``val``: U2<obj, string> -> Request
        abstract redirects: n: float -> Request
        abstract responseType: ``type``: string -> Request
        abstract retry: ?count: float * ?callback: CallbackHandler -> Request
        abstract send: ?data: U2<string, obj> -> Request
        abstract send: ?data: string -> Request
        abstract send: ?data: obj -> Request
        abstract serialize: serializer: Serializer -> Request
        abstract set: field: obj -> Request
        abstract set: field: string * ``val``: string -> Request
        [<Emit "$0.set('Cookie',$1)">] abstract set_Cookie: ``val``: ResizeArray<string> -> Request
        abstract timeout: ms: U2<float, RequestTimeout> -> Request
        abstract trustLocalhost: ?enabled: bool -> Request
        abstract ``type``: ``val``: string -> Request
        abstract unset: field: string -> Request
        abstract ``use``: fn: Plugin -> Request
        abstract withCredentials: unit -> Request
        abstract write: data: U2<string, Buffer> * ?encoding: string -> bool
        abstract maxResponseSize: size: float -> Request

    type [<AllowNullLiteral>] RequestAuthOptions =
        abstract ``type``: RequestAuthOptionsType with get, set

    type [<AllowNullLiteral>] RequestAuthOptions_ =
        abstract ``type``: string with get, set

    type [<AllowNullLiteral>] RequestFieldFields =
        [<EmitIndexer>] abstract Item: fieldName: string -> MultipartValue with get, set

    type [<AllowNullLiteral>] Plugin =
        [<Emit "$0($1...)">] abstract Invoke: req: SuperAgentRequest -> unit

    type [<AllowNullLiteral>] ProgressEvent =
        abstract direction: ProgressEventDirection with get, set
        abstract loaded: float with get, set
        abstract percent: float option with get, set
        abstract total: float option with get, set

    type [<AllowNullLiteral>] SuperAgentStaticSerialize =
        [<EmitIndexer>] abstract Item: ``type``: string -> Serializer with get, set

    type [<AllowNullLiteral>] SuperAgentStaticParse =
        [<EmitIndexer>] abstract Item: ``type``: string -> Parser with get, set

    type [<AllowNullLiteral>] RequestAttach =
        abstract filename: string option with get, set
        abstract contentType: string option with get, set

    type [<AllowNullLiteral>] RequestPfx =
        abstract pfx: U2<string, Buffer> with get, set
        abstract passphrase: string with get, set

    type [<AllowNullLiteral>] RequestTimeout =
        abstract deadline: float option with get, set
        abstract response: float option with get, set

    type [<StringEnum>] [<RequireQualifiedAccess>] RequestAuthOptionsType =
        | Basic
        | Auto

    type [<StringEnum>] [<RequireQualifiedAccess>] ProgressEventDirection =
        | Download
        | Upload
