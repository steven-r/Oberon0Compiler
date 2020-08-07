#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Reflection;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    /// Function that has been declared outside the compiler structure (e.g. Oberon0System).
    /// </summary>
    public class ExternalFunctionDeclaration : FunctionDeclaration
    {
        /// <summary>
        /// Declare a new external function
        /// </summary>
        /// <param name="name">The name of the function in Oberon0</param>
        /// <param name="block">The surrounding block</param>
        /// <param name="returnType">The return type</param>
        /// <param name="methodName">The method info coming from .NET</param>
        /// <param name="parameters">a list of parameters</param>
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

        /// <summary>
        /// Gets or sets the assembly providing the method
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        /// The class name this Method belongs to
        /// </summary>
        public string ClassName { get; }

        /// <summary>
        /// The name of the implementation method
        /// </summary>
        public string MethodName { get; }
    }
}