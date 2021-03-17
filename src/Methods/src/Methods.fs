// ts2fable 0.8.0
module rec Methods

open System
open Fable.Core
open Fable.Core.JS

let [<Import("default","methods")>] methods: ResizeArray<Method> = jsNative

type [<StringEnum>] [<RequireQualifiedAccess>] Method =
    | [<CompiledName "ACL">] ACL
    | [<CompiledName "BIND">] BIND
    | [<CompiledName "CHECKOUT">] CHECKOUT
    | [<CompiledName "CONNECT">] CONNECT
    | [<CompiledName "COPY">] COPY
    | [<CompiledName "DELETE">] DELETE
    | [<CompiledName "GET">] GET
    | [<CompiledName "HEAD">] HEAD
    | [<CompiledName "LINK">] LINK
    | [<CompiledName "LOCK">] LOCK
    | [<CompiledName "M-SEARCH">] MSEARCH
    | [<CompiledName "MERGE">] MERGE
    | [<CompiledName "MKACTIVITY">] MKACTIVITY
    | [<CompiledName "MKCALENDAR">] MKCALENDAR
    | [<CompiledName "MKCOL">] MKCOL
    | [<CompiledName "MOVE">] MOVE
    | [<CompiledName "NOTIFY">] NOTIFY
    | [<CompiledName "OPTIONS">] OPTIONS
    | [<CompiledName "PATCH">] PATCH
    | [<CompiledName "POST">] POST
    | [<CompiledName "PROPFIND">] PROPFIND
    | [<CompiledName "PROPPATCH">] PROPPATCH
    | [<CompiledName "PURGE">] PURGE
    | [<CompiledName "PUT">] PUT
    | [<CompiledName "REBIND">] REBIND
    | [<CompiledName "REPORT">] REPORT
    | [<CompiledName "SEARCH">] SEARCH
    | [<CompiledName "SOURCE">] SOURCE
    | [<CompiledName "SUBSCRIBE">] SUBSCRIBE
    | [<CompiledName "TRACE">] TRACE
    | [<CompiledName "UNBIND">] UNBIND
    | [<CompiledName "UNLINK">] UNLINK
    | [<CompiledName "UNLOCK">] UNLOCK
    | [<CompiledName "UNSUBSCRIBE">] UNSUBSCRIBE
    | Acl
    | Bind
    | Checkout
    | Connect
    | Copy
    | Delete
    | Get
    | Head
    | Link
    | Lock
    | [<CompiledName "m-search">] MSearch
    | Merge
    | Mkactivity
    | Mkcalendar
    | Mkcol
    | Move
    | Notify
    | Options
    | Patch
    | Post
    | Propfind
    | Proppatch
    | Purge
    | Put
    | Rebind
    | Report
    | Search
    | Source
    | Subscribe
    | Trace
    | Unbind
    | Unlink
    | Unlock
    | Unsubscribe