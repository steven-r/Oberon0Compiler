#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Antlr4.Runtime;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    public abstract class BaseSelectorElement
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        protected BaseSelectorElement(IToken tokenStart)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            Token = tokenStart;
        }

#pragma warning disable CS3003 // Type is not CLS-compliant
        /// <summary>
        ///     Gets the token or available.
        /// </summary>
        public IToken Token { get; }
#pragma warning restore CS3003 // Type is not CLS-compliant

        /// <summary>
        ///     Gets or sets the type definition.
        /// </summary>
        public TypeDefinition TypeDefinition { get; set; }
    }
}
