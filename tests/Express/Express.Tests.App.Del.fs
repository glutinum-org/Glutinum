module Tests.Express.App.Del

open Mocha
open ExpressServeStaticCore


describe "app.delete()" (fun _ ->

    itAsync "app.delete() works" (fun d ->
        let app = Express.e.express ()

        app.delete("/tobi", fun req (res : Response<_,_>) next ->
            res.``end``("deleted tobi")
        )

        request(app)
            .del("/tobi")
            .expect("deleted tobi", d)
            |> ignore
    )

    itAsync "app.del() should alias app.delete()" (fun d ->
        let app = Express.e.express ()

        app.del("/tobi", fun req (res : Response<_,_>) next ->
            res.``end``("deleted tobi")
        )

        request(app)
            .del("/tobi")
            .expect("deleted tobi", d)
            |> ignore
    )

)
