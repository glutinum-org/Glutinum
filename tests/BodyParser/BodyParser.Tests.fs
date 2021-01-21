namespace BodyParser.Tests

open Npm
open Mocha
open Node
open Fable.Core
open Fable.Core.JsInterop

module All =

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
        describe "BodyParser" (fun _ ->
            JsonTests.tests ()
            TextTests.tests ()
        )
