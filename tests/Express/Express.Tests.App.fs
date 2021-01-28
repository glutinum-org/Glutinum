module Tests.Express.App.Core

open Npm
open Mocha

let tests () =
    describe "app" (fun _ ->

        itAsync "should inherit from event emitter" (fun d ->
            let app = Express.e.express()

            app.on("foo", fun _ -> d()) |> ignore
            app.emit("foo") |> ignore
        )

        itAsync "should be callable" (fun d ->
            npm.supertest.supertest(Express.e.express())
                .get("/")
                .expect(
                    404,
                    Types.SuperTest.Supertest.CallbackHandler (fun err _ -> d err)
                )
                |> ignore
        )

    )
