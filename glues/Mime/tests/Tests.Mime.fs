module Tests.Mime

open Mocha
open Fable.Core
open Fable.Core.Testing
open Fable.Core.JsInterop
open Glutinum.Mime

// Code adapted from: https://github.com/broofa/mime/blob/9847c9f9ee077a8d6e17d0b738b1b28c030a9a89/src/test.js


describe "Mime" (fun _ ->

    it "Mime new constructor works" (fun _ ->
        let typeMap = createEmpty<Mime.TypeMap>
        typeMap.["text/a"] <- ResizeArray(["a"; "a1"])
        typeMap.["text/b"] <- ResizeArray(["b"; "b1"])

        let mime = mime.Mime(typeMap)

        Assert.AreEqual(mime.getType("a"), Some "text/a")
    )

    it "define works" (fun _ ->
        let typeMap = createEmpty<Mime.TypeMap>
        typeMap.["text/a"] <- ResizeArray(["a"; "a1"])
        typeMap.["text/b"] <- ResizeArray(["b"; "b1"])

        let mime = mime.Mime(typeMap)

        let subTypeMap = createEmpty<Mime.TypeMap>
        subTypeMap.["text/c"] <- ResizeArray(["c"])

        mime.define(subTypeMap)

        Assert.AreEqual(mime.getType("c"), Some "text/c")
    )

    it "getType() works" (fun _ ->
        Assert.AreEqual(mime.getType("txt"), Some "text/plain")
    )

    it "getExtension() works" (fun _ ->
        Assert.AreEqual(mime.getExtension("text/html"), Some "html")
    )

)
