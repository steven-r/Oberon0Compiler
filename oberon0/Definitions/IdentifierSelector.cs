#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Antlr4.Runtime;

namespace Oberon0.Compiler.Definitions
{
    public class IdentifierSelector(string name, IToken tokenStart) : BaseSelectorElement(tokenStart)
    {
        public Declaration Element { get; set; } = null!;

        public string Name { get; } = name;
    }
}
