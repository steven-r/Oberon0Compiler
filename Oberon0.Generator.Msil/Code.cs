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
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Types;
    using Oberon0.Generator.Msil.PredefinedFunctions;

    /// <summary>
    /// The code generation basics.
    /// </summary>
    public partial class Code : StringWriter
    {
        private int labelId;

        /// <summary>
        /// Initializes a new instance of the <see cref="Code"/> class.
        /// </summary>
        /// <param name="sb">
        /// The String builder that builds the string.
        /// </param>
        public Code(StringBuilder sb)
            : base(sb)
        {
        }

        /// <summary>
        /// Gets the class name that is also the name of the module/application.
        /// </summary>
        internal string ClassName { get; private set; }

        // ReSharper disable once MemberCanBePrivate.Global
        internal string ModuleName { [UsedImplicitly] get; set; }

        public static void CallInternalFunction(
            CodeGenerator generator,
            FunctionDeclaration call,
            Block block,
            IReadOnlyList<Expression> expressions)
        {
            var function = StandardFunctionRepository.Get(call);
            function.Instance.Generate(function, generator, call, expressions, block);
        }

        public void Branch(string branchType, string label)
        {
            this.Emit(branchType, label);
        }

        public void ConstField(ConstDeclaration constDeclaration)
        {
            this.WriteLine(
                $".data {(constDeclaration.Exportable ? "export " : string.Empty)} {MakeName(constDeclaration.Name)} = {GetDataTypeName(constDeclaration.Type)} ({DumpConstValue(constDeclaration.Value, false, true)})");
        }

        /// <summary>
        /// Create a data field
        /// </summary>
        /// <param name="declaration">
        /// The declaration.
        /// </param>
        /// <param name="isStatic">
        /// The is static.
        /// </param>
        public void DataField(Declaration declaration, bool isStatic)
        {
            this.WriteLine(
                $".field {(declaration.Exportable ? "export " : string.Empty)}static {this.GetTypeName(declaration.Type)} {MakeName(declaration.Name)}");
        }

        /// <summary>
        /// Emits the "ldelem" call.
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
                    param = this.GetTypeName(arrayTypeDefinition.ArrayType);
                    break;
            }

            this.Emit("ldelem" + suffix, param);
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

        internal static string GetIndirectSuffix(TypeDefinition type)
        {
            string suffix;
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (type.Type)
            {
                case BaseTypes.Int:
                case BaseTypes.Bool:
                    suffix = ".i4";
                    break;
                case BaseTypes.Real:
                    suffix = ".r8";
                    break;
                case BaseTypes.Array:
                    suffix = GetIndirectSuffix(((ArrayTypeDefinition)type).ArrayType);
                    break;
                case BaseTypes.Record:
                    suffix = ".ref";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return suffix;
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

        internal void Call(CodeGenerator generator, FunctionDeclaration func, IReadOnlyList<Expression> expressions)
        {
            if (func.IsInternal)
            {
                CallInternalFunction(generator, func, func.Block, expressions);
                return;
            }

            string prototype = this.GetPrototypeName(func);

            var parameters = func.Block.Declarations.OfType<ProcedureParameterDeclaration>().ToList();

            // local procedure
            this.EmitNoNewLine("call", this.GetTypeName(func.ReturnType), $"{prototype}(");
            List<string> typeNames = new List<string>();
            typeNames.AddRange(
                parameters.Select(
                    parameter => this.GetTypeName(parameter.Type)
                                 + (!parameter.Type.Type.HasFlag(BaseTypes.Complex) && parameter.IsVar
                                        ? "&"
                                        : string.Empty)));
            this.Write(string.Join(",", typeNames));
            this.WriteLine(")");
        }

        internal void Emit(string code, params object[] parameters)
        {
            this.EmitNoNewLine(code, parameters);
            this.WriteLine();
        }

        internal void EmitComment(string comment)
        {
            this.WriteLine("// " + comment);
        }

        internal string EmitLabel(string label = null)
        {
            label = label ?? this.GetLabel();
            this.Write(label + ": ");
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
                    param = this.GetTypeName(indexSelector.IndexDefinition.TargetType);
                    break;
            }

            this.Emit("stelem" + suffix, param);
        }

        internal void EmitStfld(IdentifierSelector identSelector)
        {
            this.Emit(
                "stfld",
                this.GetTypeName(identSelector.Element.Type),
                $"{this.GetTypeName(identSelector.BasicTypeDefinition)}::{MakeName(identSelector.Name)}");
        }

        internal void EndClass()
        {
            this.WriteLine("}");
        }

        internal void EndMethod()
        {
            this.Emit("ret");
            this.WriteLine("}");
        }

        internal string GetLabel()
        {
            return $"L{this.labelId++}";
        }

        internal string GetTypeName(TypeDefinition type)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases
            switch (type.Type)
            {
                case BaseTypes.Array:
                    return $"{this.GetTypeName(((ArrayTypeDefinition)type).ArrayType)}[]";
                case BaseTypes.Record:
                    return $"class {this.ClassName}/{MakeName(type.Name)}";
                default:
                    return GetTypeName(type.Type);
            }
        }

        internal void LoadConstRef(ConstDeclaration constDeclaration)
        {
            this.Emit("ldvar", $"{MakeName(constDeclaration.Name)}");
        }

        internal void LocalVarDef(Declaration declaration, bool isPointer)
        {
            this.Write($"{(isPointer ? "&" : string.Empty)}{this.GetTypeName(declaration.Type)} {MakeName(declaration.Name)}");
        }

        internal void PushConst([NotNull] object data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            switch (Type.GetTypeCode(data.GetType()))
            {
                case TypeCode.Boolean:
                    this.Emit("ldc.i4." + ((bool)data ? "1" : "0"));
                    break;
                case TypeCode.Char:
                    this.Emit("ldc.i4.s ", Convert.ToByte((char)data).ToString("x2"));
                    break;
                case TypeCode.Int16:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                    this.Emit("ldc.i2", data);
                    break;
                case TypeCode.Byte:
                    this.Emit("ldc.i1.s", data);
                    break;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    this.Emit("ldc.i4" + DotNumOrArg((int)data, -1, 8));
                    break;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    this.Emit("ldc.i8 ", data);
                    break;
                case TypeCode.Single:
                    this.Emit("ldc.r4", data);
                    break;
                case TypeCode.Double:
                    this.Emit("ldc.r8", ((double)data).ToString(CultureInfo.InvariantCulture));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(data), "Unknown data type");
            }
        }

        internal void Reference(string assemblyName)
        {
            this.WriteLine($".assembly extern {assemblyName} {{ }}");
        }

        internal void StartAssembly(string moduleName)
        {
            this.WriteLine($".assembly {moduleName} {{ }}");
        }

        internal void StartClass(string className)
        {
            this.WriteLine($".class public {className} {{");
            this.ClassName = className;
        }

        /// <summary>
        /// create the main method.
        /// </summary>
        /// <param name="module">
        /// The module.
        /// </param>
        internal void MainMethod(Module module)
        {
            if (!module.HasExports)
            {
                this.Write(
                    @"
.method static public void $O0$main() cil managed
{   
    .entrypoint 
");
                this.Emit("call", GetTypeName(BaseTypes.Void), $"Oberon0.{this.ModuleName}::$init ()");

                this.WriteLine("\tret");
                this.WriteLine("}");
            }
        }

        internal void StartMethod(FunctionDeclaration functionDeclaration)
        {
            this.Write(
                $".method {(functionDeclaration.Exportable ? "extern " : "private ")}static {this.GetTypeName(functionDeclaration.ReturnType)} {MakeName(functionDeclaration.Name)}(");
            List<string> paramList = new List<string>();
            foreach (ProcedureParameterDeclaration parameter in functionDeclaration.Block.Declarations
                .OfType<ProcedureParameterDeclaration>())
            {
                var isVar = parameter.IsVar;
                var name = parameter.Name;
                if (!parameter.IsVar && parameter.Type.Type.HasFlag(BaseTypes.Complex))
                {
                    name = $"param__{name}";
                }
                else
                {
                    isVar = isVar && !parameter.Type.Type.HasFlag(BaseTypes.Complex);
                }

                paramList.Add($"{this.GetTypeName(parameter.Type)}{(isVar ? "&" : string.Empty)} {MakeName(name)}");
            }

            this.WriteLine(
                string.Join(", ", paramList) + @")
{");
        }

        internal void StartModule(string moduleName)
        {
            this.WriteLine($".module {moduleName}.exe");
            this.ModuleName = moduleName;
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

        private void EmitNoNewLine(string code, params object[] parameters)
        {
            this.Write("\t" + code);
            foreach (string parameter in parameters)
            {
                if (!string.IsNullOrEmpty(parameter))
                    this.Write("\t" + parameter);
            }
        }

        private string GetPrototypeName(FunctionDeclaration func)
        {
            if (func is ExternalFunctionDeclaration exFunc)
            {
                return $"[{exFunc.Assembly.GetName().Name}]{exFunc.ClassName}::{exFunc.MethodName}";
            }

            return $"Oberon0.{this.ModuleName}::{MakeName(func.Name)}";
        }
    }
}