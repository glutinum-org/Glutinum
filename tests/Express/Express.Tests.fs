module Tests.Express.All

open Mocha


let tests () =
    describe "Express" (fun _ ->
        App.Core.tests ()
        App.Listen.tests ()
        App.Del.tests ()
        App.All.tests ()
        App.Engine.tests ()
        App.Head.tests ()
        App.Locals.tests ()
        App.Options.tests ()
    )
