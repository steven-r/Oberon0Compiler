#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Antlr4.Runtime;
using NUnit.Framework;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;

namespace Oberon0.TestSupport
{
    public static class TestHelper
    {
        private static readonly List<CompilerError> CompilerErrors = new List<CompilerError>();

        public static Module CompileString(string source, List<CompilerError> errors)
        {
            CompilerErrors.Clear();
            return Oberon0Compiler.CompileString(
                source,
                new Oberon0CompilerOptions
                    {
                        InitParser = parser =>
                            {
                                parser.RemoveErrorListeners();
                                parser.AddErrorListener(TestErrorListener.Instance);
                            },
                        InitLexer = lexer =>
                            {
                                lexer.RemoveErrorListeners();
                                lexer.AddErrorListener(TestErrorListener.Instance);
                            },
                        AfterCompile = compiler =>
                            {
                                errors.AddRange(CompilerErrors);
                            }
                    });
        }

        [ExcludeFromCodeCoverage]
        public static Module CompileString(string source, params string[] expectedErrors)
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = CompileString(source, errors);
            if (expectedErrors.Length == 0 && errors.Count > 0) Assert.Fail($"Expected no errors, actually found {errors.Count}");

            if (expectedErrors.Length != errors.Count) Assert.Fail($"Expected {expectedErrors.Length} errors, actually found {errors.Count}");

            for (var i = 0; i < expectedErrors.Length; i++) Assert.AreEqual(expectedErrors[i], errors[i].Message);

            return m;
        }

        internal class TestErrorListener : BaseErrorListener, IAntlrErrorListener<int>
        {
            public static readonly TestErrorListener Instance = new TestErrorListener();

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