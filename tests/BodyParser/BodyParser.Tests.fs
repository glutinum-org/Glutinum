module Tests.BodyParser.All

open Mocha
open Node
open Fable.Core
open Fable.Core.JsInterop
open BodyParser
open Connect

let private createServer opts =
    let bodyParser = bodyParser.json(opts)

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

let tests () =
    describe "BodyParser" (fun _ ->
        Json.tests ()
        Text.tests ()
    )
