module Tests.Express.App.Route

open ExpressServeStaticCore
open Fable.Core.JsInterop
open Npm
open Mocha
open Node

let tests () =
    describe "app.route" (fun _ ->
        itAsync "should return a new route" (fun d ->
            let app = Express.e.express()
            
            app.route("/foo")
                .get(fun req res ->
                    res.send("get")
                ) 
                .post(fun req res ->
                    res.send("post") 
                )
                |> ignore
            
            npm.supertest.supertest(app)
                .post("/foo")
                .expect(
                    "post",
                    fun err _ -> d err
                )
                |> ignore
        )
        
        itAsync "should all .VERB after .all" (fun d ->
            let app = Express.e.express()
            
            app.route("/foo")
                .all(fun req res next ->
                    next.Invoke()
                )
                .get(fun req res ->
                    res.send("get")
                )
                .post(fun req res ->
                    res.send("post")
                )
                |> ignore
            
            npm.supertest.supertest(app)
                .post("/foo")
                .expect(
                    "post",
                    fun err _ -> d err
                )
                |> ignore
        )
        
        itAsync "should support dynamic routes" (fun d ->
            let app = Express.e.express()
            
            app.route("/:foo")
                .get(fun req res ->
                    res.send(req.``params``.["foo"])
                )
                |> ignore
            
            npm.supertest.supertest(app)
                .get("/test")
                .expect(
                    "test",
                    fun err _ -> d err
                )
                |> ignore
        )
        
        itAsync "should not error on empty routes" (fun d ->
            let app = Express.e.express()
            
            app.route("/:foo") |> ignore
            
            npm.supertest.supertest(app)
                .get("/test")
                .expect(
                    404,
                    fun err _ -> d err
                )
                |> ignore
        )
        
    )
