# Mime

Binding for [mime](https://www.npmjs.com/package/mime)

# Usage

```fs
open Mime

// You can create a custom Mime instance
let typeMap = createEmpty<Mime.TypeMap>
typeMap.["text/a"] <- ResizeArray(["a"; "a1"])
typeMap.["text/b"] <- ResizeArray(["b"; "b1"])

let myMime = mime.Mime(typeMap)

// You can also use the default settings
mime.getType("txt")
// Returns: Some "text/plain"

mime.getExtension("text/html")
// Returns: Some "html"
```
