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
                $".data __{constDeclaration.Name} = {GetDataTypeName(constDeclaration.Type)} ({DumpConstValue(constDeclaration.Value, false, true)})");
        }

        public void DataField(Declaration declaration, bool isStatic)
        {
            WriteLine(
                $".field {(isStatic ? "static " : string.Empty)}{GetTypeName(declaration.Type)} __{declaration.Name}");
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
            switch (arrayTypeDefinition.Type)
            {
                case BaseTypes.Int:
                case BaseTypes.Bool:
                    suffix = ".i4";
                    break;
                case BaseTypes.Real:
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

        internal static string GetTypeName(BaseTypes type)
        {
            switch (type)
            {
                case BaseTypes.Int:
                    return "int32";
                case BaseTypes.String:
                    return "string";
                case BaseTypes.Real:
                    return "float64";
                case BaseTypes.Bool:
                    return "bool";
                case BaseTypes.Void:
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
                prototype = $"Oberon0.{ModuleName}::__{func.Name}";
            }

            // local procedure
            EmitNoNewLine("call", GetTypeName(func.ReturnType), $"{prototype}(");
            List<string> typeNames = new List<string>();
            typeNames.AddRange(
                func.Block.Declarations.OfType<ProcedureParameter>().Select(parameter => GetTypeName(parameter.Type) + (parameter.IsVar ? "&" : string.Empty)));
            Write(string.Join(",", typeNames));
            WriteLine(")");
        }

        internal void Emit(string code, params object[] parameters)
        {
            EmitNoNewLine(code, parameters);
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
            switch (indexSelector.IndexDefinition.TargetType.Type)
            {
                case BaseTypes.Int:
                case BaseTypes.Bool:
                    suffix = ".i4";
                    break;
                case BaseTypes.Real:
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
                GetTypeName(identSelector.Type) + "::__" + identSelector.Name);
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
            return $"L{labelId++}";
        }

        internal string GetTypeName(TypeDefinition type)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (type.Type)
            {
                case BaseTypes.Array:
                    return $"{GetTypeName(((ArrayTypeDefinition)type).ArrayType)}[]";
                case BaseTypes.Record:
                    return $"class {ClassName}/__{type.Name}";
                default:
                    return GetTypeName(type.Type);
            }
        }

        internal void LoadConstRef(ConstDeclaration constDeclaration)
        {
            Emit("ldvar", $"__{constDeclaration.Name}");
        }

        internal void LocalVarDef(Declaration declaration, bool isPointer)
        {
            Write($"{(isPointer ? "&" : string.Empty)}{GetTypeName(declaration.Type)} __{declaration.Name}");
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
                case TypeCode.Double:
                    Emit("ldc.r8", ((double)data).ToString(CultureInfo.InvariantCulture));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(data), "Unknown data type");
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
            Write($".method private static {GetTypeName(functionDeclaration.ReturnType)} __{functionDeclaration.Name}(");
            List<string> paramList = new List<string>();
            int id = 0;
            foreach (ProcedureParameter parameter in functionDeclaration.Block.Declarations.OfType<ProcedureParameter>())
            {
                parameter.GeneratorInfo = new DeclarationGeneratorInfo(id++);
                paramList.Add($"{GetTypeName(parameter.Type)}{(parameter.IsVar ? "&" : string.Empty)} __{parameter.Name}");
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
            switch (constantExpression.TargetType.Type)
            {
                case BaseTypes.Int:
                    return constantExpression.ToInt32().ToString(CultureInfo.InvariantCulture);
                case BaseTypes.Real:
                    return constantExpression.ToDouble().ToString("G", CultureInfo.InvariantCulture);
                case BaseTypes.Bool:
                    if (isLoad || isData)
                        return constantExpression.ToBool() ? "1" : "0";
                    return constantExpression.ToBool().ToString().ToLower(CultureInfo.InvariantCulture);
                default:
                    throw new ArgumentOutOfRangeException(nameof(constantExpression), "Invalid type");
            }
        }

        private static string GetDataTypeName(TypeDefinition type)
        {
            switch (type.Type)
            {
                case BaseTypes.Bool:
                case BaseTypes.Int:
                    return "int32";
                case BaseTypes.String:
                    return "string";
                case BaseTypes.Real:
                    return "float64";
                case BaseTypes.Void:
                    return "void";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), "Invalid type");
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