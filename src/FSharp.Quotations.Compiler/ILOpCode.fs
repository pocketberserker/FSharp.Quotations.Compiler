﻿namespace FSharp.Quotations.Compiler

open System
open System.Reflection
open System.Reflection.Emit

type CallTarget =
  | Method of MethodInfo
  | Ctor of ConstructorInfo
  | PropGet of PropertyInfo

type Token =
  | TokType of Type
  | TokMethod of MethodInfo
  | TokField of FieldInfo

type ILOpCode =
  | And
  | Or
  | Xor
  | Not
  | Shl
  | Shr
  | Div
  | Mul
  | Mul_Ovf
  | Neg
  | Rem
  | Add
  | Sub
  | Sub_Ovf
  | Brfalse of Label
  | Br of Label
  | Leave of Label
  | Endfinally
  | Isinst of Type
  | Conv_I
  | Conv_I1
  | Conv_I2
  | Conv_I4
  | Conv_I8
  | Conv_R4
  | Conv_R8
  | Conv_U
  | Conv_U1
  | Conv_U2
  | Conv_U4
  | Conv_U8
  | Conv_Ovf_I
  | Conv_Ovf_I1
  | Conv_Ovf_I2
  | Conv_Ovf_I4
  | Conv_Ovf_I8
  | Conv_Ovf_U
  | Conv_Ovf_U1
  | Conv_Ovf_U2
  | Conv_Ovf_U4
  | Conv_Ovf_U8
  | Box of Type
  | Unbox_Any of Type
  | Stloc of LocalBuilder * string option
  | Ldloc of LocalBuilder * string option
  | Ldloca of LocalBuilder * string option
  | Starg of int
  | Ldarg_0
  | Ldarg_1
  | Ldarg_2
  | Ldarg_3
  | Ldarg of int
  | Ldsfld of FieldInfo
  | Stfld of FieldInfo
  | Ldfld of FieldInfo
  | Ldnull
  | Ldstr of string
  | Ldc_I4_M1
  | Ldc_I4_0
  | Ldc_I4_1
  | Ldc_I4_2
  | Ldc_I4_3
  | Ldc_I4_4
  | Ldc_I4_5
  | Ldc_I4_6
  | Ldc_I4_7
  | Ldc_I4_8
  | Ldc_I4_S of int
  | Ldc_I4 of int
  | Ldc_R8 of float
  | Stelem of Type
  | Ldtoken of Token
  | Tailcall
  | Call of CallTarget
  | Callvirt of CallTarget
  | Newarr of Type
  | Newobj of ConstructorInfo
  | Initobj of Type
  | Dup
  | Pop
  | Ret

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ILOpCode =
  let stloc local _nameOpt =
    #if DEVELOPMENT
    Stloc (local, Some _nameOpt)
    #else
    Stloc (local, None)
    #endif

  let ldloc local _nameOpt =
    #if DEVELOPMENT
    Ldloc (local, Some _nameOpt)
    #else
    Ldloc (local, None)
    #endif

  let ldloca local _nameOpt =
    #if DEVELOPMENT
    Ldloca (local, Some _nameOpt)
    #else
    Ldloca (local, None)
    #endif

  let toRawOpCode = function
  | And -> OpCodes.And
  | Or -> OpCodes.Or
  | Xor -> OpCodes.Xor
  | Not -> OpCodes.Not
  | Shl -> OpCodes.Shl
  | Shr -> OpCodes.Shr
  | Div -> OpCodes.Div
  | Mul -> OpCodes.Mul
  | Mul_Ovf -> OpCodes.Mul_Ovf
  | Neg -> OpCodes.Neg
  | Rem -> OpCodes.Rem
  | Add -> OpCodes.Add
  | Sub -> OpCodes.Sub
  | Sub_Ovf -> OpCodes.Sub_Ovf
  | Brfalse _ -> OpCodes.Brfalse
  | Br _ -> OpCodes.Br
  | Leave _ -> OpCodes.Leave
  | Endfinally -> OpCodes.Endfinally
  | Isinst _ -> OpCodes.Isinst
  | Conv_I -> OpCodes.Conv_I
  | Conv_I1 -> OpCodes.Conv_I1
  | Conv_I2 -> OpCodes.Conv_I2
  | Conv_I4 -> OpCodes.Conv_I4
  | Conv_I8 -> OpCodes.Conv_I8
  | Conv_U -> OpCodes.Conv_U
  | Conv_U1 -> OpCodes.Conv_U1
  | Conv_U2 -> OpCodes.Conv_U2
  | Conv_U4 -> OpCodes.Conv_U4
  | Conv_U8 -> OpCodes.Conv_U8
  | Conv_R4 -> OpCodes.Conv_R4
  | Conv_R8 -> OpCodes.Conv_R8
  | Conv_Ovf_I -> OpCodes.Conv_Ovf_I
  | Conv_Ovf_I1 -> OpCodes.Conv_Ovf_I1
  | Conv_Ovf_I2 -> OpCodes.Conv_Ovf_I2
  | Conv_Ovf_I4 -> OpCodes.Conv_Ovf_I4
  | Conv_Ovf_I8 -> OpCodes.Conv_Ovf_I8
  | Conv_Ovf_U -> OpCodes.Conv_Ovf_U
  | Conv_Ovf_U1 -> OpCodes.Conv_Ovf_U1
  | Conv_Ovf_U2 -> OpCodes.Conv_Ovf_U2
  | Conv_Ovf_U4 -> OpCodes.Conv_Ovf_U4
  | Conv_Ovf_U8 -> OpCodes.Conv_Ovf_U8
  | Box _ -> OpCodes.Box
  | Unbox_Any _ -> OpCodes.Unbox_Any
  | Stloc _ -> OpCodes.Stloc
  | Ldloc _ -> OpCodes.Ldloc
  | Ldloca _ -> OpCodes.Ldloca
  | Starg _ -> OpCodes.Starg
  | Ldarg_0 -> OpCodes.Ldarg_0
  | Ldarg_1 -> OpCodes.Ldarg_1
  | Ldarg_2 -> OpCodes.Ldarg_2
  | Ldarg_3 -> OpCodes.Ldarg_3
  | Ldarg _ -> OpCodes.Ldarg
  | Ldsfld _ -> OpCodes.Ldsfld
  | Stfld _ -> OpCodes.Stfld
  | Ldfld _ -> OpCodes.Ldfld
  | Ldnull -> OpCodes.Ldnull
  | Ldstr _ -> OpCodes.Ldstr
  | Ldc_I4_M1 -> OpCodes.Ldc_I4_M1
  | Ldc_I4_0 -> OpCodes.Ldc_I4_0
  | Ldc_I4_1 -> OpCodes.Ldc_I4_1
  | Ldc_I4_2 -> OpCodes.Ldc_I4_2
  | Ldc_I4_3 -> OpCodes.Ldc_I4_3
  | Ldc_I4_4 -> OpCodes.Ldc_I4_4
  | Ldc_I4_5 -> OpCodes.Ldc_I4_5
  | Ldc_I4_6 -> OpCodes.Ldc_I4_6
  | Ldc_I4_7 -> OpCodes.Ldc_I4_7
  | Ldc_I4_8 -> OpCodes.Ldc_I4_8
  | Ldc_I4_S _ -> OpCodes.Ldc_I4_S
  | Ldc_I4 _ -> OpCodes.Ldc_I4
  | Ldc_R8 _ -> OpCodes.Ldc_R8
  | Stelem _ -> OpCodes.Stelem
  | Ldtoken _ -> OpCodes.Ldtoken
  | Tailcall -> OpCodes.Tailcall
  | Call _ -> OpCodes.Call
  | Callvirt _ -> OpCodes.Callvirt
  | Newarr _ -> OpCodes.Newarr
  | Newobj _ -> OpCodes.Newobj
  | Initobj _ -> OpCodes.Initobj
  | Dup -> OpCodes.Dup
  | Pop -> OpCodes.Pop
  | Ret -> OpCodes.Ret