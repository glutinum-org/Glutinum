namespace BodyParser

open Npm
open Mocha

module Tests =

    let all () =
        describe "BodyParser" (fun _ ->

            describe "bodyParser()" (fun _ ->

                it "test" (fun _ ->

                    let x = npm.bodyParser.text()

                    let y = npm.bodyParser.json()


                    ()

                )

            )

        )
