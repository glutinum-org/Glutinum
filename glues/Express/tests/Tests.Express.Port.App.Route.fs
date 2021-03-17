module Tests.Express.Port.App.Route

open Glutinum.ExpressServeStaticCore
open Glutinum.Express
open Mocha

describe "app.route" (fun _ ->
    itAsync "should return a new route" (fun d ->
        let app = express.express ()

        app.route("/foo")
            .get(fun (req : Request) (res : Response) ->
                res.send("get")
            )
            .post(fun (req : Request) (res : Response) ->
                res.send("post")
            )
            |> ignore

        request(app)
            .post("/foo")
            .expect("post", d)
            |> ignore
    )

    itAsync "should all .VERB after .all" (fun d ->
        let app = express.express ()

        app.route("/foo")
            .all(fun req res (next : NextFunction) ->
                next.Invoke()
            )
            .get(fun req (res : Response) ->
                res.send("get")
            )
            .post(fun (req : Request) (res : Response) ->
                res.send("post")
            )
            |> ignore

        request(app)
            .post("/foo")
            .expect("post", d)
            |> ignore
    )

    itAsync "should support dynamic routes" (fun d ->
        let app = express.express ()

        app.route("/:foo")
            .get(fun (req : Request) (res : Response) ->
                res.send(req.``params``.["foo"])
            )
            |> ignore

        request(app)
            .get("/test")
            .expect("test", d)
            |> ignore
    )

    itAsync "should not error on empty routes" (fun d ->
        let app = express.express ()

        app.route("/:foo") |> ignore

        request(app)
            .get("/test")
            .expect(404, d)
            |> ignore
    )

)
