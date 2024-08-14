module Tests.Express.Port.App.Request

open Glutinum.ExpressServeStaticCore
open Glutinum.Express
open Fable.Core.JsInterop
open Mocha

let url : obj = import "*" "url"

describe "app" (fun _ ->
    describe ".request" (fun _ ->
        itAsync "should extend the request prototype" (fun d ->
            let app = express.express ()

            emitJsStatement (app, url) """
$0.request.querystring = function(){
return $1.parse(this.url).query;
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
