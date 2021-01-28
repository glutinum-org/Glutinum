module Tests.Express.App.Listen

open Node
open Npm
open Mocha
open ExpressServeStaticCore

#nowarn "40"

let tests () =
    describe "app.listen()" (fun _ ->

        itAsync "should wrap with an HTTP server" (fun d ->
            let app = Express.e.express ()

            app.delete("/tobi", fun req (res : Response<_,_>) next ->
                res.``end``("deleted tobi")
            )

            let rec server : Http.Server  =
                app.listen(9999, fun _ ->
                    server.close() |> ignore
                    d()
                )

            ()
        )

    )
