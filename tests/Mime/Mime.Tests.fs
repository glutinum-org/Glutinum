namespace Mime

open Npm
open Mocha
open Mime
open Fable.Core
open Fable.Core.Testing
open Fable.Core.JsInterop

module Tests =

    let all () =
        describe "Mime" (fun _ ->

            it "Mime new constructor works" (fun _ ->
                let typeMap = createEmpty<Types.TypeMap>
                typeMap.["text/a"] <- ResizeArray(["a"; "a1"])
                typeMap.["text/b"] <- ResizeArray(["b"; "b1"])

                let mime = npm.mime.Mime(typeMap)

                Assert.AreEqual(mime.getType("a"), Some "text/a")
            )

            it "define works" (fun _ ->
                let typeMap = createEmpty<Types.TypeMap>
                typeMap.["text/a"] <- ResizeArray(["a"; "a1"])
                typeMap.["text/b"] <- ResizeArray(["b"; "b1"])

                let mime = npm.mime.Mime(typeMap)

                let subTypeMap = createEmpty<Types.TypeMap>
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
