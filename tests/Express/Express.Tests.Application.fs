module Express.Tests.Application

open Mocha
open ExpressServeStaticCore
open System.Text.RegularExpressions
open Fable.Core

describe "Express" (fun () ->

    describe "Application" (fun () ->

        describe ".get" (fun () ->

            itAsync "should have an overload taking a U3<string, RegExp, Array<U2<string, RegExp>>> and a 'simple handler'" (fun d ->
                let app = Express.e.express()

                app.get(U3.Case1 "/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .get("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a string and a 'simple handler'" (fun d ->
                let app = Express.e.express()

                app.get("/user/:id", fun (req : Request) (res : Response) ->
                    res.``end``(req.``params``.["id"])
                )

                request(app)
                    .get("/user/1")
                    .expect(200, "1", d)
                    |> ignore
            )

            itAsync "should have an overload taking a RegExp and a 'simple handler'" (fun d ->
                let app = Express.e.express()

                app.get(Regex("^\/user\/([0-9]+)$"), fun (req : Request) (res : Response) ->
                    res.``end``("fetching user " + req.``params``.[0])
                )

                request(app)
                    .get("/user/12")
                    .expect(200, "fetching user 12", d)
                    |> ignore
            )

            itAsync "should have an overload taking an array of string and a 'simple handler'" (fun d ->
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()

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
                let app = Express.e.express()

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
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()

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
                let app = Express.e.express()

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
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()
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
                let app = Express.e.express()

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

    )
)
