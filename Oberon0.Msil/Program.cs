#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using JetBrains.Annotations;
using Oberon0.Compiler;
using Oberon0.Generator.MsilBin;
using Oberon0.Shared;

namespace Oberon0.Msil
{
    /// <summary>
    ///     The program.
    /// </summary>
    [UsedImplicitly]
    public static class Program
    {
        /// <summary>
        ///     The main.
        /// </summary>
        /// <param name="args">
        ///     The args.
        /// </param>
        /// <returns>
        ///     The return code.
        /// </returns>
        public static int Main(string[] args)
        {
            var rootCommand = new RootCommand("Compile an Oberon0 source file.")
            {
                new Argument<FileInfo>("input-file", "The input file to be compiled") {Arity = ArgumentArity.ExactlyOne}
                   .ExistingOnly(),
                new Option<DirectoryInfo>(
                    new[] {"--output-path", "-o"},
                    "Output path where target files should be written to. Default: Current directory"
                ),
                new Option<bool>(
                    new[] {"--verbose", "-v"},
                    "Output more information"
                ),
                new Option<bool>(
                    new[] {"--clean"},
                    "Clean the build before running a new one."
                ),
                new Option<string>(
                    new[] {"--project-name"},
                    "Name the project different to module name."
                )
            };
            rootCommand.Handler = CommandHandler.Create<BindingContext, FileInfo, DirectoryInfo, string, bool, bool>(StartCompile);
            return rootCommand.Invoke(args);
        }

        private static int StartCompile(BindingContext context, FileSystemInfo inputFile, DirectoryInfo outputPath, string projectName, bool clean, bool verbose)
        {
            var m = Oberon0Compiler.CompileString(File.ReadAllText(inputFile.FullName));
            if (m.CompilerInstance.HasError)
            {
                return 1;
            }

            ICodeGenerator cg = new MsilBinGenerator {Module = m};

            cg.GenerateIntermediateCode();

            return cg.GenerateBinary(new CreateBinaryOptions()
            {
                OutputPath = outputPath?.FullName ?? Path.GetDirectoryName(inputFile.FullName),
                CleanSolution = clean,
                OutputDataRetrieved = OutputDataRetrieved,
                ErrorDataRetrieved = ErrorDataRetrieved,
                ModuleName = projectName ?? m.Name,
                Verbose = verbose,
            }) ? 0 : 2;
        }

        // not possible from being caught in testing
        [ExcludeFromCodeCoverage]
        private static void ErrorDataRetrieved(object sender, ProcessOutputReceivedEventArgs e)
        {
            if (e.Options.Verbose)
            {
                Console.Error.WriteLine(e.Data);
            }
        }

        private static void OutputDataRetrieved(object sender, ProcessOutputReceivedEventArgs e)
        {
            if (e.Options.Verbose)
            {
                Console.Out.WriteLine(e.Data);
            }
        }
    }
}
