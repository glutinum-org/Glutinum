namespace Mime

open Npm
open Mocha
open Fable.Core
open Fable.Core.Testing
open Fable.Core.JsInterop

// Code adapted from: https://github.com/broofa/mime/blob/9847c9f9ee077a8d6e17d0b738b1b28c030a9a89/src/test.js

module Tests =

    let all () =
        describe "Mime" (fun _ ->

            it "Mime new constructor works" (fun _ ->
                let typeMap = createEmpty<Types.Mime.TypeMap>
                typeMap.["text/a"] <- ResizeArray(["a"; "a1"])
                typeMap.["text/b"] <- ResizeArray(["b"; "b1"])

                let mime = npm.mime.Mime(typeMap)

                Assert.AreEqual(mime.getType("a"), Some "text/a")
            )

            it "define works" (fun _ ->
                let typeMap = createEmpty<Types.Mime.TypeMap>
                typeMap.["text/a"] <- ResizeArray(["a"; "a1"])
                typeMap.["text/b"] <- ResizeArray(["b"; "b1"])

                let mime = npm.mime.Mime(typeMap)

                let subTypeMap = createEmpty<Types.Mime.TypeMap>
                subTypeMap.["text/c"] <- ResizeArray(["c"])

                mime.define(subTypeMap)

                Assert.AreEqual(mime.getType("c"), Some "text/c")
            )

            it "getType() works" (fun _ ->
                Assert.AreEqual(npm.mime.getType("txt"), Some "text/plain")
            )

            it "getExtension() works" (fun _ ->
                Assert.AreEqual(npm.mime.getExtension("text/html"), Some "html")
            )

        )
