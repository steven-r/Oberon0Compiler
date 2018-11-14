#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IndexSelector.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/IndexSelector.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Definitions
{
    using Antlr4.Runtime;

    using Oberon0.Compiler.Expressions;

    public class IndexSelector : BaseSelectorElement
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public IndexSelector(Expression index, IToken tokenStart)
            : base(tokenStart)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            IndexDefinition = index;
        }

        public Expression IndexDefinition { get; set; }
    }
}