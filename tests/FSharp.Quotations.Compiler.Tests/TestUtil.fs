﻿namespace FSharp.Quotations.Compiler.Tests

open NUnit.Framework
open FsUnit

open Microsoft.FSharp.Quotations
open FSharp.Quotations.Compiler
open System

[<AutoOpen>]
module TestUtil =
  type TestModule () =
    inherit TestFixtureAttribute()

  type Test () =
    inherit TestAttribute()

  [<AttributeUsage(AttributeTargets.Method, AllowMultiple=true)>]
  type TestCase ([<ParamArray>] arguments: obj []) =
    inherit TestCaseAttribute(arguments)

  type IntRange(from: int, ``to``: int, step: int) =
    inherit RangeAttribute(from, ``to``, step)

    new (from, ``to``) = IntRange(from, ``to``, 1)

  let check (expected: 'T) (expr: Expr<'T>) =
    expr |> ExprCompiler.compile
    |> should equal expected

  let checkExn<'T, 'TExn when 'TExn :> exn> (expr: Expr<'T>) =
    try
      ExprCompiler.compile expr |> ignore
      Assert.Fail("exception is not thrown.")
    with :? 'TExn -> ()