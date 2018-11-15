#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseSelectorElement.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/BaseSelectorElement.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Definitions
{
    using Antlr4.Runtime;

    using Oberon0.Compiler.Types;

    public abstract class BaseSelectorElement
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        protected BaseSelectorElement(IToken tokenStart)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            this.Token = tokenStart;
        }

#pragma warning disable CS3003 // Type is not CLS-compliant
        /// <summary>
        /// Gets the token or available.
        /// </summary>
        public IToken Token { get; }
#pragma warning restore CS3003 // Type is not CLS-compliant

        /// <summary>
        /// Gets or sets the type definition.
        /// </summary>
        public TypeDefinition TypeDefinition { get; set; }

        /// <summary>
        /// Gets or sets the basic type definition. The base type definition represents the root type.
        /// </summary>
        public TypeDefinition BasicTypeDefinition { get; set; }
    }
}