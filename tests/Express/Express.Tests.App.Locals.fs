module Tests.Express.App.Locals

open Fable.Core.JS
open Fable.Core.JsInterop
open Npm
open Mocha
open ExpressServeStaticCore
// open Fable.Core.Testing

type CallbackHandler = Types.SuperTest.Supertest.CallbackHandler

#nowarn "40"

let tests () =
    describe "app" (fun _ ->

        describe ".locals(obj" (fun _ ->

            it "should merge locals" (fun _ ->
                let app = Express.e.express()

                Assert.deepStrictEqual(Object.keys(app.locals), ResizeArray ["settings"])
                app.locals.["user"] <- "tobi"
                app.locals.["age"] <- 2

                Assert.deepStrictEqual(Object.keys(app.locals), ResizeArray ["settings"; "user"; "age"])
                Assert.strictEqual(app.locals.["user"], box "tobi")
                Assert.strictEqual(app.locals.["age"], box 2)
            )

        )

        describe ".locals.settings" (fun _ ->
            it "should expose app settings" (fun _ ->
                let app = Express.e.express()
                app.set("title", "House of Manny") |> ignore

                let o = app.locals.["settings"]
                Assert.strictEqual(o?env, "development")
                Assert.strictEqual(o?title, "House of Manny")
            )
        )

    )
