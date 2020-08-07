#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Antlr4.Runtime;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Definitions
{
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