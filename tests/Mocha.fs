namespace Mocha

open Fable.Core

[<AutoOpen>]
module Api =

    let [<Global>] describe (name: string) (f: unit->unit) : unit = jsNative
    let [<Global>] it (msg: string) (f: unit->unit) : unit = jsNative
    let [<Global; Emit("it($1...)")>] itAsync (msg: string) (f: (unit->unit)->unit) : unit = jsNative
