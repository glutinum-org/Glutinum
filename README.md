# Glutinum

Glutinum is a tentative to bring an equivalent to [DefinitelyTyped](https://github.com/DefinitelyTyped/DefinitelyTyped) for Fable ecosystem.

Right now Glutinum contains bindings generated using [ts2fable](https://github.com/fable-compiler/ts2fable/) and improved manually.

In the future, Glutinum will try to use it's own converter to experiment with a new way to write bindings.

## Can I use the packages from this repository?

Yes, you can.

Most of the package already have a battery of tests to check regression and if they works.

## Structure of a glue

A glue, is always no-code bindings between F# and JavaScript code.

If there are code addition made to improve the user experience when working with the package then it goes into another package `Glutinum.XXX.Extensions`.

Example: `Glutinum.RangeParser.Extensions`

This is so we don't have to make all the packages includes their source code under `.fable` directory and should improve Fable memory consumption and performance.

## Tests status

<!-- DON'T REMOVE - begin tests status -->

| Binding | Number of tests |
|---------|-----------------|
| BodyParser | 5 |
| Chalk | 30 |
| Connect | 3 |
| Express | 424 |
| Mime | 4 |
| Qs | 5 |
| RangeParser | 5 |
| ServeStatic | 2 |

<!-- DON'T REMOVE - end tests status -->

## How to use this repository?

If you are on Windows using the standard terminal you need to run `node build.js`.

If you are on Linux, OSX or Windows using a bash-like terminal you can run `build.js` directly.

From now, I will always use `build.js` in the command as it is shorter.

Run `build.js --help` for more information about which command are supported.

### Getting completion from Linux, OSX, Windows bash-like

It is possible to have `TAB` completion support from your terminal.

Run `./build.js completion` and follow the instruction.

### Why this name?

It comes from the ideas that this project is trying to **glue** together F# and TypeScript.

Glutinum is from the Latin word gluten ("glue") with the suffix -um.
