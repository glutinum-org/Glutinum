# Connect

Binding for [connect](https://www.npmjs.com/package/connect)

# Usage

```fs
open Connect
let app = connect()

// We need to help the compiler with a type hint
app.``use``(fun req res ->
    res.``end``("Hello, world!")
) |> ignore
```
