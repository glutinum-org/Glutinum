namespace rec Npm.Types

open System
open Fable.Core
open Fable.Core.JS
open Node

module SuperTest =

    type RegExp = System.Text.RegularExpressions.Regex

    // // Temporary hack to avoid creating Superagent and all it's dependencies bindings
    // module Superagent =

    //     type [<AllowNullLiteral>] Response =
    //         interface end

    //     type [<AllowNullLiteral>] SuperAgentRequest =
    //         interface end

    //     type [<AllowNullLiteral>] SuperAgent<'T> =
    //         interface end

    type [<AllowNullLiteral>] IExports =
        [<Emit("$0($1)")>]
        abstract supertest: app: obj -> Supertest.SuperTest<Supertest.Test>

    module Supertest =

        type [<AllowNullLiteral>] IExports =
            abstract agent: ?app: obj * ?options: AgentOptions -> SuperAgentTest

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
            abstract expect: status: int * ?callback: CallbackHandler -> Test
            abstract expect: status: int * body: obj * ?callback: CallbackHandler -> Test
            abstract expect: checker: (Response -> obj option) * ?callback: CallbackHandler -> Test
            abstract expect: body: string * ?callback: CallbackHandler -> Test
            abstract expect: body: RegExp * ?callback: CallbackHandler -> Test
            abstract expect: body: Object * ?callback: CallbackHandler -> Test
            abstract expect: field: string * ``val``: string * ?callback: CallbackHandler -> Test
            abstract expect: field: string * ``val``: RegExp * ?callback: CallbackHandler -> Test
            abstract ``end``: ?callback: CallbackHandler -> Test

        type [<AllowNullLiteral>] AgentOptions =
            abstract ca: obj option with get, set

        type [<AllowNullLiteral>] SuperAgentTest =
            interface end

        type [<AllowNullLiteral>] SuperTest<'T when 'T :> SuperAgent.Request.SuperAgentRequest> =
            inherit SuperAgent.Request.SuperAgent<'T>
