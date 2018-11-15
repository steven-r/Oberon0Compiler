#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExternalFunctionDeclaration.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/ExternalFunctionDeclaration.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Definitions
{
    using System;
    using System.Reflection;

    using Oberon0.Compiler.Types;

    public class ExternalFunctionDeclaration : FunctionDeclaration
    {
        public ExternalFunctionDeclaration(
            string name,
            Block block,
            TypeDefinition returnType,
            MethodInfo methodName,
            params ProcedureParameterDeclaration[] parameters)
            : base(name, block, returnType, parameters)
        {
            ClassName = methodName.DeclaringType?.FullName
                        ?? throw new ArgumentNullException(nameof(methodName), "Declaring type not found");
            MethodName = methodName.Name;
            Assembly = methodName.DeclaringType?.Assembly;
        }

        public Assembly Assembly { get; }

        public string ClassName { get; }

        public string MethodName { get; }
    }
}