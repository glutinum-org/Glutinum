module Tests.Express.App.Route

open ExpressServeStaticCore
open Fable.Core.JsInterop
open Mocha
open Node

let tests =
    describe "app.route" (fun _ ->
        itAsync "should return a new route" (fun d ->
            let app = Express.e.express()

            app.route("/foo")
                .get(fun req res ->
                    res.send("get")
                )
                .post(fun req res ->
                    res.send("post")
                )
                |> ignore

            request(app)
                .post("/foo")
                .expect("post", d)
                |> ignore
        )

        itAsync "should all .VERB after .all" (fun d ->
            let app = Express.e.express()

            app.route("/foo")
                .all(fun req res next ->
                    next.Invoke()
                )
                .get(fun req res ->
                    res.send("get")
                )
                .post(fun req res ->
                    res.send("post")
                )
                |> ignore

            request(app)
                .post("/foo")
                .expect("post", d)
                |> ignore
        )

        itAsync "should support dynamic routes" (fun d ->
            let app = Express.e.express()

            app.route("/:foo")
                .get(fun req res ->
                    res.send(req.``params``.["foo"])
                )
                |> ignore

            request(app)
                .get("/test")
                .expect("test", d)
                |> ignore
        )

        itAsync "should not error on empty routes" (fun d ->
            let app = Express.e.express()

            app.route("/:foo") |> ignore

            request(app)
                .get("/test")
                .expect(404, d)
                |> ignore
        )

    )
