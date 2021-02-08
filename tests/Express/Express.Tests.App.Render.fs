module Tests.Express.App.Render

open System
open ExpressServeStaticCore
open ExpressServeStaticCore
open Fable.Core
open Fable.Core.JS
open Fable.Core.JsInterop
open Npm
open Mocha
open ExpressServeStaticCore
open Node

let tmpl (_ : string) (_ : obj) _ = import "default" "./support/tmpl.js"

let createApp() =
  let app = Express.e.express()
  app.engine(".tmpl", tmpl);
  app

let tests () =
    describe "app" (fun _ ->
        describe ".render(name, fn)" (fun _ ->

            itAsync "should support absolute paths" (fun d ->
                let app = createApp()

                app.locals.["user"] <- {| name = "tobi" |}

                app.render(path.join(__dirname, "fixtures", "user.tmpl"), fun err str ->
                    if err.IsSome then
                        d err
                    else
                        Assert.strictEqual(str, "<p>tobi</p>")
                        d ()
                )
            )

            itAsync "should support absolute paths with 'view engine'" (fun d ->
                let app = createApp()

                app.set("view engine", "tmpl")
                app.locals.["user"] <- {| name = "tobi" |}

                app.render(path.join(__dirname, "fixtures", "user"), fun err str ->
                    if err.IsSome then
                        d err
                    else
                        Assert.strictEqual(str, "<p>tobi</p>")
                        d ()
                )
            )

            itAsync "should expose app.locals" (fun d ->
                let app = createApp()

                app.set("views", path.join(__dirname, "fixtures"))
                app.locals.["user"] <- {| name = "tobi" |}

                app.render("user.tmpl", fun err str ->
                    if err.IsSome then
                        d err
                    else
                        Assert.strictEqual(str, "<p>tobi</p>")
                        d ()
                )
            )

            itAsync "should support index.<engine>" (fun d ->
                let app = createApp()

                app.set("views", path.join(__dirname, "fixtures"))
                app.set("view engine", "tmpl")

                app.render("blog/post", fun err str ->
                    if err.IsSome then
                        d err
                    else
                        Assert.strictEqual(str, "<h1>blog post</h1>")
                        d ()
                )
            )

            itAsync "should handle render error throws" (fun d ->
                let app = Express.e.express()

                emitJsStatement () """
function View(name, options){
    this.name = name;
    this.path = 'fale';
}

View.prototype.render = function(options, fn){
    throw new Error('err!');
};
"""

                emitJsStatement app """
$0.set('view', View);
"""

                app.render("something", fun err str ->
                    Assert.ok(box err.IsSome)
                    Assert.strictEqual(err.Value.Message, "err!")
                    d()
                )
            )

            describe "when the file does not exist" (fun _ ->
                itAsync "should provide a helpful error" (fun d ->
                    let app = createApp()

                    app.set("views", path.join(__dirname, "fixtures"))

                    app.render("rawr.tmpl", fun err _ ->
                        Assert.ok(err)
                        Assert.strictEqual(err.Value.Message, "Failed to lookup view \"rawr.tmpl\" in views directory \"" + path.join(__dirname, "fixtures") + "\"")
                        d()
                    )
                )
            )

            describe "when an error occurs" (fun _ ->
                itAsync "should invoke the callback" (fun d ->
                    let app = createApp()

                    app.set("views", path.join(__dirname, "fixtures"))

                    app.render("user.tmpl", fun err _ ->
                        Assert.ok(err)
                        Assert.strictEqual(err.Value?name, "RenderError")
                        d ()
                    )
                )
            )

            describe "when an extension is given" (fun _ ->
                itAsync "should render the template" (fun d ->
                    let app = createApp()

                    app.set("views", path.join(__dirname, "fixtures"))

                    app.render("email.tmpl", fun err str ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(str, "<p>This is an email</p>")
                            d ()
                    )
                )
            )

            describe "when 'view engine' is given" (fun _ ->
                itAsync "should render the template" (fun d ->
                    let app = createApp()

                    app.set("view engine", "tmpl")
                    app.set("views", path.join(__dirname, "fixtures"))

                    app.render("email", fun err str ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(str, "<p>This is an email</p>")
                            d ()
                    )
                )
            )

            describe "when 'views' is given" (fun _ ->
                itAsync "should lookup the file in the path" (fun d ->
                    let app = createApp()

                    app.set("views", path.join(__dirname, "fixtures", "default_layout"))
                    app.locals.["user"] <- {| name = "tobi" |}

                    app.render("user.tmpl", fun err str ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(str, "<p>tobi</p>")
                            d ()
                    )

                )

                describe "when array of paths" (fun _ ->
                    itAsync "should lookup the file in the path" (fun d ->
                        let app = createApp()
                        let views =
                            [|
                                path.join(__dirname, "fixtures", "local_layout")
                                path.join(__dirname, "fixtures", "default_layout")
                            |]

                        app.set("views", views)
                        app.locals.["user"] <- {| name = "tobi" |}

                        app.render("user.tmpl", fun err str ->
                            if err.IsSome then
                                d err
                            else
                                Assert.strictEqual(str, "<span>tobi</span>")
                                d ()
                        )
                    )

                    itAsync "should lookup in later paths until found" (fun d ->
                        let app = createApp()
                        let views =
                            [|
                                path.join(__dirname, "fixtures", "local_layout")
                                path.join(__dirname, "fixtures", "default_layout")
                            |]

                        app.set("views", views)
                        app.locals.["name"] <- "tobi"

                        app.render("name.tmpl", fun err str ->
                            if err.IsSome then
                                d err
                            else
                                Assert.strictEqual(str, "<p>tobi</p>")
                                d ()
                        )
                    )

                    itAsync "should error if file does not exist" (fun d ->
                        let app = createApp()
                        let views =
                            [|
                                path.join(__dirname, "fixtures", "local_layout")
                                path.join(__dirname, "fixtures", "default_layout")
                            |]

                        app.set("views", views)
                        app.locals.["name"] <- "tobi"

                        app.render("pet.tmpl", fun err str ->
                            Assert.ok(err)
                            Assert.strictEqual(err.Value.Message, "Failed to lookup view \"pet.tmpl\" in views directories \"" + views.[0] + "\" or \"" + views.[1] + "\"")
                            d()
                        )
                    )
                )
            )

            describe "caching" (fun _ ->
                itAsync "should always lookup view without cache" (fun d ->
                    let app = Express.e.express()
                    let mutable count = 0


                    emitJsStatement () """
function View(name, options){
    this.name = name;
    this.path = 'fake';
    count++;
}

View.prototype.render = function(options, fn){
    fn(null, 'abstract engine');
};
"""

                    app.set("view cache", false)
                    app.set("view", emitJsExpr () "View")

                    app.render("something", fun err str ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(count, 1)
                            Assert.strictEqual(str, "abstract engine")
                            app.render("something", fun err str ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 2)
                                    Assert.strictEqual(str, "abstract engine")
                                    d()
                            )
                    )
                )

                itAsync "should cache with 'view cache' setting" (fun d ->
                    let app = Express.e.express()
                    let mutable count = 0 // Force a "unique" name because we emit raw JavaScript

                    emitJsStatement count """
function View(name, options){
    this.name = name;
    this.path = 'fake';
    $0++;
}

View.prototype.render = function(options, fn){
    fn(null, 'abstract engine');
};
"""

                    app.set("view cache", true)
                    app.set("view", emitJsExpr () "View")

                    app.render("something", fun err str ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(count, 1)
                            Assert.strictEqual(str, "abstract engine")
                            app.render("something", fun err str ->
                                if err.IsSome then
                                    d err
                                else
                                    Assert.strictEqual(count, 1)
                                    Assert.strictEqual(str, "abstract engine")
                                    d ()
                            )
                    )
                )
            )

            describe ".render(name, options, fn)" (fun _ ->
                itAsync "should render the template" (fun d ->
                    let app = createApp()
                    app.set("views", path.join(__dirname, "fixtures"))

                    let user = {| name = "tobi" |}

                    app.render("user.tmpl", {| user = user |}, fun err str ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(str, "<p>tobi</p>")
                            d ()
                    )
                )

                itAsync "should expose app.locals" (fun d ->
                    let app = createApp()

                    app.set("views", path.join(__dirname, "fixtures"))
                    app.locals.["user"] <- {| name = "tobi" |}

                    app.render("user.tmpl", createEmpty, fun err str ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(str, "<p>tobi</p>")
                            d ()
                    )
                )

                itAsync "should give precedence to app.render() locals" (fun d ->
                    let app = createApp()

                    app.set("views", path.join(__dirname, "fixtures"))
                    app.locals.["user"] <- {| name = "tobi" |}
                    let jane = {| name = "jane" |}

                    app.render("user.tmpl", {| user = jane |}, fun err str ->
                        if err.IsSome then
                            d err
                        else
                            Assert.strictEqual(str, "<p>jane</p>")
                            d ()
                    )
                )

                describe "caching" (fun _ ->
                    itAsync "should cache with cache option" (fun d ->
                        let app = Express.e.express()
                        let mutable count = 0 // Force a "unique" name because we emit raw JavaScript

                        // Abuse emit helpers to mimic the JavaScript code as Fable emit flat arrows functions
                        // which don't have a prototype and neither is a constructor
                        emitJsStatement count """
function View(name, options){
    this.name = name;
    this.path = 'fake';
    $0++;
}

View.prototype.render = function(options, fn){
    fn(null, 'abstract engine');
};
"""

                        app.set("view cache", true)
                        app.set("view", emitJsExpr () "View")

                        app.render("something", {| cache = true |}, fun err str ->
                            if err.IsSome then
                                d err
                            else
                                Assert.strictEqual(count, 1)
                                Assert.strictEqual(str, "abstract engine")
                                app.render("something", fun err str ->
                                    if err.IsSome then
                                        d err
                                    else
                                        Assert.strictEqual(count, 1)
                                        Assert.strictEqual(str, "abstract engine")
                                        d ()
                                )
                        )
                    )
                )
            )
        )
    )

