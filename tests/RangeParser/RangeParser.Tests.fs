namespace RangeParser

open Mocha
open RangeParser
open Fable.Core
open Fable.Core.Testing
open Fable.Core.JsInterop

module Tests =

    let all () =
        describe "RangeParser" (fun _ ->

            describe "parseRange(len, str)" (fun _ ->

                itAsync "should return -2 (aka ResultInvalid) for invalid str" (fun ok ->
                    let range = parseRange.Invoke(200, "malformed")

                    match range with
                    | ParseRangeResult.ResultInvalid ->
                        ok()

                    | ParseRangeResult.UnkownError _
                    | ParseRangeResult.ResultUnsatisfiable
                    | ParseRangeResult.Range _ ->
                        failwith "Should not happen"
                )

                itAsync "should return -1 if all specified ranges are invalid" (fun ok ->
                    let range = parseRange.Invoke(200, "bytes=500-20")

                    match range with
                    | ParseRangeResult.ResultUnsatisfiable ->
                        ok()

                    | ParseRangeResult.UnkownError _
                    | ParseRangeResult.ResultInvalid
                    | ParseRangeResult.Range _ ->
                        failwith "Should not happen"
                )

                it "should parse str" (fun _ ->
                    let range = parseRange.Invoke(1000, "bytes=0-499")

                    match range with
                    | ParseRangeResult.UnkownError _
                    | ParseRangeResult.ResultInvalid
                    | ParseRangeResult.ResultUnsatisfiable ->
                        failwith "Should not happen"

                    | ParseRangeResult.Range range ->
                        // Here you can access your successful result
                        Assert.AreEqual(range.``type``, "bytes")
                        Assert.AreEqual(range.Count, 1)
                        Assert.AreEqual(range.[0].start, 0)
                        Assert.AreEqual(range.[0].``end``, 499)
                )

            )

            describe "when combine: true" (fun _ ->

                it "should combine overlapping ranges" (fun _ ->
                    let range = parseRange.Invoke(
                                    150,
                                    "bytes=0-4,90-99,5-75,100-199,101-102",
                                    jsOptions<Types.Options>(fun o ->
                                        o.combine <- true
                                    )
                                )

                    match range with
                    | ParseRangeResult.UnkownError _
                    | ParseRangeResult.ResultInvalid
                    | ParseRangeResult.ResultUnsatisfiable ->
                        failwith "Should not happen"

                    | ParseRangeResult.Range range ->
                        // Here you can access your successful result
                        Assert.AreEqual(range.``type``, "bytes")
                        Assert.AreEqual(range.Count, 2)
                        Assert.AreEqual(range.[0].start, 0)
                        Assert.AreEqual(range.[0].``end``, 75 )
                        Assert.AreEqual(range.[1].start, 90)
                        Assert.AreEqual(range.[1].``end``, 149)
                )

                it "overload parseRange.Invoke with direct combine argument works" (fun _ ->
                    let range =
                        parseRange.Invoke(
                            150,
                            "bytes=0-4,90-99,5-75,100-199,101-102",
                            true
                        )

                    match range with
                    | ParseRangeResult.UnkownError _
                    | ParseRangeResult.ResultInvalid
                    | ParseRangeResult.ResultUnsatisfiable ->
                        failwith "Should not happen"

                    | ParseRangeResult.Range range ->
                        // Here you can access your successful result
                        Assert.AreEqual(range.``type``, "bytes")
                        Assert.AreEqual(range.Count, 2)
                        Assert.AreEqual(range.[0].start, 0)
                        Assert.AreEqual(range.[0].``end``, 75 )
                        Assert.AreEqual(range.[1].start, 90)
                        Assert.AreEqual(range.[1].``end``, 149)
                )

            )

        )
