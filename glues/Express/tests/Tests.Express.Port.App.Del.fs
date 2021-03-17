module Tests.Express.Port.App.Del

open Mocha
open Glutinum.ExpressServeStaticCore
open Glutinum.Express


describe "app.delete()" (fun _ ->

    itAsync "app.delete() works" (fun d ->
        let app = express.express ()

        app.delete("/tobi", fun req (res : Response<_,_>) next ->
            res.``end``("deleted tobi")
        )

        request(app)
            .del("/tobi")
            .expect("deleted tobi", d)
            |> ignore
    )

    itAsync "app.del() should alias app.delete()" (fun d ->
        let app = express.express ()

        app.del("/tobi", fun req (res : Response<_,_>) next ->
            res.``end``("deleted tobi")
        )

        request(app)
            .del("/tobi")
            .expect("deleted tobi", d)
            |> ignore
    )

)
