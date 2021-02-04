module Tests.Express.App.Router

open ExpressServeStaticCore
open Fable.Core.JsInterop
open Npm
open Mocha

let tests () =
    describe "app.router" (fun _ ->
        itAsync "should restore req.params after leaving router" (fun d ->
            let app = Express.e.express()
            let router = Express.e.Router()
            
            let handler1 =
                fun (req : Request) (res : Response) (next : NextFunction)  ->
                    res.setHeader("x-user-id", req.``params``.["id"])
                    next.Invoke()
                |> Adapter.RequestHandler
            
            let handler2 =
                fun (req : Request) (res : Response) ->
                    res.send(req.``params``.["id"])
                |> Adapter.RequestHandler 
 
            router.``use``(fun req res next ->
                res.setHeader("x-router", emitJsExpr (req.``params``.["id"]) "String($0)")
                next.Invoke()
            )
                  
            app.get_(
                "/user/:id",
                handler1, 
                router :> RequestHandler,
                handler2
            )
               
            npm.supertest.supertest(app)
                .get("/user/1")
                .expect("x-router", "undefined")
                .expect("x-user-id", "1")
                .expect(
                    200,
                    "1",
                    fun err _ -> d err
                )
                |> ignore
        )
        
        describe "methods" (fun _ ->
            
            Methods.methods.ToArray()
            |> Array.append [| unbox "del" |]
            |> Array.iter (fun method ->
                if method = Methods.Method.Connect then
                    () // Do nothing
                else                    
                    itAsync ("should include " + (string method).ToUpper()) (fun d ->
                        let app = Express.e.express()
                        
                        app?(method)$("/foo", fun (_ : Request) (res : Response) ->
                            res.send(method)
                        )
                        
                        emitJsStatement (app, npm.supertest.supertest, method, d) """
$1($0)
    [$2]('/foo')
    .expect(200, $3)
"""
                    )
                    
                    it ("should reject numbers for app." + (string method)) (fun _ ->
                        let app = Express.e.express()
                        
                        Assert.throws(app?(method)?bind$(app, "/", 3)
                    )
                )
            )
            
            
            itAsync "should re-route when method is altered" (fun d ->
                let app = Express.e.express()
                let cb = After.e.after(3, d)
               
                 
                app.``use``(fun req res next ->
                    if req.method <> "POST" then
                        next.Invoke()
                    else
                        req.method <- "DELETE"
                        res.setHeader("X-Method-Altered", "1")
                        next.Invoke()
                )
                
                app.del("/", fun (req : Request) (res : Response) ->
                    res.``end``("deleted everything")
                )
                
                npm.supertest.supertest(app)
                    .get("/")
                    .expect(
                        404,
                        cb
                    )
                    |> ignore
                    
                npm.supertest.supertest(app)
                    .del("/")
                    .expect(
                        200,
                        "deleted everything",
                        cb
                    )
                    |> ignore 
                
                npm.supertest.supertest(app)
                    .post("/")
                    .expect("X-Method-Altered", "1")
                    .expect(
                        200,
                        "deleted everything",
                        cb
                    )
                    |> ignore 
            )            
        )
    )
