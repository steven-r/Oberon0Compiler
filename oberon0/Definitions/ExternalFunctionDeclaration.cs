#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Diagnostics;
using JetBrains.Annotations;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    /// Function that has been declared outside the compiler structure (e.g. Oberon0System).
    /// </summary>
    [DebuggerDisplay("<EXT> {ReturnType} {Name} -> {ClassName}.{MethodName}")]
    public class ExternalFunctionDeclaration : FunctionDeclaration
    {
        /// <summary>
        /// Declare a new external function
        /// </summary>
        /// <param name="name">The name of the function in Oberon0</param>
        /// <param name="block">The surrounding block</param>
        /// <param name="returnType">The return type</param>
        /// <param name="methodName">The method name</param>
        /// <param name="parameters">a list of parameters</param>
        /// <param name="className">The class name</param>
        public ExternalFunctionDeclaration(
            string name,
            Block block,
            [NotNull] TypeDefinition returnType,
            [NotNull] string className,
            [NotNull] string methodName,
            params ProcedureParameterDeclaration[] parameters)
            : base(name, block, returnType, parameters)
        {
            ClassName = className ?? throw new ArgumentNullException(nameof(className));
            MethodName = methodName ?? throw new ArgumentNullException(nameof(methodName));
        }

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