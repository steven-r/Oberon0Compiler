#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using JetBrains.Annotations;
using Oberon0.Compiler;
using Oberon0.Generator.MsilBin;
using Oberon0.Shared;
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
            var fileArg = new Argument<FileInfo>
            {
                Name = "input-file",
                Description = "The input file to be compiled",
                Arity = ArgumentArity.ExactlyOne
            };
            
            var outputPathOpt = new Option<DirectoryInfo>(
                aliases: new[] { "--output-path", "-o" },
                description: "Output path where target files should be written to. Default: Current directory");
            
            var verboseOpt = new Option<bool>(
                aliases: new[] { "--verbose", "-v" },
                description: "Output more information");
            
            var cleanOpt = new Option<bool>(
                aliases: new[] { "--clean" },
                description: "Clean the build before running a new one.");
            
            var projectNameOpt = new Option<string>(
                aliases: new[] { "--project-name" },
                description: "Name the project different to module name.");
            
            var rootCommand = new RootCommand("Compile an Oberon0 source file.")
            {
                fileArg,
                outputPathOpt,
                verboseOpt,
                cleanOpt,
                projectNameOpt
            };
            
            // Use a custom handler that properly returns the exit code
            rootCommand.Handler = new AnonymousCommandHandler(context =>
            {
                var inputFile = context.ParseResult.GetValueForArgument(fileArg);
                var outputPath = context.ParseResult.GetValueForOption(outputPathOpt);
                var verbose = context.ParseResult.GetValueForOption(verboseOpt);
                var clean = context.ParseResult.GetValueForOption(cleanOpt);
                var projectName = context.ParseResult.GetValueForOption(projectNameOpt);
                
                context.ExitCode = StartCompile(inputFile, outputPath, projectName, clean, verbose);
                return Task.FromResult(0);
            });
            
            return rootCommand.Invoke(args);
        }
        
        private class AnonymousCommandHandler : ICommandHandler
        {
            private readonly Func<InvocationContext, Task> _func;
            
            public AnonymousCommandHandler(Func<InvocationContext, Task> func)
            {
                _func = func;
            }
            
            public int Invoke(InvocationContext context)
            {
                _func(context).Wait();
                return context.ExitCode;
            }
            
            public Task<int> InvokeAsync(InvocationContext context)
            {
                return _func(context).ContinueWith(t => context.ExitCode);
            }
        }

        private static int StartCompile(FileSystemInfo inputFile, DirectoryInfo outputPath, string projectName, bool clean, bool verbose)
        {
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
