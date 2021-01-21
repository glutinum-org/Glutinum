namespace BodyParser

open Npm
open Mocha
open Node
open Fable.Core
open Fable.Core.JsInterop

module Tests =

    let all () =
        describe "BodyParser" (fun _ ->
            Tests.Json.all ()
            Tests.Text.all ()
        )
