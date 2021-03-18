module rec Glutinum.Chalk

open Fable.Core
open System

type TemplateStringsArray = System.Collections.Generic.IReadOnlyList<string>

/// <summary>
/// Main Chalk object that allows to chain styles together.
/// Call the last one as a method with a string argument.
/// Order doesn't matter, and later styles take precedent in case of a conflict.
/// This simply means that <c>chalk.red.yellow.green</c> is equivalent to <c>chalk.green</c>.
/// </summary>
[<Import("default", "chalk")>]
let chalk : Chalk.IExports = jsNative

module Chalk =

    type IExports =
        inherit Chalk
        abstract supportsColor: ColorSupport with get, set
        abstract Level: Level with get, set
        abstract Color: Color with get, set
        abstract ForegroundColor: ForegroundColor with get, set
        abstract BackgroundColor: BackgroundColor with get, set
        abstract Modifiers: Modifiers with get, set
        abstract stderr: Stderr with get, set

    type Stderr =
        inherit Chalk
        abstract supportsColor: ColorSupport with get, set

    /// <summary>
    /// Basic foreground colors.
    ///
    /// <see href="https://github.com/chalk/chalk/blob/main/readme.md#256-and-truecolor-color-support">More colors here.</see>
    /// </summary>
    type [<StringEnum>] [<RequireQualifiedAccess>] ForegroundColor =
        | Black
        | Red
        | Green
        | Yellow
        | Blue
        | Magenta
        | Cyan
        | White
        | Gray
        | Grey
        | BlackBright
        | RedBright
        | GreenBright
        | YellowBright
        | BlueBright
        | MagentaBright
        | CyanBright
        | WhiteBright

    /// <summary>
    /// Basic background colors.
    ///
    /// <see href="https://github.com/chalk/chalk/blob/main/readme.md#256-and-truecolor-color-support">More colors here.</see>
    /// </summary>
    type [<StringEnum>] [<RequireQualifiedAccess>] BackgroundColor =
        | BgBlack
        | BgRed
        | BgGreen
        | BgYellow
        | BgBlue
        | BgMagenta
        | BgCyan
        | BgWhite
        | BgGray
        | BgGrey
        | BgBlackBright
        | BgRedBright
        | BgGreenBright
        | BgYellowBright
        | BgBlueBright
        | BgMagentaBright
        | BgCyanBright
        | BgWhiteBright

    /// <summary>
    /// Basic colors.
    ///
    /// <see href="https://github.com/chalk/chalk/blob/main/readme.md#256-and-truecolor-color-support">More colors here.</see>
    /// </summary>
    type [<StringEnum>] [<RequireQualifiedAccess>] Color =
        | Black
        | Red
        | Green
        | Yellow
        | Blue
        | Magenta
        | Cyan
        | White
        | Gray
        | Grey
        | BlackBright
        | RedBright
        | GreenBright
        | YellowBright
        | BlueBright
        | MagentaBright
        | CyanBright
        | WhiteBright
        | BgBlack
        | BgRed
        | BgGreen
        | BgYellow
        | BgBlue
        | BgMagenta
        | BgCyan
        | BgWhite
        | BgGray
        | BgGrey
        | BgBlackBright
        | BgRedBright
        | BgGreenBright
        | BgYellowBright
        | BgBlueBright
        | BgMagentaBright
        | BgCyanBright
        | BgWhiteBright

    type [<StringEnum>] [<RequireQualifiedAccess>] Modifiers =
        | Reset
        | Bold
        | Dim
        | Italic
        | Underline
        | Inverse
        | Hidden
        | Strikethrough
        | Visible

    /// <summary>
    /// Levels:
    /// - <c>0</c> - All colors disabled.
    /// - <c>1</c> - Basic 16 colors support.
    /// - <c>2</c> - ANSI 256 colors support.
    /// - <c>3</c> - Truecolor 16 million colors support.
    /// </summary>
    type [<RequireQualifiedAccess>] Level =
        | N0 = 0
        | N1 = 1
        | N2 = 2
        | N3 = 3

    type [<AllowNullLiteral>] Options =
        /// <summary>
        /// Specify the color support for Chalk.
        ///
        /// By default, color support is automatically detected based on the environment.
        ///
        /// Levels:
        /// - <c>0</c> - All colors disabled.
        /// - <c>1</c> - Basic 16 colors support.
        /// - <c>2</c> - ANSI 256 colors support.
        /// - <c>3</c> - Truecolor 16 million colors support.
        /// </summary>
        abstract level: Level option with get, set

    /// Detect whether the terminal supports color.
    type [<AllowNullLiteral>] ColorSupport =
        /// The color level used by Chalk.
        abstract level: Level with get, set
        /// Return whether Chalk supports basic 16 colors.
        abstract hasBasic: bool with get, set
        /// Return whether Chalk supports ANSI 256 colors.
        abstract has256: bool with get, set
        /// Return whether Chalk supports Truecolor 16 million colors.
        abstract has16m: bool with get, set

    type [<AllowNullLiteral>] ChalkFunction =
        /// <summary>Use a template string.</summary>
        /// <remarks>Template literals are unsupported for nested calls (see <see href="https://github.com/chalk/chalk/issues/341)">issue #341</see></remarks>
        /// <example>
        /// <code>
        /// import chalk = require('chalk');
        /// log(chalk`
        /// CPU: {red ${cpu.totalPercent}%}
        /// RAM: {green ${ram.used / ram.total * 100}%}
        /// DISK: {rgb(255,131,0) ${disk.used / disk.total * 100}%}
        /// `);
        /// </code>
        /// </example>
        /// <example>
        /// <code>
        /// import chalk = require('chalk');
        /// log(chalk.red.bgBlack`2 + 3 = {bold ${2 + 3}}`)
        /// </code>
        /// </example>
        [<Emit "$0($1...)">] abstract Invoke: text: TemplateStringsArray * [<ParamArray>] placeholders: obj[] -> string
        [<Emit "$0($1...)">] abstract Invoke: [<ParamArray>] text: #obj[] -> string

    /// <summary>
    /// Main Chalk object that allows to chain styles together.
    /// Call the last one as a method with a string argument.
    /// Order doesn't matter, and later styles take precedent in case of a conflict.
    /// This simply means that <c>chalk.red.yellow.green</c> is equivalent to <c>chalk.green</c>.
    /// </summary>
    type [<AllowNullLiteral>] Chalk =
        inherit ChalkFunction
        /// Return a new Chalk instance.
        [<Emit("new $0.Instance($1...)")>]
        abstract Instance: ?options : Options -> Chalk
        /// <summary>
        /// The color support for Chalk.
        ///
        /// By default, color support is automatically detected based on the environment.
        ///
        /// Levels:
        /// - <c>0</c> - All colors disabled.
        /// - <c>1</c> - Basic 16 colors support.
        /// - <c>2</c> - ANSI 256 colors support.
        /// - <c>3</c> - Truecolor 16 million colors support.
        /// </summary>
        abstract level: Level with get, set
        /// <summary>Use HEX value to set text color.</summary>
        /// <param name="color">Hexadecimal value representing the desired color.</param>
        /// <example>
        /// <code>
        /// import chalk = require('chalk');
        /// chalk.hex('#DEADED');
        /// </code>
        /// </example>
        abstract hex: color : string -> Chalk
        /// <summary>Use keyword color value to set text color.</summary>
        /// <param name="color">Keyword value representing the desired color.</param>
        /// <example>
        /// <code>
        /// import chalk = require('chalk');
        /// chalk.keyword('orange');
        /// </code>
        /// </example>
        abstract keyword: color : string -> Chalk
        /// Use RGB values to set text color.
        abstract rgb: int * int * int -> Chalk
        /// Use HSL values to set text color.
        abstract hsl: int * int * int -> Chalk
        /// Use HSV values to set text color.
        abstract hsv: int * int * int -> Chalk
        /// Use HWB values to set text color.
        abstract hwb: int * int * int -> Chalk
        /// <summary>
        /// Use a <see href="https://en.wikipedia.org/wiki/ANSI_escape_code#SGR_parameters) (SGR">Select/Set Graphic Rendition</see> <see href="https://en.wikipedia.org/wiki/ANSI_escape_code#3/4_bit">color code number</see> to set text color.
        ///
        /// 30 &lt;= code &amp;&amp; code &lt; 38 || 90 &lt;= code &amp;&amp; code &lt; 98
        /// For example, 31 for red, 91 for redBright.
        /// </summary>
        abstract ansi: int -> Chalk
        /// <summary>Use a <see href="https://en.wikipedia.org/wiki/ANSI_escape_code#8-bit">8-bit unsigned number</see> to set text color.</summary>
        abstract ansi256: int -> Chalk
        /// <summary>Use HEX value to set background color.</summary>
        /// <param name="color">Hexadecimal value representing the desired color.</param>
        /// <example>
        /// <code>
        /// import chalk = require('chalk');
        /// chalk.bgHex('#DEADED');
        /// </code>
        /// </example>
        abstract bgHex: color : string -> Chalk
        /// <summary>Use keyword color value to set background color.</summary>
        /// <param name="color">Keyword value representing the desired color.</param>
        /// <example>
        /// <code>
        /// import chalk = require('chalk');
        /// chalk.bgKeyword('orange');
        /// </code>
        /// </example>
        abstract bgKeyword: color : string -> Chalk
        /// Use RGB values to set background color.
        abstract bgRgb: (float -> float -> float -> Chalk)
        /// Use HSL values to set background color.
        abstract bgHsl: (float -> float -> float -> Chalk)
        /// Use HSV values to set background color.
        abstract bgHsv: (float -> float -> float -> Chalk)
        /// Use HWB values to set background color.
        abstract bgHwb: (float -> float -> float -> Chalk)
        /// <summary>
        /// Use a <see href="https://en.wikipedia.org/wiki/ANSI_escape_code#SGR_parameters) (SGR">Select/Set Graphic Rendition</see> <see href="https://en.wikipedia.org/wiki/ANSI_escape_code#3/4_bit">color code number</see> to set background color.
        ///
        /// 30 &lt;= code &amp;&amp; code &lt; 38 || 90 &lt;= code &amp;&amp; code &lt; 98
        /// For example, 31 for red, 91 for redBright.
        /// Use the foreground code, not the background code (for example, not 41, nor 101).
        /// </summary>
        abstract bgAnsi: int -> Chalk
        /// <summary>Use a <see href="https://en.wikipedia.org/wiki/ANSI_escape_code#8-bit">8-bit unsigned number</see> to set background color.</summary>
        abstract bgAnsi256: int -> Chalk
        /// Modifier: Resets the current color chain.
        abstract reset: Chalk
        /// Modifier: Make text bold.
        abstract bold: Chalk
        /// Modifier: Emitting only a small amount of light.
        abstract dim: Chalk
        /// Modifier: Make text italic. (Not widely supported)
        abstract italic: Chalk
        /// Modifier: Make text underline. (Not widely supported)
        abstract underline: Chalk
        /// Modifier: Inverse background and foreground colors.
        abstract inverse: Chalk
        /// Modifier: Prints the text, but makes it invisible.
        abstract hidden: Chalk
        /// Modifier: Puts a horizontal line through the center of the text. (Not widely supported)
        abstract strikethrough: Chalk
        /// Modifier: Prints the text only when Chalk has a color support level > 0.
        /// Can be useful for things that are purely cosmetic.
        abstract visible: Chalk
        abstract black: Chalk
        abstract red: Chalk
        abstract green: Chalk
        abstract yellow: Chalk
        abstract blue: Chalk
        abstract magenta: Chalk
        abstract cyan: Chalk
        abstract white: Chalk
        abstract gray: Chalk
        abstract grey: Chalk
        abstract blackBright: Chalk
        abstract redBright: Chalk
        abstract greenBright: Chalk
        abstract yellowBright: Chalk
        abstract blueBright: Chalk
        abstract magentaBright: Chalk
        abstract cyanBright: Chalk
        abstract whiteBright: Chalk
        abstract bgBlack: Chalk
        abstract bgRed: Chalk
        abstract bgGreen: Chalk
        abstract bgYellow: Chalk
        abstract bgBlue: Chalk
        abstract bgMagenta: Chalk
        abstract bgCyan: Chalk
        abstract bgWhite: Chalk
        abstract bgGray: Chalk
        abstract bgGrey: Chalk
        abstract bgBlackBright: Chalk
        abstract bgRedBright: Chalk
        abstract bgGreenBright: Chalk
        abstract bgYellowBright: Chalk
        abstract bgBlueBright: Chalk
        abstract bgMagentaBright: Chalk
        abstract bgCyanBright: Chalk
        abstract bgWhiteBright: Chalk
