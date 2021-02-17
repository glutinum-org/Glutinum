module Tests.Express.App.Router

open System.Text.RegularExpressions
open ExpressServeStaticCore
open Fable.Core.JsInterop
open Mocha
open SuperTest
open Fable.Core


describe "app.router" (fun _ ->
    itAsync "should restore req.params after leaving router" (fun d ->
        let app = Express.e.express()
        let router = Express.e.Router()

        let handler1 =
            fun (req : Request) (res : Response) (next : NextFunction)  ->
                res.setHeader("x-user-id", req.``params``.["id"])
                next.Invoke()
            |> Adapter.RequestHandler

        let handler2 =
            fun (req : Request) (res : Response) ->
                res.send(req.``params``.["id"])
            |> Adapter.RequestHandler

        router.``use``(fun (req : Request) (res : Response) (next : NextFunction) ->
            res.setHeader("x-router", emitJsExpr (req.``params``.["id"]) "String($0)")
            next.Invoke()
        )

        app.get(
            "/user/:id",
            handler1,
            router :> RequestHandler,
            handler2
        )

        request(app)
            .get("/user/1")
            .expect("x-router", "undefined")
            .expect("x-user-id", "1")
            .expect(
                200,
                "1",
                d
            )
            |> ignore
    )

    describe "methods" (fun _ ->

        Methods.methods.ToArray()
        |> Array.append [| unbox "del" |]
        |> Array.iter (fun method ->
            if method = Methods.Method.Connect then
                () // Do nothing
            else
                itAsync ("should include " + (string method).ToUpper()) (fun d ->
                    let app = Express.e.express()

                    app?(method)$("/foo", fun (_ : Request) (res : Response) ->
                        res.send(method)
                    )

                    emitJsStatement (app, request, method, d) """
$1($0)
[$2]('/foo')
.expect(200, $3)
"""
                )

                it ("should reject numbers for app." + (string method)) (fun _ ->
                    let app = Express.e.express()

                    Assert.throws(app?(method)?bind$(app, "/", 3))
                )
        )


        itAsync "should re-route when method is altered" (fun d ->
            let app = Express.e.express()
            let cb : System.Func<obj option, SuperTest.Response,unit> = After.e.after(3, d)

            app.``use``(fun (req : Request) (res : Response) (next : NextFunction) ->
                if req.method <> "POST" then
                    next.Invoke()
                else
                    req.method <- "DELETE"
                    res.setHeader("X-Method-Altered", "1")
                    next.Invoke()
            )

            app.del("/", fun (req : Request) (res : Response) ->
                res.``end``("deleted everything")
            )

            request(app)
                .get("/")
                .expect(
                    404,
                    cb
                )
                |> ignore

            request(app)
                .del("/")
                .expect(
                    200,
                    "deleted everything",
                    cb
                )
                |> ignore

            request(app)
                .post("/")
                .expect("X-Method-Altered", "1")
                .expect(
                    200,
                    "deleted everything",
                    cb
                )
                |> ignore
        )
    )

    describe "decode params" (fun _ ->
        itAsync "should decode correct params" (fun d ->
            let app = Express.e.express()

            app.get("/:name", fun (req : Request) (res : Response) (next : NextFunction) ->
                res.send(req.``params``.["name"])
            )

            request(app)
                .get("/foo%2Fbar")
                .expect(
                    "foo/bar",
                    d
                )
                |> ignore
        )

        itAsync "should not accept params in malformed paths" (fun d ->
            let app = Express.e.express()

            app.get("/:name", fun (req : Request) (res : Response) (next : NextFunction) ->
                res.send(req.``params``.["name"])
            )

            request(app)
                .get("/%foobar")
                .expect(400, d)
                |> ignore
        )

        itAsync "should not decode spaces" (fun d ->
            let app = Express.e.express()

            app.get("/:name", fun (req : Request) (res : Response) (next : NextFunction) ->
                res.send(req.``params``.["name"])
            )

            request(app)
                .get("/foo+bar")
                .expect("foo+bar", d)
                |> ignore
        )

        itAsync "should work with unicode" (fun d ->
            let app = Express.e.express()

            app.get("/:name", fun (req : Request) (res : Response) (next : NextFunction) ->
                res.send(req.``params``.["name"])
            )

            request(app)
                .get("/%ce%b1")
                .expect("\u03b1", d)
                |> ignore
        )
    )

    itAsync "should be .use()able" (fun d ->
        let app = Express.e.express()
        let calls = ResizeArray()

        app.``use``(fun (req : Request) (res : Response) (next : NextFunction) ->
            calls.Add("before")
            next.Invoke()
        )

        app.get("/", fun (req : Request) (res : Response) (next : NextFunction) ->
            calls.Add("GET /")
            next.Invoke()
        )

        app.``use``(fun (req : Request) (res : Response) (next : NextFunction) ->
            calls.Add("after")
            res.json(calls)
        )

        request(app)
            .get("/")
            .expect(200, ResizeArray(["before"; "GET /"; "after"]), d)
            |> ignore
    )

    describe "when given a regexp" (fun _ ->
        itAsync "should match the pathname" (fun d ->
            let app = Express.e.express()

            app.get(Regex("^\/user\/[0-9]+$"), fun (req : Request) (res : Response) ->
                res.``end``("user")
            )

            request(app)
                .get("/user/12?foo=bar")
                .expect("user", d)
                |> ignore
        )

        itAsync "should populate req.params with the captures" (fun d ->
            let app = Express.e.express()

            app.get(Regex("^\/user\/([0-9]+)\/(view|edit)?$"), fun (req : Request) (res : Response) ->
                let id = req.``params``.[0]
                let op = req.``params``.[1]

                res.``end``(op + "ing user " + id)
            )

            request(app)
                .get("/user/10/edit")
                .expect("editing user 10", d)
                |> ignore
        )
    )

    describe "case sensitivity" (fun _ ->
        itAsync "should be disabled by default" (fun d ->
            let app = Express.e.express()

            app.get("/user", fun (req : Request) (res : Response) ->
                res.``end``("tj")
            )

            request(app)
                .get("/USER")
                .expect("tj", d)
                |> ignore
        )

        describe "when \"case sensitive routing\" is enabled" (fun _ ->
            itAsync "should match identical casing" (fun d ->
                let app = Express.e.express()

                app.enable("case sensitive routing") |> ignore

                app.get("/uSer", fun (req : Request) (res : Response) ->
                    res.``end``("tj")
                )

                request(app)
                    .get("/uSer")
                    .expect("tj", d)
                    |> ignore
            )

            itAsync "should not match otherwise" (fun d ->
                let app = Express.e.express()

                app.enable("case sensitive routing") |> ignore

                app.get("/uSer", fun (req : Request) (res : Response) ->
                    res.``end``("tj")
                )

                request(app)
                    .get("/user")
                    .expect(404, d)
                    |> ignore
            )
        )
    )

    describe "params" (fun _ ->
        itAsync "should overwrite existing req.params by default" (fun d ->
            let app = Express.e.express()
            let router = Express.e.Router()

            router.get("/:action", fun (req : Request) (res : Response) ->
                res.send(req.``params``)
            )

            app.``use``("/user/:user", router)

            request(app)
                .get("/user/1/get")
                .expect(200, """{"action":"get"}""", d)
                |> ignore
        )

        itAsync "should allow merging existing req.params" (fun d ->
            let app = Express.e.express()
            let router =
                Express.e.Router(jsOptions<Express.E.RouterOptions> (fun o ->
                    o.mergeParams <- true
                ))

            router.get("/:action", fun (req : Request) (res : Response) ->
                let keys = JS.Constructors.Object.keys(req.``params``)

                let result =
                    keys.ToArray()
                    |> Array.sort
                    |> Array.map(fun key ->
                        box key, box req.``params``.[key]
                    )

                res.send(result)
            )

            app.``use``("/user/:user", router)

            request(app)
                .get("/user/tj/get")
                .expect(
                    200,
                    """[["action","get"],["user","tj"]]""",
                    d
                )
                |> ignore
        )

        itAsync "should use params from router" (fun d ->
            let app = Express.e.express()
            let router = Express.e.Router()

            router.get("/:thing", fun (req : Request) (res : Response) ->
                let keys = JS.Constructors.Object.keys(req.``params``)

                let result =
                    keys.ToArray()
                    |> Array.sort
                    |> Array.map(fun key ->
                        box key, box req.``params``.[key]
                    )

                res.send(result)
            )

            app.``use``("/user/:thing", router)

            request(app)
                .get("/user/tj/get")
                .expect(
                    200,
                    """[["thing","get"]]""",
                    d
                )
                |> ignore
        )

        itAsync "should merge numeric indices req.params" (fun d ->
            let app = Express.e.express()
            let router =
                Express.e.Router(jsOptions<Express.E.RouterOptions> (fun o ->
                    o.mergeParams <- true
                ))

            router.get("/*.*", fun (req : Request) (res : Response) ->
                let keys = JS.Constructors.Object.keys(req.``params``)

                let result =
                    keys.ToArray()
                    |> Array.sort
                    |> Array.map(fun key ->
                        box key, box req.``params``.[key]
                    )

                res.send(result)
            )

            app.``use``("/user/id:(\\d+)", router)

            request(app)
                .get("/user/id:10/profile.json")
                .expect(
                    200,
                    """[["0","10"],["1","profile"],["2","json"]]""",
                    d
                )
                |> ignore
        )

        itAsync "should merge numeric indices req.params when more in parent" (fun d ->
            let app = Express.e.express()
            let router =
                Express.e.Router(jsOptions<Express.E.RouterOptions> (fun o ->
                    o.mergeParams <- true
                ))

            router.get("/*", fun (req : Request) (res : Response) ->
                let keys = JS.Constructors.Object.keys(req.``params``)

                let result =
                    keys.ToArray()
                    |> Array.sort
                    |> Array.map(fun key ->
                        box key, box req.``params``.[key]
                    )

                res.send(result)
            )

            app.``use``("/user/id:(\\d+)/name:(\\w+)", router)

            request(app)
                .get("/user/id:10/name:tj/profile")
                .expect(
                    200,
                    """[["0","10"],["1","tj"],["2","profile"]]""",
                    d
                )
                |> ignore
        )

        itAsync "should merge numeric indices req.params when parent has same number" (fun d ->
            let app = Express.e.express()
            let router =
                Express.e.Router(jsOptions<Express.E.RouterOptions> (fun o ->
                    o.mergeParams <- true
                ))

            router.get("/name:(\\w+)", fun (req : Request) (res : Response) ->
                let keys = JS.Constructors.Object.keys(req.``params``)

                let result =
                    keys.ToArray()
                    |> Array.sort
                    |> Array.map(fun key ->
                        box key, box req.``params``.[key]
                    )

                res.send(result)
            )

            app.``use``("/user/id:(\\d+)", router)

            request(app)
                .get("/user/id:10/name:tj")
                .expect(
                    200,
                    """[["0","10"],["1","tj"]]""",
                    d
                )
                |> ignore
        )

        itAsync "should ignore invalid incoming req.params" (fun d ->
            let app = Express.e.express()
            let router =
                Express.e.Router(jsOptions<Express.E.RouterOptions> (fun o ->
                    o.mergeParams <- true
                ))

            router.get("/:name", fun (req : Request) (res : Response) ->
                let keys = JS.Constructors.Object.keys(req.``params``)

                let result =
                    keys.ToArray()
                    |> Array.sort
                    |> Array.map(fun key ->
                        box key, box req.``params``.[key]
                    )

                res.send(result)
            )

            app.``use``("/user/", fun (req : Request) (res : Response) (next : NextFunction) ->
                req.``params`` <- unbox 3 // wat?
                router.Invoke(req, res, next)
            )

            request(app)
                .get("/user/tj")
                .expect(
                    200,
                    """[["name","tj"]]""",
                    d
                )
                |> ignore
        )

        itAsync "should restore req.params" (fun d ->
            let app = Express.e.express()
            let router =
                Express.e.Router(jsOptions<Express.E.RouterOptions> (fun o ->
                    o.mergeParams <- true
                ))

            router.get("/user:(\\w+)/*", fun (req : Request) (res : Response) (next : NextFunction) ->
                next.Invoke()
            )

            app.``use``("/user/id:(\\d+)", fun (req : Request) (res : Response) (next : NextFunction) ->
                router.Invoke(req, res, Adapter.NextFunction(fun err ->
                        let keys = JS.Constructors.Object.keys(req.``params``)

                        let result =
                            keys.ToArray()
                            |> Array.sort
                            |> Array.map(fun key ->
                                box key, box req.``params``.[key]
                            )

                        res.send(result)
                    )
                )
            )

            request(app)
                .get("/user/id:42/user:tj/profile")
                .expect(
                    200,
                    """[["0","42"]]""",
                    d
                )
                |> ignore
        )
    )

    describe "trailing slashes" (fun _ ->

        itAsync "should be optional by default" (fun d ->
            let app = Express.e.express()

            app.get("/user", fun (req : Request) (res : Response) ->
                res.``end``("tj")
            )

            request(app)
                .get("/user/")
                .expect("tj", d)
                |> ignore
        )

        describe "when \"strict routing\" is enabled" (fun _ ->

            itAsync "should match trailing slashes" (fun d ->
                let app = Express.e.express()

                app.enable("strict routing") |> ignore

                app.get("/user/", fun (req : Request) (res : Response) ->
                    res.``end``("tj")
                )

                request(app)
                    .get("/user/")
                    .expect("tj", d)
                    |> ignore
            )


            itAsync "should pass-though middleware" (fun d ->
                let app = Express.e.express()

                app.enable("strict routing") |> ignore

                app.``use``(fun (req : Request) (res : Response) (next : NextFunction) ->
                    res.setHeader("x-middleware", !^ "true")
                    next.Invoke()
                )

                app.get("/user/", fun (req : Request) (res : Response) ->
                    res.``end``("tj")
                )

                request(app)
                    .get("/user/")
                    .expect("x-middleware", "true")
                    .expect(200, "tj", d)
                    |> ignore
            )

            itAsync "should pass-though mounted middleware" (fun d ->
                let app = Express.e.express()

                app.enable("strict routing") |> ignore

                app.``use``("/user", fun (req : Request) (res : Response) (next : NextFunction) ->
                    res.setHeader("x-middleware", !^ "true")
                    next.Invoke()
                )

                app.get("/user/", fun (req : Request) (res : Response) ->
                    res.``end``("tj")
                )

                request(app)
                    .get("/user/")
                    .expect("x-middleware", "true")
                    .expect(200, "tj", d)
                    |> ignore
            )

            itAsync "should match no slashes" (fun d ->
                let app = Express.e.express()

                app.enable("strict routing") |> ignore

                app.get("/user", fun (req : Request) (res : Response) ->
                    res.``end``("tj")
                )

                request(app)
                    .get("/user")
                    .expect("tj", d)
                    |> ignore
            )

            itAsync "should match middleware when omitting the trailing slash" (fun d ->
                let app = Express.e.express()

                app.enable("strict routing") |> ignore

                app.``use``("/user/", fun (req : Request) (res : Response) ->
                    res.``end``("tj")
                )

                request(app)
                    .get("/user")
                    .expect(200, "tj", d)
                    |> ignore
            )


            itAsync "should match middleware" (fun d ->
                let app = Express.e.express()

                app.enable("strict routing") |> ignore

                app.``use``("/user", fun (req : Request) (res : Response) ->
                    res.``end``("tj")
                )

                request(app)
                    .get("/user")
                    .expect(200, "tj", d)
                    |> ignore
            )

            itAsync "should match middleware when adding the trailing slash" (fun d ->
                let app = Express.e.express()

                app.enable("strict routing") |> ignore

                app.``use``("/user", fun (req : Request) (res : Response) ->
                    res.``end``("tj")
                )

                request(app)
                    .get("/user/")
                    .expect(200, "tj", d)
                    |> ignore
            )

            itAsync "should fail when omitting the trailing slash" (fun d ->
                let app = Express.e.express()

                app.enable("strict routing") |> ignore

                app.get("/user/", fun (req : Request) (res : Response) ->
                    res.``end``("tj")
                )

                request(app)
                    .get("/user")
                    .expect(404, d)
                    |> ignore
            )

            itAsync "should fail when adding the trailing slash" (fun d ->
                let app = Express.e.express()

                app.enable("strict routing") |> ignore

                app.get("/user", fun (req : Request) (res : Response) ->
                    res.``end``("tj")
                )

                request(app)
                    .get("/user/")
                    .expect(404, d)
                    |> ignore
            )
        )
    )






)
