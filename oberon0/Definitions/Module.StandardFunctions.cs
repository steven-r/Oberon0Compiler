#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Module.StandardFunctions.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/Module.StandardFunctions.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Definitions
{
    using System;
    using System.Reflection;

    using Oberon0.Attributes;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Types;

    /// <summary>
    /// The module.
    /// </summary>
    public partial class Module
    {
        private void AddParameters(Oberon0ExportAttribute attr, int i, ProcedureParameter[] procParameters)
        {
            var isVar = false;
            var paramType = attr.Parameters[i];
            if (paramType.StartsWith("VAR ", StringComparison.InvariantCultureIgnoreCase))
            {
                isVar = true;
                paramType = paramType.Substring(4).Trim(); // skip "VAR "
            }
            else if (paramType.StartsWith("&"))
            {
                isVar = true;
                paramType = paramType.Substring(1).Trim();
            }

            var paramName = "attr" + i;
            TypeDefinition td = this.Block.LookupType(paramType);
            if (td == null)
            {
                throw new InvalidOperationException($"type {attr.Parameters[i]} not found");
            }

            procParameters[i] = new ProcedureParameter(paramName, this.Block, td, isVar);
        }

        private void DeclareStandardConsts()
        {
            this.Block.Declarations.Add(
                new ConstDeclaration("TRUE", this.Block.LookupType("BOOLEAN"), new ConstantBoolExpression(true)));
            this.Block.Declarations.Add(
                new ConstDeclaration("FALSE", this.Block.LookupType("BOOLEAN"), new ConstantBoolExpression(false)));
        }

        private void DeclareStandardFunctions()
        {
            // hardwired
            this.Block.Procedures.Add(
                FunctionDeclaration.AddHardwiredFunction(
                    "WriteInt",
                    this,
                    new ProcedureParameter("any", this.Block, this.Block.LookupType("INTEGER"), false)));
            this.Block.Procedures.Add(
                FunctionDeclaration.AddHardwiredFunction(
                    "WriteBool",
                    this,
                    new ProcedureParameter("any", this.Block, this.Block.LookupType("BOOLEAN"), false)));
            this.Block.Procedures.Add(
                FunctionDeclaration.AddHardwiredFunction(
                    "WriteString",
                    this,
                    new ProcedureParameter("any", this.Block, this.Block.LookupType("STRING"), false)));
            this.Block.Procedures.Add(
                FunctionDeclaration.AddHardwiredFunction(
                    "WriteReal",
                    this,
                    new ProcedureParameter("any", this.Block, this.Block.LookupType("REAL"), false)));
            this.Block.Procedures.Add(FunctionDeclaration.AddHardwiredFunction("WriteLn", this));
            this.Block.Procedures.Add(
                FunctionDeclaration.AddHardwiredFunction(
                    "ReadInt",
                    this,
                    new ProcedureParameter("any", this.Block, this.Block.LookupType("INTEGER"), true)));
            this.Block.Procedures.Add(
                FunctionDeclaration.AddHardwiredFunction(
                    "ReadBool",
                    this,
                    new ProcedureParameter("any", this.Block, this.Block.LookupType("BOOLEAN"), true)));
            this.Block.Procedures.Add(
                FunctionDeclaration.AddHardwiredFunction(
                    "ReadReal",
                    this,
                    new ProcedureParameter("any", this.Block, this.Block.LookupType("REAL"), true)));

            var asm = Assembly.Load("Oberon0.System");
            foreach (Type type in asm.GetExportedTypes())
            {
                if (type.GetCustomAttribute<Oberon0LibraryAttribute>() != null)
                {
                    this.LoadLibraryMembers(type);
                }
            }
        }

        private void DeclareStandardTypes()
        {
            SimpleTypeDefinition.IntType = new SimpleTypeDefinition(BaseTypes.Int, "INTEGER", true);
            SimpleTypeDefinition.BoolType = new SimpleTypeDefinition(BaseTypes.Bool, "BOOLEAN", true);
            SimpleTypeDefinition.RealType = new SimpleTypeDefinition(BaseTypes.Decimal, "REAL", true);
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
        /// load all members with that are exported through <see cref="Oberon0ExportAttribute"/>
        /// </summary>
        /// <param name="type">The type.</param>
        private void LoadLibraryMembers(Type type)
        {
            AssemblyName name = new AssemblyName(type.Assembly.FullName);
            foreach (MethodInfo method in type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            {
                var attr = method.GetCustomAttribute<Oberon0ExportAttribute>();
                if (attr == null) continue; // this method is not officially exported
                if (!method.IsStatic)
                {
                    throw new InvalidOperationException("method not static");
                }

                var rt = this.Block.LookupType(attr.ReturnType);
                if (rt == null)
                {
                    throw new InvalidOperationException($"return type {attr.ReturnType} not found");
                }

                ProcedureParameter[] procParameters = new ProcedureParameter[attr.Parameters.Length];
                for (int i = 0; i < attr.Parameters.Length; i++)
                {
                    this.AddParameters(attr, i, procParameters);
                }

                var fd = new FunctionDeclaration(attr.Name, this.Block, rt, procParameters)
                             {
                                 Prototype = $"[{name.Name}]Oberon0.{type.Name}::{method.Name}", IsExternal = true,
                             };
                this.Block.Procedures.Add(fd);
            }

            this.ExternalReferences.Add(type.Assembly);
        }
    }
}