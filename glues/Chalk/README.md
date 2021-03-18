# Glutinum.Chalk

Binding for [https://github.com/chalk/chalk](chalk)

## Limitations

This package doesn't support template literal style notation from chalk.

## Usage

*If you want to compare the F# code with JavaScript the section below is a port of [Chalk - Usage](https://github.com/chalk/chalk/tree/4dab5e1fb6f42c6c9fdacbe34b9dafd24359208e#usage)*

```fs
open Glutinum.Chalk
open Fable.Core

let log x = JS.console.log(x)

// Combine styled and normal strings
log(chalk.blue.Invoke("Hello") + " World" + chalk.red.Invoke("!"))

// Compose multiples styles using the chainable API
log(chalk.blue.bgRed.bold.Invoke("Hello world!"))

// Pass in multiple arguments
log(chalk.blue.Invoke("Hello", "World!", "Foo", "bar", "biz", "baz"))

// Nest styles
log(chalk.red.Invoke("Hello", chalk.underline.bgBlue.Invoke("world") + "!"));

// Nest styles of the same type even (color, underline, background)
log(
    chalk.green.Invoke(
        "I am a green line " +
        chalk.blue.underline.bold.Invoke("with a blue substring") +
        " that becomes green again!"
));

// Use RGB colors in terminal emulators that support it.
log(chalk.keyword("orange").Invoke("Yay for orange colored text!"));
log(chalk.rgb(123, 45, 67).underline.Invoke("Underlined reddish color"));
log(chalk.hex("#DEADED").bold.Invoke("Bold gray!"));
```
