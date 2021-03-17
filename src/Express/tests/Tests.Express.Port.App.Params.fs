module Tests.Express.Port.App.Params

open System
open Fable.Core.JS
open Fable.Core.JsInterop
open Mocha
open ExpressServeStaticCore
open Node

describe "app" (fun _ ->

    describe ".param(fn)" (fun _ ->
        itAsync "should map app.param(name, ...) logic" (fun d ->
            let app = Express.e.express()

            app.param(fun name regexp ->
                if Constructors.Object?prototype?toString?call(regexp) = "[object RegExp]" then
                    RequestParamHandler(fun req res next value _ ->
                        let captures = regexp.Match(string value)

                        if captures.Success then
                            req.``params``.[name] <- captures.Groups.[0].Value
                            next.Invoke()
                        else
                            next.Invoke("route")
                    )
                else
                    // In F# else needs to be balance not as in JavaScript
                    // so try to hack around this "problem" as I don't know what should be the behaviour when
                    // the condition is not meat
                    // Source: https://github.com/expressjs/express/blob/508936853a6e311099c9985d4c11a4b1b8f6af07/test/app.param.js
                    returnNothingHack
            )

            app.param(":name", RegExp("^([a-zA-Z]+)$"))

            app.get("/user/:name", fun (req : Request) (res : Response) ->
                res.send(req.``params``?("name"))
            )

            request(app)
                .get("/user/tj")
                .expect(
                    200,
                    "tj",
                    fun err _ ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/user/123")
                                .expect(404, d)
                                |> ignore
                )
                |> ignore
        )
    )

    describe ".params(names, fn)" (fun _ ->
        itAsync "should map the array" (fun d ->
            let app = Express.e.express()

            app.param(ResizeArray(["id"; "uid"]), fun (req : Request) res next id ->
                let id = Constructors.Number.Create(id)

                if isNaN(id) then
                    next.Invoke("route")
                else
                    req.``params``.["id"] <- unbox id
                    next.Invoke()
            )

            app.get("/post/:id", fun (req : Request) (res : Response) ->
                let id = req.``params``.["id"]
                if jsTypeOf id <> "number" then
                    d (Exception("Should be of type number"))

                res.send("" + unbox id) // id is a Number in JavaScript force a conversion to string
            )

            app.get("/user/:uid", fun (req : Request) (res : Response) ->
                let id = req.``params``.["id"]
                if jsTypeOf id <> "number" then
                    d (Exception("Should be of type number"))

                res.send("" + unbox id) // id is a Number in JavaScript force a conversion to string
            )

            request(app)
                .get("/user/123")
                .expect(
                    200,
                    "123",
                    fun err _ ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/123")
                                .expect(
                                    200,"123", d)
                                |> ignore
                )
                |> ignore
        )
    )

    describe ".params(name, fn)" (fun _ ->
        itAsync "should map logic for a single param" (fun d ->
            let app = Express.e.express()

            app.param("id", fun req res next id ->
                let id = Constructors.Number.Create(id)

                if isNaN(id) then
                    next.Invoke("route")
                else
                    req.``params``.["id"] <- unbox id
                    next.Invoke()
            )

            app.get("/user/:id", fun (req : Request) (res : Response) ->
                let id = req.``params``.["id"]

                if jsTypeOf id <> "number" then
                    d (Exception("Should be of type number"))

                res.send("" + unbox id) // id is a Number in JavaScript force a conversion to string
            )

            request(app)
                .get("/user/123")
                .expect("123", d)
                |> ignore
        )

        itAsync "should only call once per request" (fun d ->
            let app = Express.e.express()
            let mutable called = 0
            let mutable count = 0

            app.param("user", fun req res next user ->
                called <- called + 1
                req?user <- user
                next.Invoke()
            )

            app.get("/foo/:user", fun (req : Request) (res : Response) (next : NextFunction) ->
                count <- count + 1
                next.Invoke()
            )

            app.get("/foo/:user", fun (req : Request) (res : Response) (next : NextFunction) ->
                count <- count + 1
                next.Invoke()
            )

            app.``use``(fun req res ->
                let text = String.Join(" ", [string count; string called; req?user])
                res.``end``(text)
            )

            request(app)
                .get("/foo/bob")
                .expect("2 1 bob", d)
                |> ignore
        )


        itAsync "should call when values differ" (fun d ->
            let app = Express.e.express()
            let mutable called = 0
            let mutable count = 0

            app.param("user", fun req res next user ->
                called <- called + 1
                req?users <-
                    if isNull req?users then
                        [ user ]
                    else
                        req?users @ [ user ]

                next.Invoke()
            )

            app.get("/:user/bob", fun (req : Request) (res : Response) (next : NextFunction) ->
                count <- count + 1
                next.Invoke()
            )

            app.get("/foo/:user", fun (req : Request) (res : Response) (next : NextFunction) ->
                count <- count + 1
                next.Invoke()
            )

            app.``use``(fun req res ->
                let users = String.Join(",", unbox<string list> req?users)
                let text = String.Join(" ", [string count; string called; users])
                res.``end``(text)
            )

            request(app)
                .get("/foo/bob")
                .expect("2 2 foo,bob", d)
                |> ignore
        )

        itAsync "should support altering req.params across routes" (fun d ->
            let app = Express.e.express()

            app.param("user", fun req res next user ->
                req.``params``.["user"] <- "loki"
                next.Invoke()
            )

            app.get("/:user", fun (req : Request) (res : Response) (next : NextFunction) ->
                next.Invoke("route")
            )

            app.get("/:user", fun (req : Request) (res : Response) next ->
                res.send(req.``params``.["user"])
            )

            request(app)
                .get("/bob")
                .expect("loki", d)
                |> ignore
        )

        itAsync "should not invoke without route handler" (fun d ->
            let app = Express.e.express()

            app.param("thing", fun req res next thing ->
                req?thing <- thing
                next.Invoke()
            )

            app.param("user", fun req res next user ->
                next.Invoke(Exception("invalid invocation"))
            )

            app.post("/:user", fun (req : Request) (res : Response) (next : NextFunction) ->
                res.send(req.``params``.["user"])
            )

            app.get("/:thing", fun (req : Request) (res : Response) (next : NextFunction) ->
                res.send(req?thing)
            )

            request(app)
                .get("/bob")
                .expect(200, "bob", d)
                |> ignore
        )

        itAsync "should work with encoded values" (fun d ->
            let app = Express.e.express()

            app.param("name", fun req res next name ->
                req.``params``.["name"] <- string name
                next.Invoke()
            )

            app.get("/user/:name", fun (req : Request) (res : Response) ->
                let name = req.``params``.["name"]
                res.send(name)
            )

            request(app)
                .get("/user/foo%25bar")
                .expect("foo%bar", d)
                |> ignore
        )

        itAsync "should catch thrown error" (fun d ->
            let app = Express.e.express()

            app.param("id", fun req res next id ->
                raise (Exception("err!"))
            )

            app.get("/user/:id", fun (req : Request) (res : Response) ->
                let id = req.``params``.["id"]
                res.send(id)
            )

            request(app)
                .get("/user/123")
                .expect(500, d)
                |> ignore
        )

        itAsync "should catch thrown secondary error" (fun d ->
            let app = Express.e.express()

            app.param("id", fun req res next value ->
                ``process``.nextTick(unbox next)
            )

            app.param("id", fun req res next id ->
                raise (Exception("err!"))
            )

            app.get("/user/:id", fun (req : Request) (res : Response) ->
                let id = req.``params``.["id"]
                res.send(string id)
            )

            request(app)
                .get("/user/123")
                .expect(500, d)
                |> ignore
        )

        itAsync "should defer to next route" (fun d ->
            let app = Express.e.express()

            app.param("id", fun req res next id ->
                next.Invoke("route")
            )

            app.get("/user/:id", fun (req : Request) (res : Response) ->
                let id = req.``params``.["id"]
                res.send(string id)
            )

            app.get("/:name/123", fun (req : Request) (res : Response) ->
                res.send("name")
            )

            request(app)
                .get("/user/123")
                .expect("name", d)
                |> ignore
        )

        itAsync "should defer all the param routes" (fun d ->
            let app = Express.e.express()

            app.param("id", fun req res next value ->
                if value = box "new" then
                    next.Invoke("route")
                else
                    next.Invoke()
            )

            app.all("/user/:id", fun (req : Request) (res : Response) ->
                res.send("all.id")
            )

            app.get("/user/:id", fun (req : Request) (res : Response) ->
                res.send("get.id")
            )

            app.get("/user/new", fun (req : Request) (res : Response) ->
                res.send("get.new")
            )

            request(app)
                .get("/user/new")
                .expect("get.new", d)
                |> ignore
        )

        itAsync "should not call when values differ on error" (fun d ->
            let app = Express.e.express()
            let mutable called = 0
            let mutable count = 0

            app.param("user", fun req res next user ->
                called <- called + 1
                if (user = box "foo") then
                    raise (Exception("err!"))

                req?user <- user
                next.Invoke()
            )

            app.get("/:user/bob", fun (req : Request) (res : Response) (next : NextFunction) ->
                count <- count + 1
                next.Invoke()
            )

            app.get("/foo/:user", fun (req : Request) (res : Response) (next : NextFunction) ->
                count <- count + 1
                next.Invoke()
            )

            app.``use``(fun err req (res : Response) next ->
                res.status(500)
                let text = String.Join(" ", [string count; string called; err.Value.Message])
                res.send(text)
            )

            request(app)
                .get("/foo/bob")
                .expect(500, "0 1 err!", d)
                |> ignore
        )

        itAsync "should call when values differ when using 'next'" (fun d ->
            let app = Express.e.express()
            let mutable called = 0
            let mutable count = 0

            app.param("user", fun req res next user ->
                called <- called + 1
                if (user = box "foo") then
                    next.Invoke("route")
                req?user <- user
                next.Invoke()
            )

            app.get("/:user/bob", fun (req : Request) (res : Response) (next : NextFunction) ->
                count <- count + 1
                next.Invoke()
            )

            app.get("/foo/:user", fun (req : Request) (res : Response) (next : NextFunction) ->
                count <- count + 1
                next.Invoke()
            )

            app.``use``(fun req res ->
                let text = String.Join(" ", [string count; string called; req?user])
                res.``end``(text)
            )

            request(app)
                .get("/foo/bob")
                .expect("1 2 bob", d)
                |> ignore
        )

    )
)
