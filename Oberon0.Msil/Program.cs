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
using Microsoft.CodeAnalysis.Emit;
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
                new Argument<FileInfo>("input-file", "The input file to be compiled") {Arity = ArgumentArity.ExactlyOne}.ExistingOnly(),
                new Option<bool>(
                    new[] {"-v", "--verbose"},
                    "Be verbose on parsing"
                    ),
            };
            rootCommand.Handler = CommandHandler.Create<ParseResult, FileInfo, bool, IConsole>(StartCompile);
            return rootCommand.Invoke(args);
        }

        private static int StartCompile(ParseResult result, FileSystemInfo inputFile, bool verbose, IConsole console)
        {
            if (!inputFile.Exists)
            {
                Console.Error.Write("Cannot find {0}", inputFile.FullName);
                return 1;
            }

            _fileName = Path.GetFileNameWithoutExtension(inputFile.FullName);

            var m = Oberon0Compiler.CompileString(File.ReadAllText(inputFile.FullName));
            if (m.CompilerInstance.HasError) return 1;

            ICodeGenerator cg = new MsilBinGenerator() { Module = m };

            cg.GenerateIntermediateCode();
            string code = cg.IntermediateCode();

            if (!CompileCode(code, _fileName, cg, verbose)) return 2;

            return 0;
        }

        private static bool CompileCode(string source, string filename, ICodeGenerator cg, bool showWarnings = false)
        {
            string assemblyName = Path.GetFileNameWithoutExtension(filename);

            var syntaxTree = CSharpSyntaxTree.ParseText(source);

            if (syntaxTree == null)
                throw new InvalidOperationException("Could not compile source code, please look at error report");

            var compilationUnit = syntaxTree.CreateCompiledCSharpCode(assemblyName, cg);

            using var file = File.Create(filename + ".exe");
            var result = compilationUnit.Emit(file, options:new EmitOptions(true, includePrivateMembers: false));
            result.ThrowExceptionIfCompilationFailure(showWarnings);
            file.Flush(true);
            return true;
        }
    }
}