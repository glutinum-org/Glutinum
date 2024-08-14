module Tests.Express.Port.App.Engine

open Mocha
open Glutinum.ExpressServeStaticCore
open Glutinum.Express
open Node
open Fable.Core.JsInterop

let __dirname =
    emitJsExpr () """
import { dirname } from 'dirname-filename-esm';
dirname(import.meta);
    """

type RenderOption =
    {
        user :
            {|
                name : string
            |}
    }

let render path (options : RenderOption) (fn : EngineRenderFunc) =
    fs.readFile(path, "utf8", fun (err : Base.ErrnoException option) (str : string) ->
        if err.IsSome then
            fn.Invoke(err, None)
        else
            let str = str.Replace("{{user.name}}", options.user.name)
            fn.Invoke(null, Some str)
    )


describe "app.engine(ext, fn)" (fun _ ->

    itAsync "should map a template engine" (fun d ->
        let app = express.express ()

        app.set("views", path.join(__dirname, "fixtures")) |> ignore
        app.engine(".html", render) |> ignore
        app.locals.["user"] <-
            {|
                name = "tobi"
            |}

        app.render("user.html", fun err str ->
            if err.IsSome then
                d err
            else
                Assert.strictEqual(str, "<p>tobi</p>")
                d()
        )
    )

    it "should throw when the callback is missing" (fun _ ->
        let app = express.express ()

        Assert.throws(
            (fun () ->
                app.engine(".html", unbox null)
            )
        )
    )

    itAsync """should work without leading "." """ (fun d ->
        let app = express.express ()

        app.set("views", path.join(__dirname, "fixtures")) |> ignore
        app.engine("html", render) |> ignore
        app.locals.["user"] <-
            {|
                name = "tobi"
            |}

        app.render("user.html", fun err str ->
            if err.IsSome then
                d err
            else
                Assert.strictEqual(str, "<p>tobi</p>")
                d()
        )

    )

    itAsync """should work "view engine" setting""" (fun d ->
        let app = express.express ()

        app.set("views", path.join(__dirname, "fixtures")) |> ignore
        app.engine("html", render) |> ignore
        app.set("view engine","html") |> ignore
        app.locals.["user"] <-
            {|
                name = "tobi"
            |}

        app.render("user.html", fun err str ->
            if err.IsSome then
                d err
            else
                Assert.strictEqual(str, "<p>tobi</p>")
                d()
        )
    )

    itAsync """should work "view engine" with leading "." """ (fun d ->
        let app = express.express ()

        app.set("views", path.join(__dirname, "fixtures")) |> ignore
        app.engine(".html", render) |> ignore
        app.set("view engine",".html") |> ignore
        app.locals.["user"] <-
            {|
                name = "tobi"
            |}

        app.render("user.html", fun err str ->
            if err.IsSome then
                d err
            else
                Assert.strictEqual(str, "<p>tobi</p>")
                d()
        )

    )

)
