module Tests

open Fable.Mocha

let allTests =
    testList "All" [
        Mime.Tests.all
        Qs.Tests.all
        RangeParser.Tests.all
    ]

[<EntryPoint>]
let main _ =
    Mocha.runTests allTests
