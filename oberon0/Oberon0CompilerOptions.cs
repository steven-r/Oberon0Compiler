#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;

namespace Oberon0.Compiler
{
    /// <summary>
    ///     Oberon 0 compiler options.
    /// </summary>
    public class Oberon0CompilerOptions
    {
        /// <summary>
        ///     Gets or sets the after compile.
        /// </summary>
        public Action<Oberon0Compiler> AfterCompile { get; set; }

#pragma warning disable CS3003 // Type is not CLS-compliant
        /// <summary>
        ///     Gets or sets the init lexer.
        /// </summary>
        public Action<OberonGrammarLexer> InitLexer { get; set; }
#pragma warning restore CS3003 // Type is not CLS-compliant

#pragma warning disable CS3003 // Type is not CLS-compliant
        /// <summary>
        ///     Gets or sets the init parser.
        /// </summary>
        public Action<OberonGrammarParser> InitParser { get; set; }
#pragma warning restore CS3003 // Type is not CLS-compliant
    }
}
