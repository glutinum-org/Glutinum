module Tests.Express.App.Options

open Fable.Core.JS
open Fable.Core.JsInterop
open Npm
open Mocha
open ExpressServeStaticCore
// open Fable.Core.Testing

#nowarn "40"

let tests () =
    describe "OPTIONS" (fun _ ->

        itAsync "should default to the routes defined" (fun d ->
            let app = Express.e.express ()

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
            let app = Express.e.express ()

            app.del("/", fun (_ : Request) (_ : Response) -> ())
            app.get("/users", fun _ _ -> ())
            app.put("/users", fun _ _ -> ())
            app.get("/users", fun _ _ -> ())

            request(app)
                .options("/users")
                .expect("Allow", "GET,HEAD,PUT")
                .expect(
                    200,
                    "GET,HEAD,PUT",
                    fun err _ -> d err
                )
                |> ignore

        )

        itAsync "should not be affected by app.all" (fun d ->
            let app = Express.e.express ()

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
                .expect(
                    200,
                    "GET,HEAD,PUT",
                    fun err _ -> d err
                )
                |> ignore

        )

        itAsync "should not respond if the path is not defined" (fun d ->
            let app = Express.e.express ()

            app.get("/users", fun _ _ -> ())

            request(app)
                .options("/other")
                .expect(
                    404,
                    fun err _ -> d err
                )
                |> ignore
        )

        itAsync "should forward requests down the middleware chain" (fun d ->
            let app = Express.e.express()
            let router = Express.e.Router()

            router.get("/users", fun _ _ -> ())
            app.``use``(router)
            app.get("/other", fun _ _ -> ())

            request(app)
                .options("/other")
                .expect("Allow", "GET,HEAD")
                .expect(
                    200,
                    "GET,HEAD",
                    fun err _ -> d err
                )
                |> ignore

        )

        describe "when error occurs in response handler" (fun _ ->

            itAsync "should pass error to callback" (fun d ->
                let app = Express.e.express ()
                let router = Express.e.Router()

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
                    .expect(200, "true", fun err _ -> d err)
                    |> ignore
            )

        )
    )

    describe "app.options()" (fun _ ->

        itAsync "should override the default behaviour" (fun d ->
            let app = Express.e.express()

            app.options("/users", fun req (res : Response<_,_>) ->
                res.set("Allow", "GET") |> ignore
                res.send("GET")
            )

            app.get("/users", fun _ _ -> ())
            app.put("/users", fun _ _ -> ())

            request(app)
                .options("/users")
                .expect("GET")
                .expect("Allow", "GET", fun err _ -> d err)
                |> ignore

        )

    )



