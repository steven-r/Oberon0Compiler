#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using JetBrains.Annotations;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Oberon0.Compiler;
using Oberon0.Generator.MsilBin;
using Oberon0.Shared;

namespace Oberon0.Msil
{
    /// <summary>
    /// The program.
    /// </summary>
    [UsedImplicitly]
    public class Program
    {
        private static string _fileName;

        /// <summary>
        /// The main.
        /// </summary>
        /// <param name="args">
        /// The args.
        /// </param>
        /// <returns>
        /// The return code.
        /// </returns>
        public static int Main(string[] args)
        {
            var rootCommand = new RootCommand("Compile an Oberon0 source file.")
            {
                new Option<FileInfo>(
                    new [] {"--input", "-i"},
                    "The input file to be compiled.") { Argument = new Argument<FileInfo>().ExistingOnly()},
                new Option<bool>(
                    new[] {"-v", "--verbose"},
                    "Be verbose on parsing"
                    ),
            };
            rootCommand.Handler = CommandHandler.Create<ParseResult, FileInfo, bool, IConsole>(StartCompile);
            return rootCommand.Invoke(args.Length == 0 ? new [] { "--help" } : args);
        }

        private static int StartCompile(ParseResult result, FileSystemInfo input, bool verbose, IConsole console)
        {
            if (!input.Exists)
            {
                Console.Error.Write("Cannot find {0}", input.FullName);
                return 1;
            }

            _fileName = Path.GetFileNameWithoutExtension(input.FullName);

            var m = Oberon0Compiler.CompileString(File.ReadAllText(input.FullName));
            if (m.CompilerInstance.HasError) return 1;

            ICodeGenerator cg = new MsilBinGenerator() { Module = m };

            cg.Generate();
            string code = cg.DumpCode();

            if (!CompileCode(code, _fileName, cg, verbose)) return 2;

            return 0;
        }

        private static bool CompileCode(string source, string filename, ICodeGenerator cg, bool showWarnings = false)
        {
            string assemblyName = Path.GetFileNameWithoutExtension(filename);

            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(source);

            if (syntaxTree == null)
                throw new InvalidOperationException("Could not compile source code, please look at error report");

            var compilationUnit = syntaxTree.CreateCompiledCSharpCode(assemblyName, cg);

            using var file = File.Create(filename + ".exe");
            var result = compilationUnit.Emit(file);
            result.ThrowExceptionIfCompilationFailure(showWarnings);
            file.Flush(true);
            return true;
        }
    }
}