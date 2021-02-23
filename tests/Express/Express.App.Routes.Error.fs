module Tests.Express.App.Routes.Error

open System.Text.RegularExpressions
open ExpressServeStaticCore
open Fable.Core.JsInterop
open Mocha
open Node
open SuperTest
open Fable.Core

describe "app" (fun () ->

    describe ".VERB()" (fun () ->

        itAsync "should not get invoked without error handler on error" (fun d ->
            let app = Express.e.express()

            app.``use``(fun (req : Request) (res : Response) (next : NextFunction) ->
                next.Invoke(System.Exception("boom!"))
            )

            app.get("/bar", fun (req : Request) (res : Response) ->
                res.send("hello, world!")
            )

            request(app)
                .post("/bar")
                .expect(500, JS.Constructors.RegExp.Create("Error: boom!"), d)
                |> ignore
        )

        itAsync "should only call an error handling routing callback when an error is propagated" (fun ``done`` ->
            let app = Express.e.express()

            let mutable a = false
            let mutable b = false
            let mutable c = false
            let mutable d = false

            app.get("/",
                fun (req : Request) (res : Response) (next : NextFunction) ->
                    next.Invoke(System.Exception("fabricated error"))
                |> Adapter.RequestHandler,
                fun (req : Request) (res : Response) (next : NextFunction) ->
                    a <- true
                    next.Invoke()
                |> Adapter.RequestHandler,
                fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                    b <- true
                    Assert.strictEqual(err.Value.Message, "fabricated error")
                    next.Invoke(err)
                |> Adapter.RequestHandler,
                fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                    c <- true
                    Assert.strictEqual(err.Value.Message, "fabricated error")
                    next.Invoke()
                |> Adapter.RequestHandler,
                fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                    d <- true
                    next.Invoke()
                |> Adapter.RequestHandler,
                fun (req : Request) (res : Response) ->
                    Assert.strictEqual(a, false)
                    Assert.strictEqual(b, true)
                    Assert.strictEqual(c, true)
                    Assert.strictEqual(d, false)
                    res.send(204)
                |> Adapter.RequestHandler
            )

            request(app)
                .get("/")
                .expect(204, ``done``)
                |> ignore
        )
    )
)
