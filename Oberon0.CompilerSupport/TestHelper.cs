﻿#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Antlr4.Runtime;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Test.Support
{
    [ExcludeFromCodeCoverage]
    public static class TestHelper
    {
        private static readonly List<CompilerError> CompilerErrors = [];

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
                        parser.AddErrorListener(new TestErrorListener<IToken>(CompilerErrors));
                    },
                    InitLexer = lexer =>
                    {
                        lexer.RemoveErrorListeners();
                        lexer.AddErrorListener(new TestErrorListener<int>(CompilerErrors));
                    },
                    AfterCompile = _ => { errors.AddRange(CompilerErrors); }
                });
        }

        public static Module CompileString(string source, params string[] expectedErrors)
        {
            return CompileString(source, null, expectedErrors);
        }

        public static Module CompileString(string source, ITestOutputHelper? output,
                                           params string[] expectedErrors)
        {
            var errors = new List<CompilerError>();
            var m = CompileString(source, errors);
            if (expectedErrors.Length == 0 && errors.Count > 0)
            {
                DumpErrors(errors);
                Assert.Fail($"Expected no errors, actually found {errors.Count}");
            }

            if (expectedErrors.Length != errors.Count)
            {
                DumpErrors(errors);
                Assert.Fail($"Expected {expectedErrors.Length} errors, actually found {errors.Count}");
            }

            for (int i = 0; i < expectedErrors.Length; i++)
            {
                Assert.Equal(expectedErrors[i], errors[i].Message);
            }

            return m;

            void DumpErrors(IEnumerable<CompilerError> compilerErrors)
            {
                foreach (var compilerError in compilerErrors)
                {
                    string message = $"[{compilerError.Line}/{compilerError.Column}] {compilerError.Message}";
                    if (output == null)
                    {
                        Console.Error.WriteLine(message);
                    } else
                    {
                        output.WriteLine(message);
                    }
                }
            }
        }

        /// <summary>
        ///     Helper to compile some code with a standard application surrounding
        /// </summary>
        /// <param name="operations"></param>
        /// <param name="outputHelper"></param>
        /// <param name="expectedErrors"></param>
        /// <returns></returns>
        public static Module CompileSingleStatement(string operations, ITestOutputHelper? outputHelper, params string[] expectedErrors)
        {
            return CompileString(
                $"""
                 MODULE test; 
                 CONST 
                    true_ = TRUE; 
                    false_ = FALSE; 
                 VAR 
                    a,b,c,d,e,f,g,h,x,y,z: BOOLEAN; 
                    i, j, k, l: INTEGER;
                    r, s, t: REAL;
                 BEGIN 
                   {operations} 
                 END test.
                 """,
                outputHelper,
                expectedErrors);
        }

        /// <summary>
        ///     Helper to compile some code with a standard application surrounding
        /// </summary>
        /// <param name="operations"></param>
        /// <param name="expectedErrors"></param>
        /// <returns></returns>
        public static Module CompileSingleStatement(string operations, params string[] expectedErrors)
        {
            return CompileSingleStatement(operations, null, expectedErrors);
        }

        internal class TestErrorListener<TSymbol>(List<CompilerError> compilerErrors) : IAntlrErrorListener<TSymbol>
        {
            public void SyntaxError(
                TextWriter output,
                IRecognizer recognizer,
                TSymbol offendingSymbol,
                int line,
                int charPositionInLine,
                string msg,
                RecognitionException e)
            {
                compilerErrors.Add(
                    new CompilerError {Column = charPositionInLine, Line = line, Message = msg, Exception = e});
                Console.WriteLine($"[{line}/{charPositionInLine}] - {msg}");
            }
        }
    }
}
