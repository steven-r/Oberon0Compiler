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
    public abstract class BaseSelectorElement(IToken tokenStart)
    {
        /// <summary>
        ///     Gets the token or available.
        /// </summary>
        public IToken Token { get; } = tokenStart;

        /// <summary>
        ///     Gets or sets the type definition.
        /// </summary>
        public TypeDefinition TypeDefinition { get; set; } = null!;
    }
}
