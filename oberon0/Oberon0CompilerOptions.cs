#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Oberon0CompilerOptions.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/Oberon0CompilerOptions.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler
{
    using System;

    /// <summary>
    /// Oberon 0 compiler options.
    /// </summary>
    public class Oberon0CompilerOptions
    {
        /// <summary>
        /// Gets or sets the after compile.
        /// </summary>
        public Action<Oberon0Compiler> AfterCompile { get; set; }

#pragma warning disable CS3003 // Type is not CLS-compliant
                              /// <summary>
                              /// Gets or sets the init lexer.
                              /// </summary>
        public Action<OberonGrammarLexer> InitLexer { get; set; }
#pragma warning restore CS3003 // Type is not CLS-compliant

#pragma warning disable CS3003 // Type is not CLS-compliant
                              /// <summary>
                              /// Gets or sets the init parser.
                              /// </summary>
        public Action<OberonGrammarParser> InitParser { get; set; }
#pragma warning restore CS3003 // Type is not CLS-compliant
    }
}