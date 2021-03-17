# BodyParser

Binding for [body-parser](https://www.npmjs.com/package/body-parser)

# Usage

```fs
open BodyParser

let bodyParser =
    let options =
        jsOptions<BodyParser.OptionsJson>(fun o ->
            o.limit <- !^ "1kb"
        )

    bodyParser.json(options)

// If used with Express
open Express

let app = Express.e.express()
app.``use``(bodyParser)
```
