module Tests.Chalk.Level

open Mocha
open Fable.Core
open Glutinum.Chalk

// Test ported from https://github.com/chalk/chalk/blob/4dab5e1fb6f42c6c9fdacbe34b9dafd24359208e/test/instance.js

[<Import("*", "assert")>]
let Assert: Node.Assert.IExports = jsNative

describe "Chalk" (fun _ ->

    describe "Level" (fun _ ->

        it "don't output colors when manually disabled" (fun _ ->
            let oldLevel = chalk.level
            chalk.level <- Chalk.Level.N0

            Assert.strictEqual(
                chalk.red.Invoke("foo"),
                "foo"
            )

            chalk.level <- oldLevel
        )

        it "enable/disable colors based on overall chalk .level property, not individual instances" (fun _ ->
            let oldLevel = chalk.level

            chalk.level <- Chalk.Level.N1
            let red = chalk.red

            Assert.strictEqual(
                red.level,
                Chalk.Level.N1
            )

            chalk.level <- Chalk.Level.N0

            Assert.strictEqual(
                red.level,
                chalk.level
            )

            chalk.level <- oldLevel
        )
    )
)
