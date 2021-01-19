module Tests

open Mocha

describe "All" (fun _ ->
    Mime.Tests.all ()
    Qs.Tests.all ()
    RangeParser.Tests.all ()
    Connect.Tests.all ()
)
