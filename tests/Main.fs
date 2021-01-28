namespace Tests

open Mocha
open Fable.Core.JsInterop
module Main =

    // Import should package into the project
    let should : obj = import "default" "should"

    describe "All" (fun _ ->
        Mime.tests ()
        Qs.tests ()
        RangeParser.tests ()
        Connect.tests ()
        BodyParser.All.tests ()
        ServeStatic.tests ()
        Express.All.tests ()
    )
