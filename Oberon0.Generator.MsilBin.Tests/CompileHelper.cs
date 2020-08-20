#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.IO;
using Microsoft.CodeAnalysis;
using Oberon0.Compiler;
using Oberon0.Shared;
using Xunit.Abstractions;

namespace Oberon0.Generator.MsilBin.Tests
{
    internal static class CompileHelper
    {
        internal static byte[] CompileAndLoadAssembly(this SyntaxTree syntaxTree, ICodeGenerator codeGenerator, bool isExecutable = false)
        {
            string assemblyName = Path.GetRandomFileName();
            var compilation = syntaxTree.CreateCompiledCSharpCode(assemblyName, codeGenerator, isExecutable);

            using var ms = new MemoryStream();
            var result = compilation.Emit(ms);
            result.ThrowExceptionIfCompilationFailure();
            ms.Seek(0, SeekOrigin.Begin);
            return ms.ToArray();
        }

        internal static ICodeGenerator CompileOberon0Code(string source, out string code, ITestOutputHelper outputHelper = null)
        {
            var m = Oberon0Compiler.CompileString(source);

            ICodeGenerator cg = new MsilBinGenerator() { Module = m };

            cg.GenerateIntermediateCode();

            code = cg.IntermediateCode();
            outputHelper?.WriteLine(code);
            return cg;
        }


        public static string NlFix(this string data)
        {
            return data.Replace("\r\n", "\n");
        }
    }
}
