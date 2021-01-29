module Tests.Express.App.Engine

open Npm
open Mocha
open ExpressServeStaticCore
open Node
open Fable.Core.JsInterop

type CallbackHandler = Types.SuperTest.Supertest.CallbackHandler

#nowarn "40"

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

let tests () =
    describe "app.engine(ext, fn)" (fun _ ->

        itAsync "should map a template engine" (fun d ->
            let app = Express.e.express ()

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
                    // TODO: should binding
                    str?should?equal("<p>tobi</p>")
                    d()
            )
        )

        it "should throw when the callback is missing" (fun _ ->
            let app = Express.e.express ()
            let x = unbox null

            // TODO: should binding
            (fun () -> app.engine(".html", unbox null))
                ?should
                ?throw("callback function required")
        )

        itAsync """should work without leading "." """ (fun d ->
            let app = Express.e.express ()

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
                    // TODO: should binding
                    str?should?equal("<p>tobi</p>")
                    d()
            )

        )

        itAsync """should work "view engine" setting""" (fun d ->
            let app = Express.e.express ()

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
                    // TODO: should binding
                    str?should?equal("<p>tobi</p>")
                    d()
            )
        )

        itAsync """should work "view engine" with leading "." """ (fun d ->
            let app = Express.e.express ()

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
                    // TODO: should binding
                    str?should?equal("<p>tobi</p>")
                    d()
            )

        )

    )
