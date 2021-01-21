module Tests

open Mocha

describe "All" (fun _ ->
    Mime.Tests.tests ()
    Qs.Tests.tests ()
    RangeParser.Tests.tests ()
    Connect.Tests.tests ()
    BodyParser.Tests.All.tests ()
    ServeStatic.Tests.tests()
)
