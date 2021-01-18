namespace Mime

open Fable.Mocha
open Mime
open Fable.Core
open Fable.Core.JsInterop

module Tests =

    let all =
        testList "Mime" [

            testCase "Mime new constructor works" (fun _ ->
                let typeMap = createEmpty<Types.TypeMap>
                typeMap.["text/a"] <- ResizeArray(["a"; "a1"])
                typeMap.["text/b"] <- ResizeArray(["b"; "b1"])

                let mime = Mime.Create(typeMap)

                Expect.equal (mime.getType("a")) (Some "text/a") ""
            )

            testCase "define works" (fun _ ->
                let typeMap = createEmpty<Types.TypeMap>
                typeMap.["text/a"] <- ResizeArray(["a"; "a1"])
                typeMap.["text/b"] <- ResizeArray(["b"; "b1"])

                let mime = Mime.Create(typeMap)

                let subTypeMap = createEmpty<Types.TypeMap>
                subTypeMap.["text/c"] <- ResizeArray(["c"])

                mime.define(subTypeMap)

                Expect.equal (mime.getType("c")) (Some "text/c") ""
            )

            testCase "getType() works" (fun _ ->
                Expect.equal (mime.getType("txt")) (Some "text/plain") ""
            )

            testCase "getExtension() works" (fun _ ->
                Expect.equal (mime.getExtension("text/html")) (Some "html") ""
            )

        ]
