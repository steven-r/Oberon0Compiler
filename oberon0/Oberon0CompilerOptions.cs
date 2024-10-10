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
        public required Action<Oberon0Compiler> AfterCompile { get; set; }

        /// <summary>
        ///     Gets or sets the init lexer.
        /// </summary>
#pragma warning disable CS3003 // Typ ist nicht CLS-kompatibel
        public required Action<OberonGrammarLexer> InitLexer { get; set; }
#pragma warning restore CS3003 // Typ ist nicht CLS-kompatibel

        /// <summary>
        ///     Gets or sets the init parser.
        /// </summary>
#pragma warning disable CS3003 // Typ ist nicht CLS-kompatibel
        public required Action<OberonGrammarParser> InitParser { get; set; }
#pragma warning restore CS3003 // Typ ist nicht CLS-kompatibel
    }
}
