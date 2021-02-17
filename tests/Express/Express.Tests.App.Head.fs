module Tests.Express.App.Head

open Mocha
open ExpressServeStaticCore
open Fable.Core.JsInterop

let tests =
    describe "HEAD" (fun _ ->

        itAsync "should default to GET" (fun d ->
            let app = Express.e.express()

            app.get("/tobi", fun req (res : Response<_,_>) next ->
                res.send("tobi")
            )

            request(app)
                .head("/tobi")
                .expect(200, d)
                |> ignore
        )

        itAsync "should output the same headers as GET requests" (fun d ->
            let app = Express.e.express()

            app.get("/tobi", fun req (res : Response<_,_>) next ->
                res.send("tobi")
            )

            request(app)
                .get("/tobi")
                .expect(
                    200,
                   (fun err res ->
                        if err.IsSome then
                            d err

                        let headers = res.headers

                        request(app)
                            .get("/tobi")
                            .expect(
                                200,
                                (fun err res ->
                                    if err.IsSome then
                                        d err

                                    jsDelete headers.Value?date
                                    jsDelete res.headers.Value?date

                                    Assert.deepStrictEqual(res.headers, headers)
                                    d()
                                )
                            )
                            |> ignore
                    )
                )
                |> ignore
        )

    )

    describe "app.head()" (fun _ ->

        itAsync "should override" (fun d ->

            let app = Express.e.express()
            let mutable called = false

            app.head("/tobi", fun req (res : Response<_,_>) next ->
                called <- true
                res.``end``()
            )

            app.get("/tobi", fun req (res : Response<_,_>) next ->
                Assert.fail("should not call GET")
                res.send("tobi")
            )

            request(app)
                .head("/tobi")
                .expect(
                    200,
                    (fun err _ ->
                        Assert.strictEqual(called, true)
                        d ()
                    )
                )
                |> ignore

        )

    )
