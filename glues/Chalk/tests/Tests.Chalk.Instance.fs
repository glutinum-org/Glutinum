module Tests.Chalk.Instance

open Mocha
open Fable.Core
open Fable.Core.JsInterop
open Glutinum.Chalk

// Test ported from https://github.com/chalk/chalk/blob/4dab5e1fb6f42c6c9fdacbe34b9dafd24359208e/test/instance.js

[<Import("*", "assert")>]
let Assert: Node.Assert.IExports = jsNative

describe "Chalk" (fun _ ->

    describe "Instance" (fun _ ->

        it "create an isolated context where colors can be disabled (by level)" (fun _ ->
            let instance = chalk.Instance(jsOptions<Chalk.Options>(fun o ->
                o.level <- Some Chalk.Level.N0
            ))

            Assert.strictEqual(
                instance.red.Invoke("foo"),
                "foo"
            )

            Assert.strictEqual(
                chalk.red.Invoke("foo"),
                "\u001B[31mfoo\u001B[39m"
            )

            instance.level <- Chalk.Level.N2


            Assert.strictEqual(
                instance.red.Invoke("foo"),
                "\u001B[31mfoo\u001B[39m"
            )

        )

    )

)
