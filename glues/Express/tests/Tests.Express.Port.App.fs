module Tests.Express.Port.Express.App.App

open Mocha
open Glutinum.Express

describe "app" (fun _ ->

    itAsync "should inherit from event emitter" (fun d ->
        let app = express.express ()

        app.on("foo", fun _ -> d()) |> ignore
        app.emit("foo") |> ignore
    )

    itAsync "should be callable" (fun d ->
        request(express.express())
            .get("/")
            .expect(404, d)
            |> ignore
    )

)
