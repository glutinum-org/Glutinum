# Npm bindings

The goal of this repository is to provide a collection of Npm bindings and mutualize the infrastructure code to lower the maintenance cost.

For each bindings, you can find the documentation in the `README.md` file from their respective folders.

You can also take a look at the `tests` folder. Each binding comes with his own set of tests. Currently, we have around 450 tests.

## How to use this repository?

If you are on Windows using the standard terminal you need to run `node build.js`.

If you are on Linux, OSX or Windows using a bash-like terminal you can run `build.js` directly.

From now, I will always use `build.js` in the command as it is shorter.

Run `build.js --help` for more information about which command are supported.

### Getting completion from Linux, OSX, Windows bash-like

It is possible to have `TAB` completation support from your terminal.

Run `./build.js completion` and follow the instruction.
