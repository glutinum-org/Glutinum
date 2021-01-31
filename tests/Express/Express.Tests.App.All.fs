module Tests.Express.App.All

open Npm
open Mocha
open ExpressServeStaticCore

type CallbackHandler = Types.SuperTest.Supertest.CallbackHandler

#nowarn "40"

let tests () =
    describe "app.all()" (fun _ ->

        itAsync "should add a router per method" (fun d ->
            let app = Express.e.express ()

            app.all("/tobi", fun (req : Request) (res : Response) next ->
                res.``end``(req.``method``)
            )

            npm.supertest.supertest(app)
                .put("/tobi")
                .expect(
                    "PUT",
                    CallbackHandler (fun _ _ ->
                        npm.supertest.supertest(app)
                            .get("/tobi")
                            .expect(
                                "GET",
                                CallbackHandler (fun err _ ->
                                    d err
                                )
                            )
                            |> ignore
                    )
                )
                |> ignore
        )

        itAsync "should run the callback for a method just once" (fun d ->
            let app = Express.e.express ()
            let mutable n = 0

            app.all("/*", fun (req : Request) (res : Response) (next : NextFunction) ->
                if n > 0 then
                    d(System.Exception("DELETE called several times"))
                n <- n + 1
                next.Invoke()
            )

            npm.supertest.supertest(app)
                .del("/tobi")
                .expect(
                    404,
                    CallbackHandler (fun err _ ->
                        d err
                    )
                )
                |> ignore
        )

    )
