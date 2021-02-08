module Tests.Express.App.Router

open System.Text.RegularExpressions
open ExpressServeStaticCore
open Fable.Core.JsInterop
open Npm
open Mocha

let tests () =
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

                        emitJsStatement (app, npm.supertest.supertest, method, d) """
$1($0)
    [$2]('/foo')
    .expect(200, $3)
"""
                    )

                    it ("should reject numbers for app." + (string method)) (fun _ ->
                        let app = Express.e.express()

                        Assert.throws(app?(method)?bind$(app, "/", 3)
                    )
                )
            )


            itAsync "should re-route when method is altered" (fun d ->
                let app = Express.e.express()
                let cb : System.Func<obj option,Types.SuperTest.Supertest.Response,unit> = After.e.after(3, d)

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

    )
