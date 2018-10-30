#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestHelper.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.CompilerSupport/TestHelper.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.CompilerSupport
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Antlr4.Runtime;

    using Oberon0.Compiler;
    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Tests;

    public static class TestHelper
    {
        private static readonly List<CompilerError> CompilerErrors = new List<CompilerError>();

        public static Module CompileString(string source, List<CompilerError> errors)
        {
            CompilerErrors.Clear();
            AntlrInputStream input = new AntlrInputStream(source);
            OberonGrammarLexer lexer = new OberonGrammarLexer(input);
            CommonTokenStream tokens = new CommonTokenStream(lexer);
            OberonGrammarParser parser = new OberonGrammarParser(tokens);
            lexer.RemoveErrorListeners();
            parser.RemoveErrorListeners();
            parser.AddErrorListener(TestErrorListener.Instance);
            parser.AddParseListener(new Oberon0CompilerListener(parser));

            OberonGrammarParser.ModuleContext context = parser.module();
            errors.AddRange(CompilerErrors);
            return context.modres;
        }

        public static Module CompileString(string source)
        {
            List<CompilerError> errors = new List<CompilerError>(0);
            Module m = CompileString(source, errors);
            if (errors.Any())
            {
                throw new InvalidOperationException("There have been errors, please fix them");
            }

            return m;
        }

        internal class TestErrorListener : BaseErrorListener
        {
            public static readonly TestErrorListener Instance = new TestErrorListener();

            public override void SyntaxError(
                IRecognizer recognizer,
                IToken offendingSymbol,
                int line,
                int charPositionInLine,
                string msg,
                RecognitionException e)
            {
                CompilerErrors.Add(
                    new CompilerError { Column = charPositionInLine, Line = line, Message = msg, Exception = e });
                Console.WriteLine($"[{line}/{charPositionInLine}] - {msg}");
            }
        }
    }
}