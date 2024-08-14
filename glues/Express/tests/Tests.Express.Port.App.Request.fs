module Tests.Express.Port.App.Request

open Glutinum.Express
open Fable.Core.JsInterop
open Mocha
open Glutinum.SuperTest
open Fable.Core

// this ensures the two types are the same to asset that the is method below has the expected return type
let expectEqual (actual: 'T, expected: 'T) = 
  Assert.strictEqual(actual, expected)

let expectEqualNonStrict (actual: 'T, expected: 'T) = 
  Assert.notStrictEqual(actual, expected)

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

    describe ".is"(fun _ ->

        itAsync "should return null when body is missing" (fun d ->
            let app = express.express ()
            app.``use``(fun req res ->
                let json: U2<string option, bool> = req.is(!^"json")
                //NOTE: not testing strict equality here since the string option returns a null, whereas 
                // the runtime representation of None is undefined
                expectEqualNonStrict(json, !^None)
                //NOTE: converting this to a boolean, since providing the option to the send method
                // will result in `some` checking for null, and the response body being {}, 
                // which is indistinguishable from some other empty object
                let json = 
                    match json with 
                      | U2.Case1 (Some value) ->  false
                      | U2.Case1 None -> true
                      | U2.Case2 _ -> false
                res.``send``(json)
            )
            request(app)
                .get("/")
                .expect("true", d)
                |> ignore
        )

        itAsync "should return false when missing application/json content type" (fun d ->
            let app = express.express ()
            app.``use``(fun req res ->
                let json = req.is(!^"json")
                expectEqual(json, !^false)
                res.``send``(json)
            )
            let test =
              request(app)
                .post("/")
                .send "{}" 
                :?> SuperTest.Test
            test.expect("false", d)
                |> ignore
        )

        itAsync "should return 'json' when body and content type is present" (fun d ->
            let app = express.express ()
            app.``use``(fun req res ->
                let json = req.is(!^"json")
                expectEqual(json, !^(Some "json"))
                res.``send``(json)
            )
            let test = 
              request(app)
                .post("/")
                .set("Content-Type", "application/json")
                .send "{}"
                :?> SuperTest.Test
            test.expect("json", d)
                |> ignore
        )
    )
)
