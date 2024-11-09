#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;
using Oberon0.Compiler.Exceptions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Types;
using Oberon0System.Attributes;

namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    ///     The module.
    /// </summary>
    public partial class Module
    {
        /**
         * <summary>
         *     List of internal defined functions
         * </summary>
         */
        private readonly HardwiredFunction[] _hardwiredFunctions =
        [
            new()
            {
                Name = "ABS", Type = TypeDefinition.IntegerTypeName,
                ParameterTypes = [TypeDefinition.IntegerTypeName]
            },
            new()
            {
                Name = "ABS", Type = TypeDefinition.RealTypeName,
                ParameterTypes = [TypeDefinition.RealTypeName]
            },
            new()
            {
                Name = "WriteInt",
                ParameterTypes = [TypeDefinition.IntegerTypeName]
            },
            new()
            {
                Name = "WriteBool",
                ParameterTypes = [TypeDefinition.BooleanTypeName]
            },
            new()
            {
                Name = "WriteString",
                ParameterTypes = [TypeDefinition.StringTypeName]
            },
            new()
            {
                Name = "WriteReal",
                ParameterTypes = [TypeDefinition.RealTypeName]
            },
            new()
            {
                Name = "WriteLn",
                ParameterTypes = []
            },
            new()
            {
                Name = "ReadInt",
                ParameterTypes = ["&INTEGER"]
            },
            new()
            {
                Name = "ReadReal",
                ParameterTypes = ["&REAL"]
            },
            new()
            {
                Name = "ReadBool",
                ParameterTypes = ["&BOOLEAN"]
            }
        ];

        private void AddParameters(Oberon0ExportAttribute attr, int i,
                                   IList<ProcedureParameterDeclaration> procParameters)
        {
            string paramName = "attr" + i;
            procParameters[i] = GetProcedureParameterByName(paramName, attr.Parameters[i], Block);
        }

        /// <summary>
        ///     Get a <see cref="ProcedureParameterDeclaration" /> by passing name, type and surrounding block information
        /// </summary>
        /// <param name="parameterName">The name of the parameter</param>
        /// <param name="parameterType">The type in textual form (see remarks)</param>
        /// <param name="block">The block this parameter will be part of</param>
        /// <returns>A <see cref="ProcedureParameterDeclaration" /> element</returns>
        /// <remarks>
        ///     <see cref="parameterType" /> can hold a simple type name. If you want to create a reference type, pretend the
        ///     type-name with either <code>%amp;</code> or <code>VAR </code>
        /// </remarks>
        public static ProcedureParameterDeclaration GetProcedureParameterByName(
            string parameterName, string parameterType, Block block)
        {
            ArgumentException.ThrowIfNullOrEmpty(parameterName);
            ArgumentNullException.ThrowIfNull(parameterType);
            ArgumentNullException.ThrowIfNull(block);

            (var targetType, bool isVar) = GetParameterDeclarationFromString(parameterType, block);
            return new ProcedureParameterDeclaration(parameterName, block, targetType, isVar);
        }

        [GeneratedRegex(@"(?<ref>&|VAR\s+)?(?<name>[A-Za-z][A-Za-z$0-9]*)(?<isarray>\[(?<size>\d+)\])?")]
        internal static partial Regex ParameterDeclarationRegex();

        /// <summary>
        ///     Parses a type spec (e.g. "INTEGER", "&amp;REAL" or "BOOLEAN[10]"
        /// </summary>
        /// <param name="typeString"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        internal static (TypeDefinition, bool) GetParameterDeclarationFromString(
            string typeString, Block block)
        {
            ArgumentException.ThrowIfNullOrEmpty(typeString);
            ArgumentNullException.ThrowIfNull(block);

            var matches = ParameterDeclarationRegex().Match(typeString);
            if (!matches.Success)
            {
                throw new ArgumentException($"{typeString} is not a valid type reference", nameof(typeString));
            }

            string typeName = matches.Groups["name"].Value;
            bool isVar = matches.Groups["ref"].Success;

            var type = block.LookupType(typeName) ?? throw new InvalidOperationException($"{typeString} is not a valid type reference");

            var targetType = matches.Groups["isarray"].Success
                ? new ArrayTypeDefinition(int.Parse(matches.Groups["size"].Value), type)
                : type;
            return (targetType, isVar);
        }

        private void DeclareStandardConstants()
        {
            Block.Declarations.Add(
                new ConstDeclaration("TRUE", Block.LookupType(TypeDefinition.BooleanTypeName)!, 
                    new ConstantBoolExpression(true) { Internal = true }));
            Block.Declarations.Add(
                new ConstDeclaration("FALSE", Block.LookupType(TypeDefinition.BooleanTypeName)!, 
                    new ConstantBoolExpression(false) { Internal = true }));
            Block.Declarations.Add(
                new ConstDeclaration("EPSILON", Block.LookupType(TypeDefinition.RealTypeName)!,
                    new ConstantDoubleExpression(double.Epsilon) { Locked = true }));
        }

        private void DeclareStandardFunctions()
        {
            foreach (var function in _hardwiredFunctions)
            {
                var type = Block.LookupType(function.Type)!;
                Block.Procedures.Add(FunctionDeclaration.AddHardwiredFunction(function.Name, this, type, 
                    function.ParameterTypes));
            }

            var sysAsm = Assembly.Load("Oberon0.System");

            foreach (var type in sysAsm.GetExportedTypes())
            {
                if (type.GetCustomAttribute<Oberon0LibraryAttribute>() != null)
                {
                    LoadLibraryMembers(type);
                }
            }
        }

        private void DeclareStandardTypes()
        {
            SimpleTypeDefinition.IntType = new SimpleTypeDefinition(BaseTypes.Int, TypeDefinition.IntegerTypeName, true);
            SimpleTypeDefinition.BoolType = new SimpleTypeDefinition(BaseTypes.Bool, TypeDefinition.BooleanTypeName, true);
            SimpleTypeDefinition.RealType = new SimpleTypeDefinition(BaseTypes.Real, TypeDefinition.RealTypeName, true);
            SimpleTypeDefinition.StringType = new SimpleTypeDefinition(BaseTypes.String, TypeDefinition.StringTypeName, true);
            SimpleTypeDefinition.VoidType = new SimpleTypeDefinition(
                BaseTypes.Void,
                TypeDefinition.VoidTypeName,
                true);
            Block.Types.Add(SimpleTypeDefinition.IntType);
            Block.Types.Add(SimpleTypeDefinition.BoolType);
            Block.Types.Add(SimpleTypeDefinition.RealType);
            Block.Types.Add(SimpleTypeDefinition.StringType);
            Block.Types.Add(SimpleTypeDefinition.VoidType);
        }

        /// <summary>
        ///     load all members with that are exported through <see cref="Oberon0ExportAttribute" />
        /// </summary>
        /// <param name="type">The type.</param>
        private void LoadLibraryMembers(Type type)
        {
            foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                var attr = method.GetCustomAttribute<Oberon0ExportAttribute>();
                if (attr == null)
                {
                    continue; // this method is not exported
                }

                if (method.DeclaringType == null)
                {
                    throw new InternalCompilerException($"method.DeclaringType == null for {method.Module.Name}/{method.Name}");
                }
                
                Block.Procedures.Add(AddExternalFunctionDeclaration(attr, method.DeclaringType.FullName!, method.Name));
            }

            ExternalReferences.Add(type.Assembly);
        }

        /// <summary>
        ///     Add external function declaration
        /// </summary>
        /// <param name="attr">The <see cref="Oberon0ExportAttribute" /> structure</param>
        /// <param name="className">The providing class</param>
        /// <param name="methodName">The method name</param>
        /// <returns>an <see cref="ExternalFunctionDeclaration " /> representing the function</returns>
        // ReSharper disable once MemberCanBePrivate.Global
        internal ExternalFunctionDeclaration AddExternalFunctionDeclaration(Oberon0ExportAttribute attr,
                                                                            string className,
                                                                            string methodName)
        {
            ArgumentException.ThrowIfNullOrEmpty(className);
            ArgumentNullException.ThrowIfNull(attr);
            ArgumentException.ThrowIfNullOrEmpty(methodName);

            var rt = Block.LookupType(attr.ReturnType) ?? throw new InvalidOperationException($"Return type {attr.ReturnType} not defined");

            var procParameters = new ProcedureParameterDeclaration[attr.Parameters.Length];
            for (int i = 0; i < attr.Parameters.Length; i++)
            {
                AddParameters(attr, i, procParameters);
            }

            return new ExternalFunctionDeclaration(attr.Name, new Block(Block, this), rt,
                className, methodName, procParameters);
        }

        private sealed class HardwiredFunction
        {
            public required string Name { get; init; }

            public string Type { get; init; } = TypeDefinition.VoidTypeName;

            public required string[] ParameterTypes { get; init; } = [];
        }
    }
}
