module Tests.Express.App.Head

open Npm
open Mocha
open ExpressServeStaticCore
open Node
open Fable.Core.JsInterop
open Fable.Core
open Fable.Core.Testing

type CallbackHandler = Types.SuperTest.Supertest.CallbackHandler

// Import should package into the project
let jsAssert : obj = import "default" "assert"

#nowarn "40"

let tests () =
    describe "HEAD" (fun _ ->

        itAsync "should default to GET" (fun d ->
            let app = Express.e.express()

            app.get("/tobi", fun req (res : Response<_,_>) next ->
                res.send("tobi") |> ignore
            )

            request(app)
                .head("/tobi")
                .expect(
                    200,
                    CallbackHandler (fun err _ ->
                        d err
                    )
                )
                |> ignore
        )

        itAsync "should output the same headers as GET requests" (fun d ->
            let app = Express.e.express()

            app.get("/tobi", fun req (res : Response<_,_>) next ->
                res.send("tobi") |> ignore
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

                                    jsAssert?deepStrictEqual(res.headers, headers)
                                    d()

                                )
                            )
                            |> ignore
                    )
                )
                |> ignore
        )

    ) |> ignore

    describe "app.head()" (fun _ ->

        itAsync "should override" (fun d ->

            let app = Express.e.express()
            let mutable called = false

            app.head("/tobi", fun req (res : Response<_,_>) next ->
                called <- true
                res.``end``()
            )

            app.get("/tobi", fun req (res : Response<_,_>) next ->
                jsAssert$(0, "should not could GET")
                res.send("tobi") |> ignore
            )

            request(app)
                .head("/tobi")
                .expect(
                    200,
                    CallbackHandler (fun err _ ->
                        Assert.AreEqual(called, true)
                        d ()
                    )
                )
                |> ignore

        )

    )
