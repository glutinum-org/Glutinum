namespace RangeParser

open Fable.Mocha
open RangeParser
open Fable.Core
open Fable.Core.JsInterop

module Tests =

    let all =
        testList "RangeParser" [

            testList "parseRange(len, str)" [

                testCase "should return -2 (aka ResultInvalid) for invalid str" (fun _ ->
                    let range = parseRange.Invoke(200, "malformed")

                    match range with
                    | ParseRangeResult.ResultInvalid ->
                        Expect.pass () ""

                    | ParseRangeResult.UnkownError _
                    | ParseRangeResult.ResultUnsatisfiable
                    | ParseRangeResult.Range _ ->
                        failwith "Should not happen"
                )

                testCase "should return -1 if all specified ranges are invalid" (fun _ ->
                    let range = parseRange.Invoke(200, "bytes=500-20")

                    match range with
                    | ParseRangeResult.ResultUnsatisfiable ->
                        Expect.pass () ""

                    | ParseRangeResult.UnkownError _
                    | ParseRangeResult.ResultInvalid
                    | ParseRangeResult.Range _ ->
                        failwith "Should not happen"
                )

                testCase "should parse str" (fun _ ->
                    let range = parseRange.Invoke(1000, "bytes=0-499")

                    match range with
                    | ParseRangeResult.UnkownError _
                    | ParseRangeResult.ResultInvalid
                    | ParseRangeResult.ResultUnsatisfiable ->
                        failwith "Should not happen"

                    | ParseRangeResult.Range range ->
                        // Here you can access your successful result
                        Expect.equal range.``type`` "bytes" ""
                        Expect.equal range.Count 1 ""
                        Expect.equal range.[0].start 0 ""
                        Expect.equal range.[0].``end`` 499 ""
                )

            ]

            testList "when combine: true" [

                testCase "should combine overlapping ranges" (fun _ ->
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
                        Expect.equal range.``type`` "bytes" ""
                        Expect.equal range.Count 2 ""
                        Expect.equal range.[0].start 0 ""
                        Expect.equal range.[0].``end`` 75  ""
                        Expect.equal range.[1].start 90 ""
                        Expect.equal range.[1].``end`` 149 ""
                )

                testCase "overload parseRange.Invoke with direct combine argument works" (fun _ ->
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
                        Expect.equal range.``type`` "bytes" ""
                        Expect.equal range.Count 2 ""
                        Expect.equal range.[0].start 0 ""
                        Expect.equal range.[0].``end`` 75  ""
                        Expect.equal range.[1].start 90 ""
                        Expect.equal range.[1].``end`` 149 ""
                )

            ]

        ]
