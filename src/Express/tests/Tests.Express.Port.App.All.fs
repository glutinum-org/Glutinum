module Tests.Express.Port.App.All

open Mocha
open ExpressServeStaticCore

describe "app.all()" (fun _ ->

    itAsync "should add a router per method" (fun d ->
        let app = Express.e.express ()

        app.all("/tobi", fun (req : Request) (res : Response) next ->
            res.``end``(req.``method``)
        )

        request(app)
            .put("/tobi")
            .expect(
                "PUT",
                fun _ ->
                    request(app)
                        .get("/tobi")
                        .expect("GET", d)
                        |> ignore
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

        request(app)
            .del("/tobi")
            .expect(404, d)
            |> ignore
    )

)
