# RangeParser

Binding for [range-parser](https://www.npmjs.com/package/range-parser)

# Usage

```fs
open RangeParser

let range = rangeParser.rangeParser(1000, "bytes=0-499")

// The binding include a custom active pattern making it easier/safer to work with the library

match range with
| ParseRangeResult.UnkownError _
| ParseRangeResult.ResultInvalid
| ParseRangeResult.ResultUnsatisfiable ->
    // Handle error case

| ParseRangeResult.Range range ->
    // Here you can access your successful result
    range.``type`` // Returns: "bytes"
    range.Count // Returns: 1
    range.[0].start // Returns: 0
    range.[0].``end`` // Returns: 499
```
