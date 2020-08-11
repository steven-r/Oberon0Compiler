#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Antlr4.Runtime;

namespace Oberon0.Compiler.Definitions
{
    public class IdentifierSelector : BaseSelectorElement
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant

        public IdentifierSelector(string name, IToken tokenStart)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
            : base(tokenStart)
        {
            Name = name;
        }

        public Declaration Element { get; set; }

        public string Name { get; }
    }
}