module Tests.BodyParser.Json

open Npm
open Mocha
open Node
open Fable.Core
open Fable.Core.JsInterop

// Code adapted from: https://github.com/expressjs/body-parser/blob/480b1cfe29af19c070f4ae96e0d598c099f42a12/test/json.js


let private createServer opts =
    let bodyParser = npm.bodyParser.json(opts)

    http.createServer(fun req res ->
        let req = req :?> Types.Connect.CreateServer.IncomingMessage

        let next =
            Types.Connect.CreateServer.NextFunction(fun error ->
                    res.statusCode <-
                        if isNull error then
                            200
                        else
                            if isNull error?status then
                                500
                            else
                                error?status |> unbox<int>
                    if isNull error then
                        res.``end``(JS.JSON.stringify(req?body))
                    else
                        res.``end``(error?message)
            )

        bodyParser.Invoke(req, res, next)
    )


let tests () =
    describe "bodyParser.json()" (fun _ ->

        itAsync "should default to {}" (fun ok ->
            request(box (createServer null))
                .post("/")
                .expect(200, "{}", fun err _ -> ok err)
                |> ignore
        )

        itAsync "should parse JSON" (fun ok ->
            let test =
                request(box (createServer null))
                    .post("/")
                    .set("Content-Type", "application/json")
                    .send("""{"user":"tobi"}""")
                    :?> Types.SuperTest.Supertest.Test

            test.expect(
                200,
                """{"user":"tobi"}""",
                fun err _ -> ok err
            )
            |> ignore
        )

        describe "with limit option" (fun _ ->

            itAsync "should 413 when over limit with Content-Length" (fun d ->
                let buf = buffer.Buffer.alloc(1024, ".")

                let server =
                    createServer(jsOptions<Types.BodyParser.OptionsJson>(fun o ->
                        o.limit <- !^ "1kb"
                    ))

                let test =
                    request(box server)
                        .post("/")
                        .set("Content-Type", "application/json")
                        .set("Content-Length", "1034")
                        .send(JS.JSON.stringify(
                                createObj [
                                    "str" ==> buf.toString()
                                ]
                            )
                        )
                        :?> Types.SuperTest.Supertest.Test

                test.expect(
                    413,
                    Types.SuperTest.Supertest.CallbackHandler (fun err _ -> d err)
                )
                |> ignore
            )
        )
    )
