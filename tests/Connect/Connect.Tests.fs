namespace Connect

open Mocha
open Fable.Core
open Fable.Core.Testing
open Fable.Core.JsInterop
open Node
open Connect
open SuperTest
module Tests =

    let all () =
        describe "Connect" (fun _ ->

            let mutable app : Types.CreateServer.Server = Unchecked.defaultof<_>

            beforeEach (fun _ ->
                app <- connect()
            )

            itAsync "should inherit from event emitter" (fun ok ->
                app.on("foo", ok) |> ignore
                app.emit("foo") |> ignore
            )

            itAsync "should work in http.createServer" (fun ok ->
                let app = connect()

                // We need to help the compiler with a type hint
                app.``use``(fun req (res : Http.ServerResponse) ->
                    res.``end``("Hello, world!")
                ) |> ignore

                let server = http.createServer(app)

                supertest.supertest(box server)
                    .get("/")
                    ?expect(200, "Hello, world!", ok)
            )

            describe "error handler" (fun _ ->

                itAsync "should use custom error code" (fun ok ->
                    let app = connect()

                    app.``use``(fun req res next ->
                        let err = new System.Exception("boom!")
                        err?status <- 503
                        raise err |> ignore
                    ) |> ignore

                    supertest.supertest(box app)
                        .get("/")
                        ?expect(503, ok)
                )

            )

        )
