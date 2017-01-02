﻿using System;
using System.Collections.Generic;
using Antlr4.Runtime;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Tests
{
    internal static class TestHelper
    {
        private static readonly List<CompilerError> CompilerErrors = new List<CompilerError>();


        internal class TestErrorListener: BaseErrorListener, IAntlrErrorListener<int>
        {
            public static readonly TestErrorListener Instance = new TestErrorListener();

            public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg,
                RecognitionException e)
            {
                CompilerErrors.Add(new CompilerError
                {
                    Column = charPositionInLine,
                    Line = line,
                    Message = msg,
                    Exception = e
                });
                Console.WriteLine($"[{line}/{charPositionInLine}] - {msg}");
            }

            public void SyntaxError(IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg,
                RecognitionException e)
            {
                CompilerErrors.Add(new CompilerError
                {
                    Column = charPositionInLine,
                    Line = line,
                    Message = msg,
                    Exception = e
                });
                Console.WriteLine($"[{line}/{charPositionInLine}] - {msg}");
            }
        }

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

    }
}
