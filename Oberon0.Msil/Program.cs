#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using JetBrains.Annotations;
using Oberon0.Compiler;
using Oberon0.Compiler.Generator;
using Oberon0.Generator.MsilBin;
using System;
using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

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
            Argument<FileInfo> fileArg = new ("input-file")
            {
                Description = "The input file to be compiled",
                Arity = ArgumentArity.ExactlyOne
            };

            Option<DirectoryInfo> outputPathOpt = new("--output-path", "-o")
            {
                Description = "Output path where target files should be written to. Default: Current directory"
            };

            Option<bool> verboseOpt = new("--verbose", "-v")
            {
                Description = "Output more information"
            };

            Option<bool> cleanOpt = new("--clean")
            {
                Description = "Clean the build before running a new one."
            };

            Option<string> projectNameOpt = new("--project-name")
            {
                Description = "Name the project different to module name."
            };
            
            var rootCommand = new RootCommand("Compile an Oberon0 source file.")
            {
                fileArg,
                outputPathOpt,
                verboseOpt,
                cleanOpt,
                projectNameOpt
            };

            var result = rootCommand.Parse(args);
            if (result.Errors.Count > 0)
            {
                foreach (ParseError parseError in result.Errors)
                {
                    Console.Error.WriteLine(parseError.Message);
                }
                return 1;
            }

            var inputFile = result.GetRequiredValue(fileArg);
            var outputPath = result.GetValue(outputPathOpt);
            var verbose = result.GetValue(verboseOpt);
            var clean = result.GetValue(cleanOpt);
            var projectName = result.GetValue(projectNameOpt);

            return StartCompile(inputFile, outputPath, projectName, clean, verbose);
        }
        
        private static int StartCompile(FileSystemInfo inputFile, DirectoryInfo outputPath, string projectName, bool clean, bool verbose)
        {
            if (!inputFile.Exists)
            {
                Console.Error.WriteLine($"File does not exist: '{inputFile.Name}'.");
                return 1;
            }
            
            var m = Oberon0Compiler.CompileString(File.ReadAllText(inputFile.FullName));
            if (m.CompilerInstance?.HasError ?? true)
            {
                return 1;
            }

            var cg = new MsilBinGenerator(module: m);

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
