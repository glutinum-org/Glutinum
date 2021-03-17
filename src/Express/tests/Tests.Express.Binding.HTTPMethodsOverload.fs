module Tests.Express.Binding.HTTPMethodsOverload

open Mocha
open Glutinum.ExpressServeStaticCore
open System.Text.RegularExpressions
open Fable.Core
open Glutinum.Express

describe "Express" (fun () ->

    describe "Application" (fun () ->

        describe ".get" (fun () ->

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'simple handler'" (fun d ->
                let app = express.express()

                app.get(U3.Case1 "/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .get("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'simple handler'" (fun d ->
                let app = express.express()

                app.get("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .get("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'simple handler'" (fun d ->
                let app = express.express()

                app.get(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``("fetching user " + req.``params``.[0])
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "fetching user 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``()
                )

                request(app)
                    .get("/user/1")
                    .expect(200, fun _ ->
                        request(app)
                            .get("/post/13")
                            .expect(200, fun _ ->
                                Assert.strictEqual(count, 2)
                                d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .get("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    let result =
                        if req.url.Contains("user") then
                            req.``params``.["userId"]
                        else
                            req.``params``.[0]
                    res.``end``(result)
                )

                request(app)
                    .get("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(U3.Case1 "/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.get("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .get("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get("/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.get("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .get("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.get("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .get("/user/12")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.get("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.get("/post/:postId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["postId"])
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .get("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.get(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                app.get(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .get("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.get("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.get(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .get("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.get("/user",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user")
                    .expect(200, "handler 1 reached\nhandler 2 reached", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.get(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached for user: " + req.``params``.[0])
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "handler 1 reached\nhandler 2 reached for user: 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(count, 2)
                            d ()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and an ErrorHandler" (fun d ->
                let app = express.express()

                app.get("/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and an ErrorHandler" (fun d ->
                let app = express.express()

                app.get(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.get(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and an ErrorHandler" (fun d ->
                let app = express.express()

                app.get(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

        )

        describe ".post" (fun () ->

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'simple handler'" (fun d ->
                let app = express.express()

                app.post(U3.Case1 "/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .post("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'simple handler'" (fun d ->
                let app = express.express()

                app.post("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .post("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'simple handler'" (fun d ->
                let app = express.express()

                app.post(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``("fetching user " + req.``params``.[0])
                )

                request(app)
                    .post("/user/12")
                    .expect(200, "fetching user 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``()
                )

                request(app)
                    .post("/user/1")
                    .expect(200, fun _ ->
                        request(app)
                            .post("/post/13")
                            .expect(200, fun _ ->
                                Assert.strictEqual(count, 2)
                                d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .post("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .post("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    let result =
                        if req.url.Contains("user") then
                            req.``params``.["userId"]
                        else
                            req.``params``.[0]
                    res.``end``(result)
                )

                request(app)
                    .post("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .post("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(U3.Case1 "/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.post("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .post("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post("/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.post("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .post("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.post("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .post("/user/12")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.post("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.post("/post/:postId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["postId"])
                )

                request(app)
                    .post("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .post("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.post(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                app.post(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .post("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .post("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.post("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.post(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .post("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .post("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.post("/user",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .post("/user")
                    .expect(200, "handler 1 reached\nhandler 2 reached", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.post(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached for user: " + req.``params``.[0])
                    |> Adapter.RequestHandler
                )

                request(app)
                    .post("/user/12")
                    .expect(200, "handler 1 reached\nhandler 2 reached for user: 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .post("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .post("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .post("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .post("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .post("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .post("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .post("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .post("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .post("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(count, 2)
                            d ()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and an ErrorHandler" (fun d ->
                let app = express.express()

                app.post("/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .post("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and an ErrorHandler" (fun d ->
                let app = express.express()

                app.post(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .post("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .post("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .post("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .post("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .post("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.post(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .post("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .post("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and an ErrorHandler" (fun d ->
                let app = express.express()

                app.post(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .post("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

        )

        describe ".put" (fun () ->

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'simple handler'" (fun d ->
                let app = express.express()

                app.put(U3.Case1 "/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .put("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'simple handler'" (fun d ->
                let app = express.express()

                app.put("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .put("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'simple handler'" (fun d ->
                let app = express.express()

                app.put(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``("fetching user " + req.``params``.[0])
                )

                request(app)
                    .put("/user/12")
                    .expect(200, "fetching user 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``()
                )

                request(app)
                    .put("/user/1")
                    .expect(200, fun _ ->
                        request(app)
                            .put("/post/13")
                            .expect(200, fun _ ->
                                Assert.strictEqual(count, 2)
                                d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .put("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .put("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    let result =
                        if req.url.Contains("user") then
                            req.``params``.["userId"]
                        else
                            req.``params``.[0]
                    res.``end``(result)
                )

                request(app)
                    .put("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .put("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(U3.Case1 "/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.put("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .put("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put("/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.put("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .put("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.put("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .put("/user/12")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.put("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.put("/post/:postId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["postId"])
                )

                request(app)
                    .put("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .put("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.put(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                app.put(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .put("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .put("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.put("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.put(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .put("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .put("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.put("/user",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .put("/user")
                    .expect(200, "handler 1 reached\nhandler 2 reached", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.put(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached for user: " + req.``params``.[0])
                    |> Adapter.RequestHandler
                )

                request(app)
                    .put("/user/12")
                    .expect(200, "handler 1 reached\nhandler 2 reached for user: 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .put("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .put("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .put("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .put("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .put("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .put("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .put("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .put("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .put("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(count, 2)
                            d ()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and an ErrorHandler" (fun d ->
                let app = express.express()

                app.put("/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .put("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and an ErrorHandler" (fun d ->
                let app = express.express()

                app.put(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .put("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .put("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .put("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .put("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .put("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.put(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .put("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .put("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and an ErrorHandler" (fun d ->
                let app = express.express()

                app.put(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .put("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

        )

        describe ".del" (fun () ->

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'simple handler'" (fun d ->
                let app = express.express()

                app.del(U3.Case1 "/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .del("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'simple handler'" (fun d ->
                let app = express.express()

                app.del("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .del("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'simple handler'" (fun d ->
                let app = express.express()

                app.del(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``("fetching user " + req.``params``.[0])
                )

                request(app)
                    .del("/user/12")
                    .expect(200, "fetching user 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``()
                )

                request(app)
                    .del("/user/1")
                    .expect(200, fun _ ->
                        request(app)
                            .del("/post/13")
                            .expect(200, fun _ ->
                                Assert.strictEqual(count, 2)
                                d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .del("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .del("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    let result =
                        if req.url.Contains("user") then
                            req.``params``.["userId"]
                        else
                            req.``params``.[0]
                    res.``end``(result)
                )

                request(app)
                    .del("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .del("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(U3.Case1 "/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.del("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .del("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del("/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.del("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .del("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.del("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .del("/user/12")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.del("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.del("/post/:postId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["postId"])
                )

                request(app)
                    .del("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .del("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.del(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                app.del(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .del("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .del("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.del("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.del(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .del("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .del("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.del("/user",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .del("/user")
                    .expect(200, "handler 1 reached\nhandler 2 reached", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.del(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached for user: " + req.``params``.[0])
                    |> Adapter.RequestHandler
                )

                request(app)
                    .del("/user/12")
                    .expect(200, "handler 1 reached\nhandler 2 reached for user: 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .del("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .del("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .del("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .del("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .del("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .del("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .del("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .del("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .del("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(count, 2)
                            d ()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and an ErrorHandler" (fun d ->
                let app = express.express()

                app.del("/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .del("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and an ErrorHandler" (fun d ->
                let app = express.express()

                app.del(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .del("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .del("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .del("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .del("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .del("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.del(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .del("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .del("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and an ErrorHandler" (fun d ->
                let app = express.express()

                app.del(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .del("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

        )

        describe ".delete" (fun () ->

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'simple handler'" (fun d ->
                let app = express.express()

                app.delete(U3.Case1 "/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .delete("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'simple handler'" (fun d ->
                let app = express.express()

                app.delete("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .delete("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'simple handler'" (fun d ->
                let app = express.express()

                app.delete(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``("fetching user " + req.``params``.[0])
                )

                request(app)
                    .delete("/user/12")
                    .expect(200, "fetching user 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``()
                )

                request(app)
                    .delete("/user/1")
                    .expect(200, fun _ ->
                        request(app)
                            .delete("/post/13")
                            .expect(200, fun _ ->
                                Assert.strictEqual(count, 2)
                                d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .delete("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .delete("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    let result =
                        if req.url.Contains("user") then
                            req.``params``.["userId"]
                        else
                            req.``params``.[0]
                    res.``end``(result)
                )

                request(app)
                    .delete("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .delete("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(U3.Case1 "/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.delete("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .delete("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete("/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.delete("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .delete("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.delete("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .delete("/user/12")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.delete("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.delete("/post/:postId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["postId"])
                )

                request(app)
                    .delete("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .delete("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.delete(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                app.delete(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .delete("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .delete("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.delete("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.delete(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .delete("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .delete("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.delete("/user",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .delete("/user")
                    .expect(200, "handler 1 reached\nhandler 2 reached", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.delete(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached for user: " + req.``params``.[0])
                    |> Adapter.RequestHandler
                )

                request(app)
                    .delete("/user/12")
                    .expect(200, "handler 1 reached\nhandler 2 reached for user: 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .delete("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .delete("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .delete("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .delete("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .delete("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .delete("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .delete("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .delete("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .delete("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(count, 2)
                            d ()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and an ErrorHandler" (fun d ->
                let app = express.express()

                app.delete("/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .delete("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and an ErrorHandler" (fun d ->
                let app = express.express()

                app.delete(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .delete("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .delete("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .delete("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .delete("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .delete("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.delete(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .delete("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .delete("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and an ErrorHandler" (fun d ->
                let app = express.express()

                app.delete(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .delete("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

        )

        describe ".all" (fun () ->

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'simple handler'" (fun d ->
                let app = express.express()

                app.all(U3.Case1 "/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .get("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'simple handler'" (fun d ->
                let app = express.express()

                app.all("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .get("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'simple handler'" (fun d ->
                let app = express.express()

                app.all(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``("fetching user " + req.``params``.[0])
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "fetching user 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``()
                )

                request(app)
                    .get("/user/1")
                    .expect(200, fun _ ->
                        request(app)
                            .get("/post/13")
                            .expect(200, fun _ ->
                                Assert.strictEqual(count, 2)
                                d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .get("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    let result =
                        if req.url.Contains("user") then
                            req.``params``.["userId"]
                        else
                            req.``params``.[0]
                    res.``end``(result)
                )

                request(app)
                    .get("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(U3.Case1 "/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.all("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .get("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all("/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.all("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .get("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.all("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .get("/user/12")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.all("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.all("/post/:postId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["postId"])
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .get("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.all(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                app.all(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .get("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.all("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.all(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .get("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.all("/user",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user")
                    .expect(200, "handler 1 reached\nhandler 2 reached", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.all(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached for user: " + req.``params``.[0])
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "handler 1 reached\nhandler 2 reached for user: 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(count, 2)
                            d ()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and an ErrorHandler" (fun d ->
                let app = express.express()

                app.all("/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and an ErrorHandler" (fun d ->
                let app = express.express()

                app.all(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.all(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .get("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and an ErrorHandler" (fun d ->
                let app = express.express()

                app.all(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

        )

        describe ".patch" (fun () ->

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'simple handler'" (fun d ->
                let app = express.express()

                app.patch(U3.Case1 "/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .patch("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'simple handler'" (fun d ->
                let app = express.express()

                app.patch("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .patch("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'simple handler'" (fun d ->
                let app = express.express()

                app.patch(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``("fetching user " + req.``params``.[0])
                )

                request(app)
                    .patch("/user/12")
                    .expect(200, "fetching user 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``()
                )

                request(app)
                    .patch("/user/1")
                    .expect(200, fun _ ->
                        request(app)
                            .patch("/post/13")
                            .expect(200, fun _ ->
                                Assert.strictEqual(count, 2)
                                d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .patch("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .patch("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    let result =
                        if req.url.Contains("user") then
                            req.``params``.["userId"]
                        else
                            req.``params``.[0]
                    res.``end``(result)
                )

                request(app)
                    .patch("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .patch("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(U3.Case1 "/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.patch("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .patch("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch("/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.patch("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .patch("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.patch("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .patch("/user/12")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.patch("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.patch("/post/:postId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["postId"])
                )

                request(app)
                    .patch("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .patch("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.patch(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                app.patch(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .patch("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .patch("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.patch("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.patch(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .patch("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .patch("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.patch("/user",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .patch("/user")
                    .expect(200, "handler 1 reached\nhandler 2 reached", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.patch(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached for user: " + req.``params``.[0])
                    |> Adapter.RequestHandler
                )

                request(app)
                    .patch("/user/12")
                    .expect(200, "handler 1 reached\nhandler 2 reached for user: 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .patch("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .patch("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .patch("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .patch("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .patch("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .patch("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .patch("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .patch("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .patch("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(count, 2)
                            d ()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and an ErrorHandler" (fun d ->
                let app = express.express()

                app.patch("/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .patch("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and an ErrorHandler" (fun d ->
                let app = express.express()

                app.patch(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .patch("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .patch("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .patch("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .patch("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .patch("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.patch(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .patch("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .patch("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and an ErrorHandler" (fun d ->
                let app = express.express()

                app.patch(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .patch("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

        )

        describe ".options" (fun () ->

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'simple handler'" (fun d ->
                let app = express.express()

                app.options(U3.Case1 "/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .options("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'simple handler'" (fun d ->
                let app = express.express()

                app.options("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .options("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'simple handler'" (fun d ->
                let app = express.express()

                app.options(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``("fetching user " + req.``params``.[0])
                )

                request(app)
                    .options("/user/12")
                    .expect(200, "fetching user 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``()
                )

                request(app)
                    .options("/user/1")
                    .expect(200, fun _ ->
                        request(app)
                            .options("/post/13")
                            .expect(200, fun _ ->
                                Assert.strictEqual(count, 2)
                                d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .options("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .options("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    let result =
                        if req.url.Contains("user") then
                            req.``params``.["userId"]
                        else
                            req.``params``.[0]
                    res.``end``(result)
                )

                request(app)
                    .options("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .options("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(U3.Case1 "/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.options("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .options("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options("/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.options("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .options("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.options("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .options("/user/12")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.options("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.options("/post/:postId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["postId"])
                )

                request(app)
                    .options("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .options("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.options(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                app.options(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .options("/user/12")
                    .expect(200, "12", fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .options("/post/13")
                            .expect(200, "13", fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.options("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["userId"])
                )

                app.options(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.[0])
                )

                request(app)
                    .options("/user/1")
                    .expect(200, "1", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .options("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.options("/user",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .options("/user")
                    .expect(200, "handler 1 reached\nhandler 2 reached", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and a list of RequestHandler" (fun d ->
                let app = express.express()

                app.options(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        res.write("handler 1 reached\n") |> ignore
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        res.``end``("handler 2 reached for user: " + req.``params``.[0])
                    |> Adapter.RequestHandler
                )

                request(app)
                    .options("/user/12")
                    .expect(200, "handler 1 reached\nhandler 2 reached for user: 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .options("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .options("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .options("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .options("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .options("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .options("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .options("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .options("/post/33")
                                .expect(200, "/post/33", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .options("/user/7")
                    .expect(200, "/user/7", fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(count, 2)
                            d ()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and an ErrorHandler" (fun d ->
                let app = express.express()

                app.options("/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .options("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and an ErrorHandler" (fun d ->
                let app = express.express()

                app.options(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .options("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .options("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .options("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .options("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .options("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.options(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .options("/user/12")
                    .expect(200, "captured", fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .options("/post/33")
                                .expect(200, "captured", fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and an ErrorHandler" (fun d ->
                let app = express.express()

                app.options(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .options("/user/12")
                    .expect(200, "captured", d)
                    |> ignore
            )

        )

        describe ".head" (fun () ->

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable called = false

                app.head(U3.Case1 "/user/:id", fun (req : Request) (res : Response) ->
                    called <- true
                    res.``end``()
                )

                request(app)
                    .head("/user/1")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(called, true)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable called = false

                app.head("/user/:id", fun (req : Request) (res : Response) ->
                    called <- true
                    res.``end``()
                )

                request(app)
                    .head("/user/1")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(called, true)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable called = false

                app.head(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    called <- true
                    res.``end``()
                )

                request(app)
                    .head("/user/12")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(called, true)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``()
                )

                request(app)
                    .head("/user/1")
                    .expect(200, fun _ ->
                        request(app)
                            .head("/post/13")
                            .expect(200, fun _ ->
                                Assert.strictEqual(count, 2)
                                d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``()
                )

                request(app)
                    .head("/user/1")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .head("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'simple handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) ->
                    count <- count + 1
                    res.``end``()
                )

                request(app)
                    .head("/user/1")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .head("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(U3.Case1 "/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.head("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .head("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head("/user/:id", fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.head("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .head("/user/1")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.head("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .head("/user/12")
                    .expect(
                        200,
                        fun _ ->
                            Assert.strictEqual(count, 1)
                            d()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(ResizeArray(["/user/:userId"; "/post/:postId"]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.head("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                app.head("/post/:postId", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .head("/user/12")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .head("/post/13")
                            .expect(200, fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.head(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                app.head(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .head("/user/12")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err

                        request(app)
                            .head("/post/13")
                            .expect(200, fun err ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    d()
                            )
                            |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a 'next handler'" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]), fun (req : Request) (res : Response) (next : NextFunction) ->
                    count <- count + 1
                    next.Invoke()
                )

                app.head("/user/:userId", fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                app.head(Regex("^\/post\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``()
                )

                request(app)
                    .head("/user/1")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .head("/post/13")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 2)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head("/user",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``()
                    |> Adapter.RequestHandler
                )

                request(app)
                    .head("/user")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(count, 2)
                            d ()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``()
                    |> Adapter.RequestHandler
                )

                request(app)
                    .head("/user/12")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(count, 2)
                            d ()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .head("/user/7")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .head("/post/33")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .head("/user/7")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .head("/post/33")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .head("/user/7")
                    .expect(200,  fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .head("/post/33")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|RegExp and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .head("/user/7")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .head("/post/33")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a list of RequestHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke()
                    |> Adapter.RequestHandler,
                    fun (req : Request) (res : Response) ->
                        count <- count + 1
                        res.``end``(req.url)
                    |> Adapter.RequestHandler
                )

                request(app)
                    .head("/user/7")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(count, 2)
                            d ()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a string and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable captured = false

                app.head("/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        captured <- true
                        res.``end``()
                    |> Adapter.RequestHandler
                )

                request(app)
                    .head("/user/12")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(captured, true)
                            d ()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable captured = false

                app.head(Regex("^\/user\/([0-9]+)$"),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        captured <- true
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .head("/user/12")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(captured, true)
                            d ()
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(ResizeArray(["/user/:userId"; "/post/:postId"]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .head("/user/12")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .head("/post/33")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(ResizeArray([Regex("^\/user\/([0-9]+)$"); Regex("^\/post\/([0-9]+)$")]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .head("/user/12")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .head("/post/33")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking an array of string|Regex and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable count = 0

                app.head(ResizeArray([U2.Case1 "/user/:userId"; U2.Case2 (Regex("^\/post\/([0-9]+)$"))]),
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        count <- count + 1
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .head("/user/12")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            request(app)
                                .head("/post/33")
                                .expect(200, fun err ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 4)
                                        d ()
                                )
                                |> ignore
                    )
                    |> ignore
            )

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and an ErrorHandler" (fun d ->
                let app = express.express()
                let mutable captured = false

                app.head(U3.Case1 "/user/:userId",
                    fun (req : Request) (res : Response) (next : NextFunction) ->
                        next.Invoke(System.Exception("failing on purpose"))
                    |> Adapter.RequestHandler,
                    fun (err : Error option) (req : Request) (res : Response) (next : NextFunction) ->
                        Assert.strictEqual(err.Value.Message, "failing on purpose")
                        captured <- true
                        res.``end``("captured")
                    |> Adapter.RequestHandler
                )

                request(app)
                    .head("/user/12")
                    .expect(200, fun err ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(captured, true)
                            d ()
                    )
                    |> ignore
            )

        )

    )
)
