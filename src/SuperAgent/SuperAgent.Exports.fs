[<AutoOpen>]
module rec Npm.Exports

open Fable.Core

type Npm.Types.IExports with
    /// <summary>
    /// API not tested.
    ///
    /// I created the bindings in order to get the types.
    ///
    /// If there is a bug please open an issue
    /// </summary>
    [<Import("default", "superagent")>]
    member __.superagent (url : string) : SuperAgent.Types.Request.SuperAgentRequest = jsNative
