# Glutinum.Express

Binding for [Express](https://github.com/expressjs/express)

# Usage

```fs
open Fable.Core
open Glutinum.Express
open Glutinum.ExpressServeStaticCore

let port = 2700
let app = express.express ()

app.get (
    "/",
    fun (req: Request) (res: Response) ->
        JS.console.log ($"New request, %s{req.hostname}")
        res.send ("Hello world")
)

app.listen (
    port,
    fun () -> JS.console.log $"Started listening on http://127.0.0.1:%i{port}"
)
|> ignore
```
