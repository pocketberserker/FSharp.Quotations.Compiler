﻿namespace FSharp.Quotations.Compiler.Tests

[<TestModule>]
module ObjTest =
  type Class (value: int) =
    member val Value = value with get, set
    override __.Equals(x) =
      match x with
      | :? Class as other -> value = other.Value
      | _ -> false
    override __.GetHashCode() = value
    override __.ToString() = sprintf "Class(%d)" value

  [<Test>]
  let ``new`` () = <@ new Class(42) @> |> check (new Class(42))

  [<Test>]
  let ``ToString()`` () = <@ (Class(42).ToString()) @> |> check (Class(42).ToString())

  [<Test>]
  let ``property get`` () = <@ (Class(42)).Value @> |> check 42

  [<Test>]
  let ``property set`` () =
    <@ let c = Class(42)
       c.Value <- 10
       c.Value
    @> |> check 10