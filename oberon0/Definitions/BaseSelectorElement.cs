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
    using System;

    using Antlr4.Runtime;

    using JetBrains.Annotations;

    public abstract class BaseSelectorElement
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        protected BaseSelectorElement([NotNull] IToken tokenStart)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
        {
            this.Token = tokenStart ?? throw new ArgumentNullException(nameof(tokenStart));
        }

#pragma warning disable CS3003 // Type is not CLS-compliant
        public IToken Token { get; }
#pragma warning restore CS3003 // Type is not CLS-compliant
    }
}