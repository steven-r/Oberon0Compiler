#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Oberon0Compiler.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/Oberon0Compiler.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler
{
    using Antlr4.Runtime;

    using Oberon0.Compiler.Definitions;

    public class Oberon0Compiler
    {
        public static Oberon0Compiler Instance { get; set; }

#pragma warning disable CS3003 // Type is not CLS-compliant
        public OberonGrammarParser.ModuleContext Context { get; set; }
#pragma warning restore CS3003 // Type is not CLS-compliant

#pragma warning disable CS3003 // Type is not CLS-compliant
        public OberonGrammarLexer Lexer { get; private set; }
#pragma warning restore CS3003 // Type is not CLS-compliant

#pragma warning disable CS3003 // Type is not CLS-compliant
        public OberonGrammarParser Parser { get; private set; }
#pragma warning restore CS3003 // Type is not CLS-compliant

        /// <summary>
        /// Gets or sets a value indicating whether the module has at least one error.
        /// </summary>
        public bool HasError { get; set; }

        public static Module CompileString(string source, Oberon0CompilerOptions options = null)
        {
            Instance = new Oberon0Compiler();

            AntlrInputStream input = new AntlrInputStream(source);
            Instance.Lexer = new OberonGrammarLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(Instance.Lexer);

            options?.InitLexer?.Invoke(Instance.Lexer);

            Instance.Parser = new OberonGrammarParser(tokens);
            Instance.Parser.AddParseListener(new Oberon0CompilerListener(Instance.Parser));

            options?.InitParser?.Invoke(Instance.Parser);

            Instance.Context = Instance.Parser.module();

            Instance.HasError = Instance.Parser.NumberOfSyntaxErrors > 0;

            options?.AfterCompile?.Invoke(Instance);

            return Instance.Context.modres;
        }
    }
}