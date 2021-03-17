module Tests.Express.Port.App.Request

open Glutinum.ExpressServeStaticCore
open Glutinum.Express
open Fable.Core.JsInterop
open Mocha

describe "app" (fun _ ->
    describe ".request" (fun _ ->
        itAsync "should extend the request prototype" (fun d ->
            let app = express.express ()

            emitJsStatement app """
$0.request.querystring = function(){
return require('url').parse(this.url).query;
};
"""

            app.``use``(fun req res ->
                res.``end``(req?querystring())
            )

            request(app)
                .get("/foo?name=tobi")
                .expect("name=tobi", d)
                |> ignore
        )
    )
)
