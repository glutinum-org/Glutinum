module Tests.Express.App.Locals

open Fable.Core.JS
open Fable.Core.JsInterop
open Mocha
open ExpressServeStaticCore
// open Fable.Core.Testing
open Fable.Core.JS


describe "app" (fun _ ->

    describe ".locals(obj" (fun _ ->

        it "should merge locals" (fun _ ->
            let app = Express.e.express()

            Assert.deepStrictEqual(Constructors.Object.keys(app.locals), ResizeArray ["settings"])
            app.locals.["user"] <- "tobi"
            app.locals.["age"] <- 2

            Assert.deepStrictEqual(Constructors.Object.keys(app.locals), ResizeArray ["settings"; "user"; "age"])
            Assert.strictEqual(app.locals.["user"], box "tobi")
            Assert.strictEqual(app.locals.["age"], box 2)
        )

    )

    describe ".locals.settings" (fun _ ->
        it "should expose app settings" (fun _ ->
            let app = Express.e.express()
            app.set("title", "House of Manny") |> ignore

            let o = app.locals.["settings"]
            Assert.strictEqual(o?env, "test")
            Assert.strictEqual(o?title, "House of Manny")
        )
    )

)