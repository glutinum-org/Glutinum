# Qs

Binding for [qs](https://www.npmjs.com/package/qs)

# Usage

```fs
open Qs

let res = qs.parse("0=foo")

// You can access the value store at index 0 like that
res.[0]
// Return: Some (U4.Case "foo")

// You can also pass options to the 'parse' function
qs.parse(
    "a[0]=b&a[1]=c",
    jsOptions<Qs.IParseOptions>(fun o ->
        o.arrayFormat <- Qs.IArrayFormat.Brackets
    )
)
```
