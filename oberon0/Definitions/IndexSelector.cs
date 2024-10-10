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
    public class IndexSelector(Expression index, IToken tokenStart) : BaseSelectorElement(tokenStart)
    {
        public Expression IndexDefinition { get; set; } = index;
    }
}
