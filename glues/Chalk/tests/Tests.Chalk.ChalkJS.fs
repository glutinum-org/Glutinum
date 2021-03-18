module Tests.Chalk.ChalkJS

open Mocha
open Fable.Core
open Fable.Core.JsInterop
open Glutinum.Chalk

// Test ported from https://github.com/chalk/chalk/blob/4dab5e1fb6f42c6c9fdacbe34b9dafd24359208e/test/chalk.js

[<Import("*", "assert")>]
let Assert: Node.Assert.IExports = jsNative

describe "Chalk" (fun _ ->

    it "don't add any styling when called as the base function" (fun _ ->
        Assert.strictEqual(chalk.Invoke("foo"), "foo")
    )

    it "support multiple arguments in base function" (fun _ ->
        Assert.strictEqual(chalk.Invoke("hello", "there"), "hello there")
    )

    it "support automatic casting to string" (fun _ ->
        Assert.strictEqual(chalk.Invoke(ResizeArray([ "hello"; "there" ])), "hello,there")
        Assert.strictEqual(chalk.Invoke(123), "123")

        Assert.strictEqual(chalk.bold.Invoke(ResizeArray([ "foo"; "bar" ])), "\u001B[1mfoo,bar\u001B[22m")
        Assert.strictEqual(chalk.green.Invoke(98765), "\u001B[32m98765\u001B[39m")
    )

    it "style string" (fun _ ->
        Assert.strictEqual(chalk.underline.Invoke("foo"), "\u001B[4mfoo\u001B[24m")
        Assert.strictEqual(chalk.red.Invoke("foo"), "\u001B[31mfoo\u001B[39m")
        Assert.strictEqual(chalk.bgRed.Invoke("foo"), "\u001B[41mfoo\u001B[49m")
    )

    it "support applying multiple styles at once" (fun  _ ->
        Assert.strictEqual(chalk.red.bgGreen.underline.Invoke("foo"), "\u001B[31m\u001B[42m\u001B[4mfoo\u001B[24m\u001B[49m\u001B[39m")
        Assert.strictEqual(chalk.underline.red.bgGreen.Invoke("foo"), "\u001B[4m\u001B[31m\u001B[42mfoo\u001B[49m\u001B[39m\u001B[24m")
    )

    it "support nesting styles" (fun _ ->
        Assert.strictEqual(
            chalk.red.Invoke(
                "foo" +
                chalk.underline.bgBlue.Invoke("bar") +
                "!"
            ),
            "\u001B[31mfoo\u001B[4m\u001B[44mbar\u001B[49m\u001B[24m!\u001B[39m"
        )
    )

    it "support nesting styles of the same type (color, underline, bg)" (fun  _ ->
        Assert.strictEqual(
            chalk.red.Invoke(
                "a" +
                chalk.yellow.Invoke(
                    "b" +
                    chalk.green.Invoke("c")
                    + "b"
                ) +
                "c"
            ),
            "\u001B[31ma\u001B[33mb\u001B[32mc\u001B[39m\u001B[31m\u001B[33mb\u001B[39m\u001B[31mc\u001B[39m"
        )
    )

    it "reset all styles with '.reset()'" (fun _ ->
        Assert.strictEqual(
            chalk.reset.Invoke(
                chalk.red.bgGreen.underline.Invoke("foo") + "foo"
            ),
            "\u001B[0m\u001B[31m\u001B[42m\u001B[4mfoo\u001B[24m\u001B[49m\u001B[39mfoo\u001B[0m"
        )
    )

    it "support caching multiple styles" (fun _ ->
        let red = chalk.red.red
        let green = chalk.red.green
        let redBold = red.bold
        let greenBold = green.bold

        Assert.notStrictEqual(red.Invoke("foo"), green.Invoke("foo"))
        Assert.notStrictEqual(redBold.Invoke("bar"), greenBold.Invoke("bar"))
        Assert.notStrictEqual(green.Invoke("baz"), greenBold.Invoke("baz"))
    )

    it "alias gray to grey" (fun _ ->
        Assert.strictEqual(
            chalk.grey.Invoke("foo"),
            "\u001B[90mfoo\u001B[39m"
        )
    )

    it "support variable number of arguments" (fun _ ->
        Assert.strictEqual(
            chalk.red.Invoke("foo", "bar"),
            "\u001B[31mfoo bar\u001B[39m"
        )
    )

    it "support falsy value" (fun _ ->
        Assert.strictEqual(
            chalk.red.Invoke(0),
            "\u001B[31m0\u001B[39m"
        )
    )

    it "don't output escape codes if the input is empty" (fun _ ->
        Assert.strictEqual(
            chalk.red.Invoke(),
            ""
        )
        Assert.strictEqual(
            chalk.red.blue.black.Invoke(),
            ""
        )
    )

    it "keep Function.prototype methods" (fun _ ->
        // Not ported because it test JavaScript specific behaviour
        // test('keep Function.prototype methods', t => {
        // 	t.is(Reflect.apply(chalk.grey, null, ['foo']), '\u001B[90mfoo\u001B[39m');
        // 	t.is(chalk.reset(chalk.red.bgGreen.underline.bind(null)('foo') + 'foo'), '\u001B[0m\u001B[31m\u001B[42m\u001B[4mfoo\u001B[24m\u001B[49m\u001B[39mfoo\u001B[0m');
        // 	t.is(chalk.red.blue.black.call(null), '');
        // });
        ()
    )

    it "line breaks should open and close colors" (fun _ ->
        Assert.strictEqual(
            chalk.grey.Invoke("hello\nworld"),
            "\u001B[90mhello\u001B[39m\n\u001B[90mworld\u001B[39m"
        )
    )

    it "line breaks should open and close colors with CRLF" (fun _ ->
        Assert.strictEqual(
            chalk.grey.Invoke("hello\r\nworld"),
            "\u001B[90mhello\u001B[39m\r\n\u001B[90mworld\u001B[39m"
        )
    )

    it "properly convert RGB to 16 colors on basic color terminals"  (fun _ ->
        Assert.strictEqual(
            chalk.Instance(jsOptions<Chalk.Options>(fun o ->
                o.level <- Some Chalk.Level.N1
            )).hex("#FF0000").Invoke("hello"),
            "\u001B[91mhello\u001B[39m"
        )

        Assert.strictEqual(
            chalk.Instance(jsOptions<Chalk.Options>(fun o ->
                o.level <- Some Chalk.Level.N1
            )).bgHex("#FF0000").Invoke("hello"),
            "\u001B[101mhello\u001B[49m"
        )
    )

    it "properly convert RGB to 256 colors on basic color terminals" (fun _ ->
        Assert.strictEqual(
            chalk.Instance(jsOptions<Chalk.Options>(fun o ->
                o.level <- Some Chalk.Level.N2
            )).hex("#FF0000").Invoke("hello"),
            "\u001B[38;5;196mhello\u001B[39m"
        )

        Assert.strictEqual(
            chalk.Instance(jsOptions<Chalk.Options>(fun o ->
                o.level <- Some Chalk.Level.N2
            )).bgHex("#FF0000").Invoke("hello"),
            "\u001B[48;5;196mhello\u001B[49m"
        )

        Assert.strictEqual(
            chalk.Instance(jsOptions<Chalk.Options>(fun o ->
                o.level <- Some Chalk.Level.N3
            )).bgHex("#FF0000").Invoke("hello"),
            "\u001B[48;2;255;0;0mhello\u001B[49m"
        )
    )

    it "don't emit RGB codes if level is 0" (fun _ ->
        Assert.strictEqual(
            chalk.Instance(jsOptions<Chalk.Options>(fun o ->
                o.level <- Some Chalk.Level.N0
            )).hex("#FF0000").Invoke("hello"),
            "hello"
        )

        Assert.strictEqual(
            chalk.Instance(jsOptions<Chalk.Options>(fun o ->
                o.level <- Some Chalk.Level.N0
            )).bgHex("#FF0000").Invoke("hello"),
            "hello"
        )
    )

    it "supports blackBright color" (fun _ ->
        Assert.strictEqual(
            chalk.blackBright.Invoke("foo"),
            "\u001B[90mfoo\u001B[39m"
        )
    )

    it "sets correct level for chalk.stderr and respects it" (fun _ ->
        Assert.strictEqual(
            chalk.stderr.level,
            3
        )

        Assert.strictEqual(
            chalk.stderr.red.bold.Invoke("foo"),
            "\u001B[31m\u001B[1mfoo\u001B[22m\u001B[39m"
        )
    )

    it "supports rbg colors" (fun _ ->
        Assert.strictEqual(
            chalk.rgb(123, 45, 67).underline.Invoke("foo"),
            "\u001B[38;2;123;45;67m\u001B[4mfoo\u001B[24m\u001B[39m"
        )
    )

    it "supports hsl colors" (fun _ ->
        Assert.strictEqual(
            chalk.hsl(32, 100, 50).bold.Invoke("foo"),
            "\u001B[38;2;255;136;0m\u001B[1mfoo\u001B[22m\u001B[39m"
        )
    )

    it "supports hsv colors" (fun _ ->
        Assert.strictEqual(
            chalk.hsv(32, 100, 100).bold.Invoke("foo"),
            "\u001B[38;2;255;136;0m\u001B[1mfoo\u001B[22m\u001B[39m"
        )
    )

    it "supports hwb colors" (fun _ ->
        Assert.strictEqual(
            chalk.hwb(32, 0, 50).bold.Invoke("foo"),
            "\u001B[38;2;128;68;0m\u001B[1mfoo\u001B[22m\u001B[39m"
        )
    )


    it "supports ansi colors" (fun _ ->
        Assert.strictEqual(
            chalk.ansi(31).bgAnsi(93).Invoke("foo"),
            "\u001B[38;2;128;0;0m\u001B[48;2;255;255;0mfoo\u001B[49m\u001B[39m"
        )
    )


    it "supports ansi256 colors" (fun _ ->
        Assert.strictEqual(
            chalk.bgAnsi256(194).Invoke("foo"),
            "\u001B[48;2;204;255;204mfoo\u001B[49m"
        )
    )
)
