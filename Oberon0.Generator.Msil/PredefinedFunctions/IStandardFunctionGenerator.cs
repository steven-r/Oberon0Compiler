#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IStandardFunctionGenerator.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil/IStandardFunctionGenerator.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    using System.Collections.Generic;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;

    /// <summary>
    /// Interface used to generate code for a specific standard function
    /// </summary>
    public interface IStandardFunctionGenerator
    {
        /// <summary>
        /// Generates code for the given function/procedure
        /// </summary>
        /// <param name="metadata">The function metadata.</param>
        /// <param name="generator">The generator.</param>
        /// <param name="functionDeclaration">The function declaration</param>
        /// <param name="parameters">The list of parameters</param>
        /// <param name="block">The block.</param>
        void Generate(
            IStandardFunctionMetadata metadata,
            CodeGenerator generator,
            FunctionDeclaration functionDeclaration,
            IReadOnlyList<Expression> parameters,
            Block block);
    }
}