module Tests.Express.App.Router

open System.Text.RegularExpressions
open ExpressServeStaticCore
open Fable.Core.JsInterop
open Mocha
open Node
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

    itAsync "should allow escaped regexp" (fun d ->
        let app = Express.e.express()

        app.get("/user/\\d+", fun (req : Request) (res : Response) ->
            res.``end``("woot")
        )

        request(app)
            .get("/user/10")
            .expect(200, fun err ->
                if err.IsSome then
                    d err
                else
                    request(app)
                        .get("/user/tj")
                        .expect(404, d)
                        |> ignore
            )
            |> ignore
    )

    itAsync "should allow literal \".\"" (fun d ->
        let app = Express.e.express()

        app.get("/api/users/:from..:to", fun (req : Request) (res : Response) ->
            let from = req.``params``.["from"]
            let ``to`` = req.``params``.["to"]

            res.``end``("users from " + from + " to " + ``to``);
        )

        request(app)
            .get("/api/users/1..50")
            .expect("users from 1 to 50", d)
            |> ignore
    )

    describe "*" (fun d ->
        itAsync "should capture everything" (fun d ->
            let app = Express.e.express()

            app.get("*", fun (req : Request) (res : Response) ->
                res.``end``(req.``params``.[0])
            )

            request(app)
                .get("/user/tobi.json")
                .expect("/user/tobi.json", d)
                |> ignore
        )

        itAsync "should decode the capture" (fun d ->
            let app = Express.e.express()

            app.get("*", fun (req : Request) (res : Response) ->
                res.``end``(req.``params``.[0])
            )

            request(app)
                .get("/user/tobi%20and%20loki.json")
                .expect("/user/tobi and loki.json", d)
                |> ignore
        )


        itAsync "should denote a greedy capture group" (fun d ->
            let app = Express.e.express()

            app.get("/user/*.json", fun (req : Request) (res : Response) ->
                res.``end``(req.``params``.[0])
            )

            request(app)
                .get("/user/tj.json")
                .expect("tj", d)
                |> ignore
        )


        itAsync "should work with several" (fun d ->
            let app = Express.e.express()

            app.get("/api/*.*", fun (req : Request) (res : Response) ->
                let resource = req.``params``.[0]
                let format = req.``params``.[1]

                res.``end``(resource + " as " + format)
            )

            request(app)
                .get("/api/users/foo.bar.json")
                .expect("users/foo.bar as json", d)
                |> ignore
        )

        itAsync "should work cross-segment" (fun d ->
            let app = Express.e.express()

            app.get("/api*", fun (req : Request) (res : Response) ->
                res.send(req.``params``.[0]);
            )

            request(app)
                .get("/api")
                .expect("", fun err ->
                    request(app)
                        .get("/api/hey")
                        .expect("/hey", d)
                        |> ignore
                )
                |> ignore
        )

        itAsync "should allow naming" (fun d ->
            let app = Express.e.express()

            app.get("/api/:resource(*)", fun (req : Request) (res : Response) ->
                res.``end``(req.``params``.["resource"])
            )

            request(app)
                .get("/api/users/0.json")
                .expect("users/0.json", d)
                |> ignore
        )


        itAsync "should not be greedy immediately after param" (fun d ->
            let app = Express.e.express()

            app.get("/user/:user*", fun (req : Request) (res : Response) ->
                res.``end``(req.``params``.["user"])
            )

            request(app)
                .get("/user/122")
                .expect("122", d)
                |> ignore
        )

        itAsync "should eat everything after /" (fun d ->
            let app = Express.e.express()

            app.get("/user/:user*", fun (req : Request) (res : Response) ->
                res.``end``(req.``params``.["user"])
            )

            request(app)
                .get("/user/122/aaa")
                .expect("122", d)
                |> ignore
        )

        itAsync "should span multiple segments" (fun d ->
            let app = Express.e.express()

            app.get("/file/*", fun (req : Request) (res : Response) ->
                res.``end``(req.``params``.[0])
            )

            request(app)
                .get("/file/javascripts/jquery.js")
                .expect("javascripts/jquery.js", d)
                |> ignore
        )

        itAsync "should be optional" (fun d ->
            let app = Express.e.express()

            app.get("/file/*", fun (req : Request) (res : Response) ->
                res.``end``(req.``params``.[0])
            )

            request(app)
                .get("/file/")
                .expect("", d)
                |> ignore
        )

        itAsync "should require a preceding /" (fun d  ->
            let app = Express.e.express()

            app.get("/file/*", fun (req : Request) (res : Response) ->
                res.``end``(req.``params``.[0])
            )

            request(app)
                .get("/file")
                .expect(404, d)
                |> ignore
        )

        itAsync "should keep correct parameter indexes" (fun d ->
            let app = Express.e.express()

            app.get("/*/user/:id", fun (req : Request) (res : Response) ->
                res.send(req.``params``)
            )

            request(app)
                .get("/1/user/2")
                .expect(200, """{"0":"1","id":"2"}""", d)
                |> ignore
        )

        itAsync "should work within arrays" (fun d ->
            let app = Express.e.express()

            app.get(ResizeArray [|"/user/:id"; "/foo/*"; "/:bar"|], fun (req : Request) (res : Response) ->
                res.send(req.``params``.["bar"])
            )

            request(app)
                .get("/test")
                .expect(200, "test", d)
                |> ignore
        )
    )

    describe ":name" (fun _ ->

        itAsync "should denote a capture group" (fun d ->
            let app = Express.e.express()

            app.get("/user/:user", fun (req : Request) (res : Response) ->
                res.send(req.``params``.["user"])
            )

            request(app)
                .get("/user/tj")
                .expect("tj", d)
                |> ignore
        )

        itAsync "should match a single segment only" (fun d ->
            let app = Express.e.express()

            app.get("/user/:user", fun (req : Request) (res : Response) ->
                res.send(req.``params``.["user"])
            )

            request(app)
                .get("/user/tj/edit")
                .expect(404, d)
                |> ignore
        )

        itAsync "should allow several capture groups" (fun d ->
            let app = Express.e.express()

            app.get("/user/:user/:op", fun (req : Request) (res : Response) ->
                res.send(req.``params``.["op"] + "ing " + req.``params``.["user"])
            )

            request(app)
                .get("/user/tj/edit")
                .expect("editing tj", d)
                |> ignore
        )

        itAsync "should work following a partial capture group" (fun d ->
            let app = Express.e.express()
            let cb : System.Func<obj option, unit> = After.e.after(2, d)

            app.get("/user(s)?/:user/:op", fun (req : Request) (res : Response) ->
                let endOfStr =
                    if not (isNull req.``params``.[0]) then
                        " (old)"
                    else
                        ""
                res.send(req.``params``.["op"] + "ing " + req.``params``.["user"] + endOfStr)
            )

            request(app)
                .get("/user/tj/edit")
                .expect("editing tj", cb)
                |> ignore

            request(app)
                .get("/users/tj/edit")
                .expect("editing tj (old)", cb)
                |> ignore
        )

        itAsync "should work inside literal parenthesis" (fun d ->
            let app = Express.e.express()

            app.get("/:user\\(:op\\)", fun (req : Request) (res : Response) ->
                res.send(req.``params``.["op"] + "ing " + req.``params``.["user"])
            )

            request(app)
                .get("/tj(edit)")
                .expect("editing tj", d)
                |> ignore
        )

        itAsync "should work in array of paths" (fun d ->
            let app = Express.e.express()
            let cb : System.Func<obj option, unit> = After.e.after(2, d)

            app.get(ResizeArray [|"/user/:user/poke"; "/user/:user/pokes"|], fun (req : Request) (res : Response) ->
                res.send("poking " + req.``params``.["user"])
            )

            request(app)
                .get("/user/tj/poke")
                .expect("poking tj", cb)
                |> ignore

            request(app)
                .get("/user/tj/pokes")
                .expect("poking tj", cb)
                |> ignore
        )
    )

    describe ":name?" (fun () ->
        itAsync "should denote an optional capture group" (fun d ->
            let app = Express.e.express()

            app.get("/user/:user/:op?", fun (req : Request) (res : Response) ->
                let op =
                    if isNull req.``params``.["op"] then
                        "view"
                    else
                        req.``params``.["op"]

                res.send(op + "ing " + req.``params``.["user"])
            )

            request(app)
                .get("/user/tj")
                .expect("viewing tj", d)
                |> ignore
        )

        itAsync "should populate the capture group" (fun d ->
            let app = Express.e.express()

            app.get("/user/:user/:op?", fun (req : Request) (res : Response) ->
                let op =
                    if isNull req.``params``.["op"] then
                        "view"
                    else
                        req.``params``.["op"]

                res.send(op + "ing " + req.``params``.["user"])
            )

            request(app)
                .get("/user/tj/edit")
                .expect("editing tj", d)
                |> ignore
        )
    )

    describe ".:name" (fun () ->
        itAsync "should denote a format" (fun d  ->
            let app = Express.e.express()

            app.get("/:name.:format", fun (req : Request) (res : Response) ->
                res.send(req.``params``.["name"] + " as " + req.``params``.["format"])
            )

            request(app)
                .get("/foo.json")
                .expect("foo as json", fun err ->
                    request(app)
                        .get("/foo")
                        .expect(404, d)
                        |> ignore
                )
                |> ignore
        )
    )

    describe ".:name?" (fun () ->
        itAsync "should denote an optional format" (fun d  ->
            let app = Express.e.express()

            app.get("/:name.:format?", fun (req : Request) (res : Response) ->
                let format =
                    if isNull req.``params``.["format"] then
                        "html"
                    else
                        req.``params``.["format"]

                res.send(req.``params``.["name"] + " as " + format)
            )

            request(app)
                .get("/foo")
                .expect("foo as html", fun err ->
                    request(app)
                        .get("/foo.json")
                        .expect("foo as json", d)
                        |> ignore
                )
                |> ignore
        )
    )


    describe "when next() is called" (fun () ->
        itAsync "should continue lookup" (fun d ->
            let app = Express.e.express()
            let calls = ResizeArray()

            app.get("/foo/:bar?", fun (req : Request) (res : Response) (next : NextFunction) ->
                calls.Add("/foo/:bar?")
                next.Invoke()
            )

            app.get("/bar", fun (req : Request) (res : Response) ->
                Assert.ok(0)
            )

            app.get("/foo", fun (req : Request) (res : Response) (next : NextFunction) ->
                calls.Add("/foo")
                next.Invoke()
            )

            app.get("/foo", fun (req : Request) (res : Response) (next : NextFunction) ->
                calls.Add("/foo 2")
                res.json(calls)
            )

            request(app)
                .get("/foo")
                .expect(200, ResizeArray(["/foo/:bar?"; "/foo"; "/foo 2"]), d)
                |> ignore
        )
    )

    describe "when next(\"route\") is called" (fun () ->
        itAsync "should jump to next route" (fun d ->
            let app = Express.e.express()

            let fn (req : Request) (res : Response) (next : NextFunction) =
                res.set("X-Hit", "1") |> ignore
                next.Invoke_route()

            app.get("/foo", fn, fun (req : Request) (res : Response) (next : NextFunction) ->
                res.``end``("failure")
            )

            app.get("/foo", fun (req : Request) (res : Response) ->
                res.``end``("success")
            )

            request(app)
                .get("/foo")
                .expect("X-Hit", "1")
                .expect(200, "success", d)
                |> ignore
        )
    )

    describe "when next(\"router\") is called" (fun () ->
        itAsync "should jump out of router" (fun d ->
            let app = Express.e.express()
            let router = Express.e.Router()

            let fn (req : Request) (res : Response) (next : NextFunction) =
                res.set("X-Hit", "1") |> ignore
                next.Invoke_router()

            router.get("/foo", fn, fun (req : Request) (res : Response) (next : NextFunction) ->
                res.``end``("failure")
            )

            router.get("/foo", fun (req : Request) (res : Response) (next : NextFunction) ->
                res.``end``("failure")
            )

            app.``use``(router)

            app.get("/foo", fun (req : Request) (res : Response) ->
                res.``end``("success")
            )

            request(app)
                .get("/foo")
                .expect("X-Hit", "1")
                .expect(200, "success", d)
                |> ignore
        )
    )

    describe "when next(err) is called" (fun () ->
        itAsync "should break out of app.router" (fun d ->
            let app = Express.e.express()
            let calls = ResizeArray()

            app.get("/foo/:bar?", fun (req : Request) (res : Response) (next : NextFunction) ->
                calls.Add("/foo/:bar?")
                next.Invoke()
            )

            app.get("/bar", fun (req : Request) (res : Response) ->
                Assert.ok(0)
            )

            app.get("/foo", fun (req : Request) (res : Response) (next : NextFunction) ->
                calls.Add("/foo")
                next.Invoke(System.Exception("fail"))
            )

            app.get("/foo", fun (req : Request) (res : Response) (next : NextFunction) ->
                Assert.ok(0)
            )

            app.``use``(fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                res.json(createObj [
                    "calls" ==> calls
                    "error" ==> err.Value.Message
                ])
            )

            request(app)
                .get("/foo")
                .expect(
                    200,
                    createObj [
                        "calls" ==> [| "/foo/:bar?"; "/foo" |]
                        "error" ==> "fail"
                    ],
                    d
                )
                |> ignore
        )


        itAsync "should call handler in same route, if exists" (fun d ->
            let app = Express.e.express()

            let fn1 =
                fun (req : Request) (res : Response) (next : NextFunction) ->
                    next.Invoke(System.Exception("boom!"))
                |> Adapter.RequestHandler

            let fn2 =
                fun (req : Request) (res : Response) (next : NextFunction) ->
                    res.send("foo here")
                |> Adapter.RequestHandler

            let fn3 =
                fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                    res.send("route go " + err.Value.Message)
                |> Adapter.RequestHandler

            app.get("/foo", fn1, fn2, fn3)

            app.``use``(fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                res.``end``("error!")
            )

            request(app)
                .get("/foo")
                .expect("route go boom!", d)
                |> ignore

        )
    )

    itAsync "should allow rewriting of the url" (fun d ->
        let app = Express.e.express()

        app.get("/account/edit", fun (req : Request) (res : Response) (next : NextFunction) ->
            req?user <- {| id = 12  |} // Faux authenticated user
            req.url <- "/user/" + req?user?id + "/edit"
            next.Invoke()
        )

        app.get("/user/:id/edit", fun (req : Request) (res : Response) ->
            res.send("editing user " + req.``params``.["id"])
        )

        request(app)
            .get("/account/edit")
            .expect("editing user 12", d)
            |> ignore
    )

    itAsync "should run in order added" (fun d ->
        let app = Express.e.express()
        let path = ResizeArray()

        app.get("*", fun (req : Request) (res : Response) (next : NextFunction) ->
            path.Add(0)
            next.Invoke()
        )

        app.get("/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
            path.Add(1)
            next.Invoke()
        )

        app.``use``(fun (req : Request) (res : Response) (next : NextFunction) ->
            path.Add(2)
            next.Invoke()
        )

        app.all("/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
            path.Add(3)
            next.Invoke()
        )

        app.all("*", fun (req : Request) (res : Response) (next : NextFunction) ->
            path.Add(4)
            next.Invoke()
        )

        app.``use``(fun (req : Request) (res : Response) (next : NextFunction) ->
            path.Add(5)
            res.``end``(System.String.Join(",", path.ToArray() |> Array.map string))
        )

        request(app)
            .get("/user/1")
            .expect(200, "0,1,2,3,4,5", d)
            |> ignore
    )

    it "should be chainable" (fun () ->
        let app = Express.e.express()

        Assert.strictEqual(app.get("/", fun (req : Request) (res : Response) -> ()), app)
    )
)
