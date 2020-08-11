#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Shared;

namespace Oberon0.Generator.MsilBin.PredefinedFunctions
{
    /// <summary>
    /// Interface used to generate code for a specific standard function
    /// </summary>
    public interface IStandardFunctionGenerator
    {
        /// <summary>
        /// Generates code for the given function/procedure
        /// </summary>
        /// <param name="metadata">The function metadata.</param>
        /// <param name="codeGenerator">Code generator</param>
        /// <param name="functionDeclaration">The function declaration</param>
        /// <param name="parameters">The list of parameters</param>
        /// <param name="block">The block.</param>
        ExpressionSyntax Generate(
            [UsedImplicitly] IStandardFunctionMetadata metadata,
            ICodeGenerator codeGenerator,
            FunctionDeclaration functionDeclaration,
            IReadOnlyList<Expression> parameters);
    }
}