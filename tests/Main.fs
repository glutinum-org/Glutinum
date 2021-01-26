namespace Tests

open Mocha

module Main =

    describe "All" (fun _ ->
        Mime.tests ()
        Qs.tests ()
        RangeParser.tests ()
        Connect.tests ()
        BodyParser.All.tests ()
        ServeStatic.tests ()
        Express.All.tests ()
    )
