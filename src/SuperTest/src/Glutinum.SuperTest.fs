module rec Glutinum.SuperTest

open System
open Fable.Core
open Fable.Core.JS

[<Import("default", "supertest")>]
let supertest : IExports = jsNative

type IExports =
    [<Emit("$0($1)")>]
    abstract supertest : app : obj -> SuperTest.SuperTest<SuperTest.Test>
    abstract agent: ?app: obj * ?options: SuperTest.AgentOptions -> SuperTest.SuperAgentTest

//let supertest (_app : obj) : Supertest.Test = jsNative
//
//type Supertest() =
//    [<Import("default", "supertest")>]
//    abstract agent: ?app: obj * ?options: Supertest.AgentOptions -> Supertest.SuperAgentTest


type RegExp = System.Text.RegularExpressions.Regex

module SuperTest =

//    type [<AllowNullLiteral>] IExports =
//        abstract agent: ?app: obj * ?options: AgentOptions -> SuperAgentTest

    type [<AllowNullLiteral>] Response =
        inherit SuperAgent.Request.Response

    type [<AllowNullLiteral>] Request =
        inherit SuperAgent.Request.SuperAgentRequest

    type CallbackHandler =
        Action<obj option, Response>
        // [<Emit "$0($1...)">] abstract Invoke: err: obj option * res: Response -> unit

    type [<AllowNullLiteral>] Test =
        inherit SuperAgent.Request.SuperAgentRequest
        abstract app: obj option with get, set
        abstract url: string with get, set
        abstract serverAddress: app: obj option * path: string -> string
        abstract expect: status: int -> Test
        abstract expect: body : string -> Test
        abstract expect: status: int * callback: Func<obj option, Response, unit> -> Test
//            abstract expect: status: int * body: obj * callback: CallbackHandler -> Test
        abstract expect: status: int * body: obj -> Test
        abstract expect: status : int * callback: Func<obj option, unit> -> Test
        abstract expect: status: int * body: obj * callback: Func<obj option, Response, unit> -> Test
        abstract expect: status: int * body: obj * callback: Func<obj option, unit> -> Test
        abstract expect: status: string * body: obj * callback: Func<obj option, unit> -> Test
        abstract expect: body: obj * callback: Func<obj option, Response, unit> -> Test
        abstract expect: body: obj * callback: Func<obj option, unit> -> Test

        abstract expect: checker: (Response -> obj option) * ?callback: CallbackHandler -> Test
        abstract expect: body: string * ?callback: CallbackHandler -> Test
        abstract expect: body: RegExp * ?callback: CallbackHandler -> Test
        abstract expect: body: Object * ?callback: CallbackHandler -> Test
        abstract expect: field: string * ``val``: string -> Test
        abstract expect: field: string * ``val``: string * callback: Func<obj option, Response, unit> -> Test
        abstract expect: field: string * ``val``: RegExp * ?callback: Func<obj option, Response, unit> -> Test
        abstract ``end``: ?callback: CallbackHandler -> Test

    type [<AllowNullLiteral>] AgentOptions =
        abstract ca: obj option with get, set

    type [<AllowNullLiteral>] SuperAgentTest =
        interface end

    type [<AllowNullLiteral>] SuperTest<'T when 'T :> SuperAgent.Request.SuperAgentRequest> =
        inherit SuperAgent.Request.SuperAgent<'T>
