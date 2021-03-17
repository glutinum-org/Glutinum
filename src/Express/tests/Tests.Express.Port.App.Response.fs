module Tests.Express.Port.App.Response

open ExpressServeStaticCore
open Fable.Core.JsInterop
open Mocha

describe "app" (fun _ ->
    describe ".response" (fun _ ->
        itAsync "should extend the response prototype" (fun d ->
            let app = Express.e.express()

            emitJsStatement app """
$0.response.shout = function(str){
this.send(str.toUpperCase());
};
"""

            app.``use``(fun req res ->
                res?shout("hey")
            )

            request(app)
                .get("/")
                .expect("HEY", d)
                |> ignore
        )

        itAsync "should not be influenced by other app protos" (fun d ->
            let app = Express.e.express()
            let app2 = Express.e.express()

            emitJsStatement app """
$0.response.shout = function(str){
this.send(str.toUpperCase());
};
"""

            emitJsStatement app2 """
$0.response.shout = function(str){
this.send(str);
};
"""

            app.``use``(fun req res ->
                res?shout("hey")
            )

            request(app)
                .get("/")
                .expect("HEY", d)
                |> ignore
        )
    )
)
