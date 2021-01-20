namespace Connect

open Mocha
open Fable.Core
open Fable.Core.Testing
open Fable.Core.JsInterop
open Node
open Connect
open SuperTest
open Npm

module Tests =

    let all () =
        describe "Connect" (fun _ ->

            let mutable app : Types.CreateServer.Server = Unchecked.defaultof<_>

            beforeEach (fun _ ->
                app <- npm.connect()
            )

            itAsync "should inherit from event emitter" (fun ok ->
                app.on("foo", ok) |> ignore
                app.emit("foo") |> ignore
            )

            itAsync "should work in http.createServer" (fun ok ->
                let app = npm.connect()

                // We need to help the compiler with a type hint
                app.``use``(fun req (res : Http.ServerResponse) ->
                    res.``end``("Hello, world!")
                ) |> ignore

                let server = http.createServer(app)

                npm.supertest.supertest(box server)
                    .get("/")
                    .expect(200, box "Hello, world!", unbox<Types.Supertest.CallbackHandler> ok)
                    |> ignore
            )

            describe "error handler" (fun _ ->

                itAsync "should use custom error code" (fun ok ->
                    let app = npm.connect()

                    app.``use``(fun req res next ->
                        let err = new System.Exception("boom!")
                        err?status <- 503
                        raise err |> ignore
                    ) |> ignore

                    npm.supertest.supertest(box app)
                        .get("/")
                        .expect(503, unbox<Types.Supertest.CallbackHandler> ok)
                        |> ignore
                )

            )

        )
