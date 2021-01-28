module Tests.Express.App.Del

open Node
open Npm
open Mocha
open ExpressServeStaticCore

type CallbackHandler = Types.SuperTest.Supertest.CallbackHandler

#nowarn "40"

let tests () =
    describe "app.delete()" (fun _ ->

        itAsync "app.delete() works" (fun d ->
            let app = Express.e.express ()

            app.delete("/tobi", fun req (res : Response<_,_>) next ->
                res.``end``("deleted tobi")
            )

            npm.supertest.supertest(app)
                .del("/tobi")
                .expect(
                    "deleted tobi",
                    CallbackHandler (fun err _ ->
                        d err
                    )
                )
                |> ignore
        )

        itAsync "app.del() should alias app.delete()" (fun d ->
            let app = Express.e.express ()

            app.del("/tobi", fun req (res : Response<_,_>) next ->
                res.``end``("deleted tobi")
            )

            npm.supertest.supertest(app)
                .del("/tobi")
                .expect(
                    "deleted tobi",
                    CallbackHandler (fun err _ ->
                        d err
                    )
                )
                |> ignore
        )

    )
