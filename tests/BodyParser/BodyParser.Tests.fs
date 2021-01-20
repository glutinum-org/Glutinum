namespace BodyParser

open Npm
open Mocha
open Fable.Core
open Fable.Core.Testing
open Fable.Core.JsInterop
open Node
open BodyParser
open SuperTest

module Tests =

    let all () =
        describe "BodyParser" (fun _ ->

            describe "bodyParser()" (fun _ ->

                it "test" (fun _ ->

                    let x = npm.BodyParser.text()

                    let y = npm.BodyParser.json()


                    ()

                )

            )

        )
