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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    using Antlr4.Runtime;

    using NUnit.Framework;

    using Oberon0.Compiler;
    using Oberon0.Compiler.Definitions;
    using Oberon0.TestSupport;

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
            lexer.AddErrorListener(TestErrorListener.Instance);
            parser.RemoveErrorListeners();
            parser.AddErrorListener(TestErrorListener.Instance);
            parser.AddParseListener(new Oberon0CompilerListener(parser));

            OberonGrammarParser.ModuleContext context = parser.module();
            errors.AddRange(CompilerErrors);
            return context.modres;
        }

        [ExcludeFromCodeCoverage]
        public static Module CompileString(string source, params string[] expectedErrors)
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = CompileString(source, errors);
            if (expectedErrors.Length == 0 && errors.Count > 0)
            {
                Assert.Fail($"Expected no errors, actually found {errors.Count}");
            }

            if (expectedErrors.Length != errors.Count)
            {
                Assert.Fail($"Expected {expectedErrors.Length} errors, actually found {errors.Count}");
            }

            for (int i = 0; i < expectedErrors.Length; i++)
            {
                Assert.AreEqual(expectedErrors[i], errors[i].Message);    
            }

            return m;
        }

        internal class TestErrorListener : BaseErrorListener, IAntlrErrorListener<int>
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

            public void SyntaxError(
                IRecognizer recognizer,
                int offendingSymbol,
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