module Tests.ServeStatic

open Mocha
open Fable.Core.Testing
open Fable.Core.JsInterop
open ServeStatic
open Node
open Connect

// Code adapted from: https://github.com/expressjs/serve-static/blob/94feedb81682f4503ed9f8dc6d51a5c1b9bfa091/test/test.js

let private createServer opts =
    let dir = path.join(__dirname, "files")

    let serve = serveStatic.serveStatic(dir, opts)

    http.createServer(fun req res ->
        // let req = req :?> Types.Connect.CreateServer.IncomingMessage

        let next =
            Connect.CreateServer.NextFunction(fun error ->
                res.statusCode <-
                    if isNull error then
                        404
                    else
                        if isNull error?status then
                            500
                        else
                            error?status |> unbox<int>
                if isNull error then
                    res.``end``("sorry!")
                else
                    res.``end``(error?``stack``)
            )

        serve.Invoke(req, res, next)
        |> ignore

    )


describe "ServeStatic" (fun _ ->

    itAsync "should support nesting" (fun d ->
        request(box (createServer null))
            .get("/users/tobi.txt")
            .expect(200, "ferret", d)
            |> ignore
    )

    itAsync "should support index.html" (fun d ->
        request(box (createServer null))
            .get("/users/")
            .expect(200, "<p>tobi, loki, jane</p>", d)
            |> ignore
    )

)
