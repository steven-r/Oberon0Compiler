#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using System;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Oberon0System.Attributes;

namespace Oberon0.Shared
{
    /// <summary>
    ///     Compile support functions
    /// </summary>
    public static class CompileSupportExtensions
    {
        /// <summary>
        ///     Create a intermediate structure containing the compiled c# code which can be emitted afterwards to memory/disk
        /// </summary>
        /// <param name="syntaxTree">The C# AST</param>
        /// <param name="assemblyName">The target assembly name. The file extension depends on <see cref="isExecutable" />.</param>
        /// <param name="codeGenerator">The used oberon0 code generator</param>
        /// <param name="isExecutable">If true, an executable will be created, a DLL otherwise</param>
        /// <returns>An intermediate form (see <see cref="CSharpCompilation" /> for details)</returns>
        public static CSharpCompilation CreateCompiledCSharpCode(this SyntaxTree syntaxTree, string assemblyName,
                                                                 ICodeGenerator codeGenerator, bool isExecutable = true)
        {
            var trustedAssembliesPaths =
                ((string) AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")).Split(Path.PathSeparator);
            var references = trustedAssembliesPaths
                             //.Where(p => neededAssemblies.Contains(Path.GetFileNameWithoutExtension(p)) || p.Contains("\\System.") && !p.Contains("\\System.Private"))
                            .Select(p => MetadataReference.CreateFromFile(p))
                            .ToList();
            references.Add(MetadataReference.CreateFromFile(typeof(Oberon0ExportAttribute).Assembly
               .Location)
            );
            var options = isExecutable
                ? new CSharpCompilationOptions(OutputKind.ConsoleApplication,
                    mainTypeName: codeGenerator.GetMainClassName())
                : new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary);
            return CSharpCompilation.Create(
                assemblyName + (isExecutable ? ".exe" : ".dll"),
                new[] {syntaxTree},
                references,
                options);
        }

        /// <summary>
        ///     Show warnings and stop in case of error
        /// </summary>
        /// <param name="result">The compilation result</param>
        /// <param name="showWarnings">if true, all warnings and errors are displayed</param>
        /// <exception cref="BadImageFormatException"></exception>
        public static void ThrowExceptionIfCompilationFailure(this EmitResult result, bool showWarnings = false)
        {
            foreach (var diagnostic in result.Diagnostics.Where(x => !x.IsSuppressed))
            {
                if (diagnostic.Severity == DiagnosticSeverity.Error ||
                    diagnostic.Severity == DiagnosticSeverity.Warning && showWarnings)
                {
                    Console.Out.WriteLine($"{diagnostic.Location}: {diagnostic.Id} - {diagnostic.GetMessage()}");
                }
            }

            if (result.Success)
            {
                return;
            }

            var compilationErrors = result.Diagnostics.Where(diagnostic =>
                                               diagnostic.IsWarningAsError ||
                                               diagnostic.Severity == DiagnosticSeverity.Error)
                                          .ToList();
            if (!compilationErrors.Any())
            {
                return;
            }

            var firstError = compilationErrors.First();
            string errorNumber = firstError.Id;
            string errorDescription = firstError.GetMessage();
            string firstErrorMessage = $"{errorNumber}: {errorDescription};";
            throw new BadImageFormatException($"Compilation failed, first error is: {firstErrorMessage}");
        }
    }
}
