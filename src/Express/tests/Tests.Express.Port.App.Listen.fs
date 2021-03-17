module Tests.Express.Port.App.Listen

open Node
open Mocha
open Glutinum.ExpressServeStaticCore
open Glutinum.Express

#nowarn "40"

describe "app.listen()" (fun _ ->

    itAsync "should wrap with an HTTP server" (fun d ->
        let app = express.express ()

        app.del("/tobi", fun req (res : Response<_,_>) next ->
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
