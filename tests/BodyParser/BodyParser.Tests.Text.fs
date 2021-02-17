module Tests.BodyParser.Text

open Mocha
open Node
open Fable.Core
open Fable.Core.JsInterop
open BodyParser
open Connect
open SuperTest

// Code adapted from: https://github.com/expressjs/body-parser/blob/480b1cfe29af19c070f4ae96e0d598c099f42a12/test/text.js

let private createServer (opts : BodyParser.OptionsText) =
    let bodyParser = bodyParser.text(opts)

    http.createServer(fun req res ->
        let req = req :?> Connect.CreateServer.IncomingMessage

        let next =
            Connect.CreateServer.NextFunction(fun error ->
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


let tests =
    describe "bodyParser.text()" (fun _ ->

        itAsync "should parse text/plain" (fun d ->
            let test =
                request(box (createServer null))
                    .post("/")
                    .set("Content-Type", "text/plain")
                    .send("user is tobi")
                    :?> SuperTest.Test

            test.expect(200, "\"user is tobi\"", d)
            |> ignore
        )

        describe "with limit option" (fun _ ->

            itAsync "should 413 when over limit with Content-Length" (fun d ->
                let buf = buffer.Buffer.alloc(1028, ".")

                let server =
                    createServer(jsOptions<BodyParser.OptionsText>(fun o ->
                        o.limit <- !^ "1kb"
                    ))

                let test =
                    request(box server)
                        .post("/")
                        .set("Content-Type", "text/plain")
                        .set("Content-Length", "1028")
                        .send(buf.toString())
                        :?> SuperTest.Test

                test.expect(413, d)
                |> ignore
            )
        )
    )

