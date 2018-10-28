#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentifierSelector.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/IdentifierSelector.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Definitions
{
    using Antlr4.Runtime;

    using JetBrains.Annotations;

    using Oberon0.Compiler.Types;

    public class IdentifierSelector : BaseSelectorElement
    {
#pragma warning disable CS3001 // Argument type is not CLS-compliant
        public IdentifierSelector(string name, [NotNull] IToken tokenStart)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
            : base(tokenStart)
        {
            Name = name;
        }

        public Declaration Element { get; set; }

        public string Name { get; }

        public TypeDefinition Type { get; set; }
    }
}