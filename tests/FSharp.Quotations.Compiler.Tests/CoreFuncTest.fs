﻿(*
 * FSharp.Quotations.Compiler - a compiler for F# expression tree
 * Written in 2015 by bleis-tift (hey_c_est_la_vie@hotmail.co.jp)
 * kyonmm, zakky-dev
 * 
 * To the extent possible under law, the author(s) have dedicated all copyright
 * and related and neighboring rights to this software to the public domain worldwide.
 * This software is distributed without any warranty.
 * 
 * You should have received a copy of the CC0 Public Domain Dedication along with this software.
 * If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.
 *)
namespace FSharp.Quotations.Compiler.Tests

[<TestModule>]
module CoreFuncTest =
  [<Test>]
  let ``box int`` () = <@ box 1 @> |> check (box 1)

  [<Test>]
  let ``box bigint`` () = <@ box 1I @> |> check (box 1I)

  [<Test>]
  let ``box char`` () = <@ box 'a' @> |> check (box 'a')

  [<Test>]
  let ``box float`` () = <@ box 1.0 @> |> check (box 1.0)

  [<Test>]
  let ``box decimal`` () = <@ box 1.0M @> |> check (box 1.0M)

  [<Test>]
  let ``box bool`` () = <@ box true @> |> check (box true)

  [<Test>]
  let ``box byte`` () = <@ box 255uy @> |> check (box 255uy)

  [<Test>]
  let ``box string`` () = <@ box "str" @> |> check (box "str")

  [<Test>]
  let ``box int[]`` () = <@ box [|10|] @> |> check (box [|10|])

  [<Test>]
  let ``failwith string`` () = <@ failwith "str" @> |> checkExnType typeof<exn>

  [<Test>]
  let ``compare int int`` () = <@ compare 1 1 @> |> check 0

  [<Test>]
  let ``compare bigint bigint`` () = <@ compare 1I 1I @> |> check (compare 1I 1I)

  [<Test>]
  let ``compare char char`` () = <@ compare 'a' 'a' @> |> check 0

  [<Test>]
  let ``compare float float`` () = <@ compare 1.0 1.0 @> |> check 0

  [<Test>]
  let ``compare decimal decimal`` () = <@ compare 1.0M 1.0M @> |> check 0

  [<Test>]
  let ``compare bool bool`` () = <@ compare true true @> |> check 0

  [<Ignore("compareの結果ではなく、CompareToメソッド呼び出しの結果が返ってきてしまっているので、bge.un.s などを実装する必要があるように見える")>]
  [<TestCase(0uy, 0uy, 0)>]
  [<TestCase(1uy, 1uy, 0)>]
  [<TestCase(255uy, 255uy, 0)>]
  [<TestCase(0uy, 1uy, -1)>]
  [<TestCase(1uy, 255uy, -1)>]
  [<TestCase(1uy, 0uy, 1)>]
  [<TestCase(255uy, 1uy, 1)>]
  let ``compare byte byte`` (a:byte, b:byte, expected:int) = <@ compare a b @> |> check expected

  [<Test>]
  let ``compare string string`` () = <@ compare "str" "str" @> |> check 0

  [<Test>]
  let ``compare int[] int[]`` () = <@ compare [|10|] [|10|] @> |> check 0

  [<Test>]
  let ``defaultArg (string option) string`` () =
    <@ defaultArg None "default" @> |> check "default"
    <@ defaultArg (Some "str") "default" @> |> check "str"

  [<Test>]
  let ``defaultArg (char option) char `` () =
    <@ defaultArg None 'd' @> |> check 'd'
    <@ defaultArg (Some 'c') 'd' @> |> check 'c'

  [<Test>]
  let ``defaultArg with pipeline op`` () =
    <@ (None, "default") ||> defaultArg @> |> check "default"
    <@ (Some "str", "default") ||> defaultArg @> |> check "str"

  [<Test>]
  let ``hash int`` () = <@ hash 1 @> |> check 1

  [<Test>]
  let ``hash bigint`` () = <@ hash 1I @> |> check (hash 1I)

  [<Test>]
  let ``hash char`` () = <@ hash 'c' @> |> check (hash 'c')

  [<Test>]
  let ``hash float`` () = <@ hash 1.0 @> |> check (hash 1.0)

  [<Test>]
  let ``hash decimal`` () = <@ hash 1.0M @> |> check (hash 1.0M)

  [<Test>]
  let ``hash bool`` () = <@ hash true @> |> check 1

  [<TestCase(0uy)>]
  [<TestCase(1uy)>]
  [<TestCase(255uy)>]
  let ``hash byte`` (b:byte) = <@ hash b @> |> check (hash b)

  [<Test>]
  let ``hash string`` () = <@ hash "str" @> |> check (hash "str")

  [<Test>]
  let ``hash int[]`` () = <@ hash [|10|] @> |> check (hash [|10|])

  [<Test>]
  let ``limitedHash int int`` () = <@ limitedHash 1 1 @> |> check 1

  [<Test>]
  let ``limitedHash int bigint`` () = <@ limitedHash 1 1I @> |> check (limitedHash 1 1I)

  [<Test>]
  let ``limitedHash int char`` () = <@ limitedHash 1 'c' @> |> check (limitedHash 1 'c')

  [<Test>]
  let ``limitedHash int float`` () = <@ limitedHash 1 1.0 @> |> check (limitedHash 1 1.0)

  [<Test>]
  let ``limitedHash int decimal`` () = <@ limitedHash 1 1.0M @> |> check (limitedHash 1 1.0M)

  [<Test>]
  let ``limitedHash int bool`` () = <@ limitedHash 1 true @> |> check 1

  [<TestCase(1, 0uy)>]
  [<TestCase(1, 1uy)>]
  [<TestCase(1, 255uy)>]
  let ``limitedHash int byte`` (i:int, b:byte) = <@ limitedHash i b @> |> check (limitedHash i b)

  [<Test>]
  let ``limitedHash int string`` () = <@ limitedHash 1 "str" @> |> check (limitedHash 1 "str")

  [<Test>]
  let ``limitedHash int int[]`` () = <@ limitedHash 1 [|10|] @> |> check (limitedHash 1 [|10|])

  [<Test>]
  let ``id int`` () = <@ id 1 @> |> check 1

  [<Test>]
  let ``id bigint`` () = <@ id 1I @> |> check (id 1I)

  [<Test>]
  let ``id char`` () = <@ id 'c' @> |> check 'c'

  [<Test>]
  let ``id float`` () = <@ id 1.0 @> |> check 1.0

  [<Test>]
  let ``id decimal`` () = <@ id 1.0M @> |> check 1.0M

  [<Test>]
  let ``id bool`` () = <@ id true @> |> check true

  [<TestCase(0uy)>]
  [<TestCase(1uy)>]
  [<TestCase(255uy)>]
  let ``id byte`` (b:byte) = <@ id b @> |> check b

  [<Test>]
  let ``id string`` () = <@ id "str" @> |> check "str"

  [<Test>]
  let ``id int[]`` () = <@ id [|10|] @> |> check [|10|]

  [<Test>]
  let ``ignore int`` () = <@ ignore 1 @> |> check ()

  [<Test>]
  let ``ignore bigint`` () = <@ ignore 1I @> |> check ()

  [<Test>]
  let ``ignore char`` () = <@ ignore 'c' @> |> check ()

  [<Test>]
  let ``ignore float`` () = <@ ignore 1.0 @> |> check ()

  [<Test>]
  let ``ignore decimal`` () = <@ ignore 1.0M @> |> check ()

  [<Test>]
  let ``ignore bool`` () = <@ ignore true @> |> check ()

  [<TestCase(0uy)>]
  [<TestCase(1uy)>]
  [<TestCase(255uy)>]
  let ``ignore byte`` (b:byte) = <@ ignore b @> |> check ()

  [<Test>]
  let ``ignore string`` () = <@ ignore "str" @> |> check ()

  [<Test>]
  let ``ignore int[]`` () = <@ ignore [|10|] @> |> check ()

  [<Test>]
  let ``invalidArg string string`` () =
    <@ invalidArg "a" "b" @> |> checkExnType typeof<System.ArgumentException>

  [<Test>]
  let ``invalidOp string`` () =
    <@ invalidOp "a" @> |> checkExnType typeof<System.InvalidOperationException>

  [<Test>]
  let ``max int int`` () = <@ max 10 20 @> |> check 20

  [<Test>]
  let ``max bigint bigint`` () = <@ max 10I 20I @> |> check (max 10I 20I)

  [<Test>]
  let ``max char char`` () = <@ max 'a' 'b' @> |> check 'b'

  [<Test>]
  let ``max float float`` () = <@ max 1.0 2.0 @> |> check 2.0

  [<Test>]
  let ``max decimal decimal`` () = <@ max 1.0M 2.0M @> |> check 2.0M

  [<Test>]
  let ``max bool bool`` () = <@ max false true @> |> check true

  [<TestCase(0uy, 0uy)>]
  [<TestCase(0uy, 1uy)>]
  [<TestCase(0uy, 254uy)>]
  [<TestCase(0uy, 255uy)>]
  [<TestCase(255uy, 0uy)>]
  [<TestCase(255uy, 1uy)>]
  [<TestCase(255uy, 254uy)>]
  [<TestCase(255uy, 255uy)>]
  let ``max byte byte`` (a:byte, b:byte) = <@ max a b @> |> check (max a b)

  [<Test>]
  let ``max string string`` () = <@ max "aaa" "bbb" @> |> check "bbb"

  [<Test>]
  let ``max int[] int[]`` () = <@ max [|10|] [|20|] @> |> check [|20|]

  [<Test>]
  let ``min int int`` () = <@ min 10 20 @> |> check 10

  [<Test>]
  let ``min bigint bigint`` () = <@ min 10I 20I @> |> check 10I

  [<Test>]
  let ``min char char`` () = <@ min 'a' 'b' @> |> check 'a'

  [<Test>]
  let ``min float float`` () = <@ min 1.0 2.0 @> |> check 1.0

  [<Test>]
  let ``min decimal decimal`` () = <@ min 1.0M 2.0M @> |> check 1.0M

  [<Test>]
  let ``min bool bool`` () = <@ min false true @> |> check false

  [<TestCase(0uy, 0uy)>]
  [<TestCase(0uy, 1uy)>]
  [<TestCase(0uy, 254uy)>]
  [<TestCase(0uy, 255uy)>]
  [<TestCase(255uy, 0uy)>]
  [<TestCase(255uy, 1uy)>]
  [<TestCase(255uy, 254uy)>]
  [<TestCase(255uy, 255uy)>]
  let ``min byte byte`` (a:byte, b:byte) = <@ min a b @> |> check (min a b)

  [<Test>]
  let ``min string string`` () = <@ min "aaa" "bbb" @> |> check "aaa"

  [<Test>]
  let ``min int[] int[]`` () = <@ min [|10|] [|20|] @> |> check [|10|]

  [<Test>]
  let ``sizeof type`` () =
    <@ sizeof<byte> @> |> check (sizeof<byte>)
    <@ sizeof<int> @> |> check (sizeof<int>)
    <@ sizeof<string> @> |> check (sizeof<string>)

  [<Test>]
  let ``typeof type`` () =
    <@ typeof<byte> @> |> check (typeof<byte>)
    <@ typeof<int> @> |> check (typeof<int>)
    <@ typeof<string> @> |> check (typeof<string>)

  [<Test>]
  let ``typedefof type`` () =
    <@ typedefof<byte> @> |> check (typedefof<byte>)
    <@ typedefof<int> @> |> check (typedefof<int>)
    <@ typedefof<string> @> |> check (typedefof<string>)

  [<Test>]
  let ``not bool`` () = <@ not true @> |> check false

  [<Test>]
  let ``nullArg string`` () =
    <@ nullArg "a" @> |> checkExnType typeof<System.ArgumentNullException>

  [<Test>]
  let ``fst int * string`` () = <@ fst (1, "str") @> |> check 1

  [<Test>]
  let ``snd int * string`` () = <@ snd (1, "str") @> |> check "str"

  type Disposable () =
    member val Disposed = false with get, set
    interface System.IDisposable with
      member this.Dispose () =
        this.Disposed <- true

  [<Test>]
  let ``using`` () =
    <@ let x = new Disposable()
       using x (fun _ -> ())
       x.Disposed @>
    |> check true
    <@ let x: Disposable = Unchecked.defaultof<Disposable>
       using x (fun _ -> ()) @>
    |> check ()

  module Unchecked =
    open Microsoft.FSharp.Core.Operators.Unchecked

    [<Test>]
    let ``Unchecked.compre int int`` () = <@ compare 1 1 @> |> check 0

    [<Test>]
    let ``Unchecked.compre bigint bigint`` () = <@ compare 1I 1I @> |> check (compare 1I 1I)

    [<Test>]
    let ``Unchecked.compare char char`` () = <@ compare 'a' 'a' @> |> check 0

    [<Test>]
    let ``Unchecked.compare float float`` () = <@ compare 1.0 1.0 @> |> check 0

    [<Test>]
    let ``Unchecked.compare decimal decimal`` () = <@ compare 1.0M 1.0M @> |> check 0

    [<Ignore("compareの結果ではなく、CompareToメソッド呼び出しの結果が返ってきてしまっているので、bge.un.s などを実装する必要があるように見える")>]
    [<TestCase(0uy, 0uy, 0)>]
    [<TestCase(1uy, 1uy, 0)>]
    [<TestCase(255uy, 255uy, 0)>]
    [<TestCase(0uy, 1uy, -1)>]
    [<TestCase(1uy, 255uy, -1)>]
    [<TestCase(1uy, 0uy, 1)>]
    [<TestCase(255uy, 1uy, 1)>]
    let ``Unchecked.compare byte byte`` (a:byte, b:byte, expected:int) = <@ compare a b @> |> check expected

    [<Test>]
    let ``Unchecked.hash int`` () = <@ hash 1 @> |> check 1

    [<Test>]
    let ``Unchecked.hash bigint`` () = <@ hash 1I @> |> check (hash 1I)

    [<Test>]
    let ``Unchecked.hash char`` () = <@ hash 'c' @> |> check (hash 'c')

    [<Test>]
    let ``Unchecked.hash float`` () = <@ hash 1.0 @> |> check (hash 1.0)

    [<Test>]
    let ``Unchecked.hash decimal`` () = <@ hash 1.0M @> |> check (hash 1.0M)

    [<Test>]
    let ``Unchecked.hash byte`` () = <@ hash 1uy @> |> check (hash 1uy)

    [<Test>]
    let ``Unchecked.equals int int`` () = <@ equals 1 1 @> |> check true

    [<Test>]
    let ``Unchecked.equals bigint bigint`` () = <@ equals 1I 1I @> |> check true

    [<Test>]
    let ``Unchecked.equals char char`` () = <@ equals 'c' 'c' @> |> check true

    [<Test>]
    let ``Unchecked.equals float flaot`` () = <@ equals 1.0 1.0 @> |> check true

    [<Test>]
    let ``Unchecked.equals decimal decimal`` () = <@ equals 1.0M 1.0M @> |> check true

    [<Test>]
    let ``Unchecked.equals byte byte`` () = <@ equals 1uy 1uy @> |> check true

    [<Test>]
    let ``Unchecked.defaultof char`` () = <@ defaultof<char> @> |> check defaultof<char>
