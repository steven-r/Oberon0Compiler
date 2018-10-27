#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Code.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil/Code.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;

    using JetBrains.Annotations;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Types;

    public class Code : StringWriter
    {
        private int labelId;

        public Code(StringBuilder sb)
            : base(sb)
        {
        }

        internal string ClassName { get; private set; }

        // ReSharper disable once MemberCanBePrivate.Global
        internal string ModuleName { [UsedImplicitly] get; set; }

        public void Branch(string branchType, string label)
        {
            Emit(branchType, label);
        }

        public void ConstField(ConstDeclaration constDeclaration)
        {
            WriteLine(
                $".data {constDeclaration.Name} = {GetDataTypeName(constDeclaration.Type)} ({DumpConstValue(constDeclaration.Value, false, true)})");
        }

        public void DataField(Declaration declaration, bool isStatic)
        {
            WriteLine(
                $".field {(isStatic ? "static " : string.Empty)}{GetTypeName(declaration.Type)} {declaration.Name}");
        }

        /// <summary>
        /// Emits the ldelem.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="arrayTypeDefinition">The array type definition.</param>
        public void EmitLdelem(IndexSelector index, ArrayTypeDefinition arrayTypeDefinition)
        {
            string suffix = string.Empty;
            string param = null;
            switch (arrayTypeDefinition.BaseType)
            {
                case BaseType.IntType:
                case BaseType.BoolType:
                    suffix = ".i4";
                    break;
                case BaseType.DecimalType:
                    suffix = ".r8";
                    break;
                default:
                    param = GetTypeName(arrayTypeDefinition.ArrayType);
                    break;
            }

            Emit("ldelem" + suffix, param);
        }

        internal static string DotNumOrArg(int value, int min, int max, bool isSimpleShortForm = true)
        {
            if (value < min || value > max)
            {
                if (value <= 255 && isSimpleShortForm)
                    return $".s {value}";
                return $" {value}";
            }

            if (value < 0)
                return $".m{-value}";
            return $".{value}";
        }

        internal static string GetTypeName(BaseType type)
        {
            switch (type)
            {
                case BaseType.IntType:
                    return "int32";
                case BaseType.StringType:
                    return "string";
                case BaseType.DecimalType:
                    return "float64";
                case BaseType.BoolType:
                    return "bool";
                case BaseType.VoidType:
                    return "void";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), "Unsupported type");
            }
        }

        internal void Call(FunctionDeclaration func)
        {
            if (func.IsInternal)
            {
                throw new NotImplementedException();
            }

            string prototype = func.Prototype;
            if (!func.IsExternal)
            {
                prototype = $"Oberon0.{ModuleName}::{func.Name}";
            }

            // local procedure
            EmitNoNewLine("call", GetTypeName(func.ReturnType), $"{prototype}(");
            List<string> typeNames = new List<string>();
            typeNames.AddRange(
                func.Block.Declarations.OfType<ProcedureParameter>().Select(parameter => GetTypeName(parameter.Type)));
            Write(string.Join(",", typeNames));
            WriteLine(")");
        }

        internal void Emit(string opCode, params object[] parameters)
        {
            EmitNoNewLine(opCode, parameters);
            WriteLine();
        }

        internal void EmitComment(string comment)
        {
            WriteLine("// " + comment);
        }

        internal string EmitLabel(string label = null)
        {
            label = label ?? GetLabel();
            Write(label + ": ");
            return label;
        }

        internal void EmitStelem(IndexSelector indexSelector)
        {
            string suffix;
            string param = null;
            switch (indexSelector.IndexDefinition.TargetType.BaseType)
            {
                case BaseType.IntType:
                case BaseType.BoolType:
                    suffix = ".i4";
                    break;
                case BaseType.DecimalType:
                    suffix = ".r8";
                    break;
                default:
                    suffix = ".ref";
                    param = GetTypeName(indexSelector.IndexDefinition.TargetType);
                    break;
            }

            Emit("stelem" + suffix, param);
        }

        internal void EmitStfld(IdentifierSelector identSelector)
        {
            Emit(
                "stfld",
                GetTypeName(identSelector.Element.Type),
                GetTypeName(identSelector.Type) + "::" + identSelector.Name);
        }

        internal void EndClass()
        {
            WriteLine("}");
        }

        internal void EndMethod()
        {
            Emit("ret");
            WriteLine("}");
        }

        internal string GetLabel()
        {
            return $"L{this.labelId++}";
        }

        internal string GetTypeName(TypeDefinition type)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (type.BaseType)
            {
                case BaseType.ComplexType:
                    if (type is ArrayTypeDefinition arrayTypeDefinition)
                    {
                        return $"{GetTypeName(arrayTypeDefinition.ArrayType)}[]";
                    }

                    if (type is RecordTypeDefinition)
                    {
                        return $"class {ClassName}/{type.Name}";
                    }

                    throw new ArgumentException("Array or record type is required", nameof(type));
                default:
                    return GetTypeName(type.BaseType);
            }
        }

        internal void LoadConstRef(ConstDeclaration constDeclaration)
        {
            Emit("ldvar", constDeclaration.Name);
        }

        internal void LocalVarDef(Declaration declaration, bool isPointer)
        {
            Write($"{(isPointer ? "&" : string.Empty)}{GetTypeName(declaration.Type)} {declaration.Name}");
        }

        internal void PushConst([NotNull] object data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            switch (Type.GetTypeCode(data.GetType()))
            {
                case TypeCode.Boolean:
                    Emit("ldc.i4." + ((bool)data ? "1" : "0"));
                    break;
                case TypeCode.Char:
                    Emit("ldc.i4.s ", Convert.ToByte((char)data).ToString("x2"));
                    break;
                case TypeCode.Int16:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                    Emit("ldc.i2", data);
                    break;
                case TypeCode.Byte:
                    Emit("ldc.i1.s", data);
                    break;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    Emit("ldc.i4" + DotNumOrArg((int)data, -1, 8));
                    break;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    Emit("ldc.i8 ", data);
                    break;
                case TypeCode.Single:
                    Emit("ldc.r4", data);
                    break;
                case TypeCode.Decimal:
                    Emit("ldc.r8", ((decimal)data).ToString(CultureInfo.InvariantCulture));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal void Reference(string assemblyName)
        {
            WriteLine($".assembly extern {assemblyName} {{ }}");
        }

        internal void StartAssembly(string moduleName)
        {
            WriteLine($".assembly {moduleName} {{ }}");
        }

        internal void StartClass(string className)
        {
            WriteLine($".class public {className} {{");
            ClassName = className;
        }

        internal void StartMainMethod()
        {
            Write(
                $@"
.method static public void $O0$main() cil managed
{{   .entrypoint 
");
        }

        internal void StartMethod(FunctionDeclaration functionDeclaration)
        {
            Write($".method private static {GetTypeName(functionDeclaration.ReturnType)} {functionDeclaration.Name}(");
            List<string> paramList = new List<string>();
            int id = 0;
            foreach (ProcedureParameter parameter in functionDeclaration.Block.Declarations.OfType<ProcedureParameter>())
            {
                parameter.GeneratorInfo = new DeclarationGeneratorInfo(id++);
                paramList.Add($"{GetTypeName(parameter.Type)} {parameter.Name}");
            }

            WriteLine(
                string.Join(", ", paramList) + @")
{");
        }

        internal void StartModule(string moduleName)
        {
            WriteLine($".module {moduleName}.exe");
            ModuleName = moduleName;
        }

        private static string DumpConstValue(
            ConstantExpression constantExpression,
            bool isLoad = false,
            bool isData = false)
        {
            switch (constantExpression.TargetType.BaseType)
            {
                case BaseType.IntType:
                    return constantExpression.ToInt32().ToString();
                case BaseType.DecimalType:
                    return constantExpression.ToDouble().ToString("G");
                case BaseType.BoolType:
                    if (isLoad || isData)
                        return constantExpression.ToBool() ? "1" : "0";
                    return constantExpression.ToBool().ToString().ToLower(CultureInfo.InvariantCulture);
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(constantExpression.TargetType.BaseType),
                        "Invalid type");
            }
        }

        private static string GetDataTypeName(TypeDefinition type)
        {
            switch (type.BaseType)
            {
                case BaseType.BoolType:
                case BaseType.IntType:
                    return "int32";
                case BaseType.StringType:
                    return "string";
                case BaseType.DecimalType:
                    return "float64";
                case BaseType.VoidType:
                    return "void";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type.BaseType), "Invalid type");
            }
        }

        private void EmitNoNewLine(string opCode, params object[] parameters)
        {
            Write("\t" + opCode);
            foreach (string parameter in parameters)
            {
                if (!string.IsNullOrEmpty(parameter))
                    Write("\t" + parameter);
            }
        }
    }
}