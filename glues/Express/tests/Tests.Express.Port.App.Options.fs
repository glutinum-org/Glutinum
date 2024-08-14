module Tests.Express.Port.App.Options

open Mocha
open Glutinum.ExpressServeStaticCore
open Glutinum.Express

describe "OPTIONS" (fun _ ->

    itAsync "should default to the routes defined" (fun d ->
        let app = express.express ()

        app.del("/", fun (_ : Request) (_ : Response) -> ())
        app.get("/users", fun _ _ -> ())
        app.put("/users", fun _ _ -> ())

        request(app)
            .options("/users")
            .expect("Allow", "GET,HEAD,PUT")
            .expect(200, "GET,HEAD,PUT", d)
            |> ignore
    )

    itAsync "should only include each method once" (fun d ->
        let app = express.express ()

        app.del("/", fun (_ : Request) (_ : Response) -> ())
        app.get("/users", fun _ _ -> ())
        app.put("/users", fun _ _ -> ())
        app.get("/users", fun _ _ -> ())

        request(app)
            .options("/users")
            .expect("Allow", "GET,HEAD,PUT")
            .expect(200, "GET,HEAD,PUT", d)
            |> ignore

    )

    itAsync "should not be affected by app.all" (fun d ->
        let app = express.express ()

        app.del("/", fun (_ : Request) (_ : Response) -> ())
        app.get("/users", fun _ _ -> ())
        app.put("/users", fun _ _ -> ())
        app.all("/users", fun (req : Request) (res : Response) (next : NextFunction) ->
            res.setHeader("x-hit", "1")
            next.Invoke()
        )

        request(app)
            .options("/users")
            .expect("x-hit", "1")
            .expect("Allow", "GET,HEAD,PUT")
            .expect(200, "GET,HEAD,PUT", d)
            |> ignore

    )

    itAsync "should not respond if the path is not defined" (fun d ->
        let app = express.express ()

        app.get("/users", fun _ _ -> ())

        request(app)
            .options("/other")
            .expect(404, d)
            |> ignore
    )

    itAsync "should forward requests down the middleware chain" (fun d ->
        let app = express.express ()
        let router = express.Router()

        router.get("/users", fun _ _ -> ())
        app.``use``(router)
        app.get("/other", fun _ _ -> ())

        request(app)
            .options("/other")
            .expect("Allow", "GET,HEAD")
            .expect(200, "GET,HEAD", d)
            |> ignore

    )

    describe "when error occurs in response handler" (fun _ ->

        itAsync "should pass error to callback" (fun d ->
            let app = express.express ()
            let router = express.Router()

            router.get("/users", fun _ _ -> ())

            app.``use``(fun (req : Request) (res : Response) (next : NextFunction) ->
                res.writeHead(200)
                next.Invoke()
            )

            app.``use``(router)
            app.``use``(fun err req (res : Response) next ->
                res.``end``("true")
            )

            request(app)
                .options("/users")
                .expect(200, "true", d)
                |> ignore
        )

    )
)

describe "app.options()" (fun _ ->

    itAsync "should override the default behaviour" (fun d ->
        let app = express.express ()

        app.options("/users", fun (req : Request) (res : Response) ->
            res.set("Allow", "GET") |> ignore
            res.send("GET")
        )

        app.get("/users", fun _ _ -> ())
        app.put("/users", fun _ _ -> ())

        request(app)
            .options("/users")
            .expect("GET")
            .expect("Allow", "GET", d)
            |> ignore

    )

)
