﻿#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using Oberon0.Compiler.Expressions;
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
        {
            new HardwiredFunction
            {
                Name = "ABS", Type = "INTEGER",
                ParameterTypes = new[] {"INTEGER"}
            },
            new HardwiredFunction
            {
                Name = "ABS", Type = "REAL",
                ParameterTypes = new[] {"REAL"}
            },
            new HardwiredFunction
            {
                Name = "WriteInt", Type = null,
                ParameterTypes = new[] {"INTEGER"}
            },
            new HardwiredFunction
            {
                Name = "WriteBool", Type = null,
                ParameterTypes = new[] {"BOOLEAN"}
            },
            new HardwiredFunction
            {
                Name = "WriteString", Type = null,
                ParameterTypes = new[] {"STRING"}
            },
            new HardwiredFunction
            {
                Name = "WriteReal", Type = null,
                ParameterTypes = new[] {"REAL"}
            },
            new HardwiredFunction
            {
                Name = "WriteLn", Type = null
            },
            new HardwiredFunction
            {
                Name = "ReadInt", Type = null,
                ParameterTypes = new[] {"&INTEGER"}
            },
            new HardwiredFunction
            {
                Name = "ReadReal", Type = null,
                ParameterTypes = new[] {"&REAL"}
            },
            new HardwiredFunction
            {
                Name = "ReadBool", Type = null,
                ParameterTypes = new[] {"&BOOLEAN"}
            }
        };

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
            [NotNull] string parameterName, [NotNull] string parameterType, [NotNull] Block block)
        {
            if (parameterName == null)
            {
                throw new ArgumentNullException(nameof(parameterName));
            }

            if (parameterType == null)
            {
                throw new ArgumentNullException(nameof(parameterType));
            }

            if (block == null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            (var targetType, bool isVar) = GetParameterDeclarationFromString(parameterType, block);
            return new ProcedureParameterDeclaration(parameterName, block, targetType, isVar);
        }

        /// <summary>
        ///     Parses a type spec (e.g. "INTEGER", "&amp;REAL" or "BOOLEAN[10]"
        /// </summary>
        /// <param name="typeString"></param>
        /// <param name="block"></param>
        /// <returns></returns>
        internal static (TypeDefinition, bool) GetParameterDeclarationFromString(
            [NotNull] string typeString, [NotNull] Block block)
        {
            if (typeString == null)
            {
                throw new ArgumentNullException(nameof(typeString));
            }

            if (block == null)
            {
                throw new ArgumentNullException(nameof(block));
            }

            var matches = Regex.Match(
                typeString,
                "(?<ref>&|VAR\\s+)?(?<name>[A-Za-z][A-Za-z$0-9]*)(?<isarray>\\[(?<size>\\d+)\\])?");
            if (!matches.Success)
            {
                throw new ArgumentException($"{typeString} is not a valid type reference", nameof(typeString));
            }

            string typeName = matches.Groups["name"].Value;
            bool isVar = matches.Groups["ref"].Success;

            var type = block.LookupType(typeName);
            if (type == null)
            {
                throw new ArgumentException($"{typeString} is not a valid type reference", nameof(typeString));
            }

            var targetType = matches.Groups["isarray"].Success
                ? new ArrayTypeDefinition(int.Parse(matches.Groups["size"].Value), type)
                : type;
            return (targetType, isVar);
        }

        private void DeclareStandardConstants()
        {
            Block.Declarations.Add(
                new ConstDeclaration("TRUE", Block.LookupType("BOOLEAN"), new ConstantBoolExpression(true)));
            Block.Declarations.Add(
                new ConstDeclaration("FALSE", Block.LookupType("BOOLEAN"), new ConstantBoolExpression(false)));
            Block.Declarations.Add(
                new ConstDeclaration("EPSILON", Block.LookupType("REAL"),
                    new ConstantDoubleExpression(double.Epsilon)));
        }

        private void DeclareStandardFunctions()
        {
            foreach (var function in _hardwiredFunctions)
            {
                Block.Procedures.Add(FunctionDeclaration.AddHardwiredFunction(function.Name, this,
                    function.Type == null ? SimpleTypeDefinition.VoidType : Block.LookupType(function.Type),
                    function.ParameterTypes));
            }

            Assembly asm = null;

            var sysAsm = Assembly.Load("Oberon0.System");
            asm = sysAsm; // reached only if no exception

            foreach (var type in asm.GetExportedTypes())
            {
                if (type.GetCustomAttribute<Oberon0LibraryAttribute>() != null)
                {
                    LoadLibraryMembers(type);
                }
            }
        }

        private void DeclareStandardTypes()
        {
            SimpleTypeDefinition.IntType = new SimpleTypeDefinition(BaseTypes.Int, "INTEGER", true);
            SimpleTypeDefinition.BoolType = new SimpleTypeDefinition(BaseTypes.Bool, "BOOLEAN", true);
            SimpleTypeDefinition.RealType = new SimpleTypeDefinition(BaseTypes.Real, "REAL", true);
            SimpleTypeDefinition.StringType = new SimpleTypeDefinition(BaseTypes.String, "STRING", true);
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
                    continue; // this method is not officially exported
                }

                Debug.Assert(method.DeclaringType != null, "method.DeclaringType != null");
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
        internal ExternalFunctionDeclaration AddExternalFunctionDeclaration([NotNull] Oberon0ExportAttribute attr,
                                                                            [NotNull] string className,
                                                                            [NotNull] string methodName)
        {
            if (attr == null)
            {
                throw new ArgumentNullException(nameof(attr));
            }

            if (className == null)
            {
                throw new ArgumentNullException(nameof(className));
            }

            if (methodName == null)
            {
                throw new ArgumentNullException(nameof(methodName));
            }

            var rt = Block.LookupType(attr.ReturnType);
            if (rt == null)
            {
                throw new InvalidOperationException($"Return type {attr.ReturnType} not defined");
            }

            var procParameters = new ProcedureParameterDeclaration[attr.Parameters.Length];
            for (int i = 0; i < attr.Parameters.Length; i++)
            {
                AddParameters(attr, i, procParameters);
            }

            return new ExternalFunctionDeclaration(attr.Name, new Block(Block, this), rt,
                className, methodName, procParameters);
        }

        private class HardwiredFunction
        {
            public string Name { get; set; }
            public string Type { get; set; }

            public string[] ParameterTypes { get; set; }
        }
    }
}
