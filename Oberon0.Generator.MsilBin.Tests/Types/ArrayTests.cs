#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Generator.MsilBin.Tests.Types
{
    public class ArrayTests(ITestOutputHelper output)
    {
        [Fact]
        public void SimpleArrayDefinition()
        {
            const string source = """
                                  MODULE Array;
                                  VAR 
                                    a: ARRAY 32 OF INTEGER;
                                    b: ARRAY 32 OF BOOLEAN;

                                  END Array.
                                  """;
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            Assert.NotNull(syntaxTree.CompileAndLoadAssembly(cg));
        }

        [Fact]
        public void SimpleArrayDefinitionLocal()
        {
            const string source = """
                                  MODULE Array;
                                  VAR 
                                    a: ARRAY 32 OF INTEGER;
                                    b: ARRAY 32 OF INTEGER;
                                    i: INTEGER;
                                    s: INTEGER;

                                  BEGIN
                                    i := 1;
                                    WHILE (i <= 32) DO
                                      a[i] := i;
                                      b[i] := 32-i;
                                      i := i+1
                                    END;
                                    i := 1;
                                    WHILE (i <= 32) DO
                                      s := s + a[i] + b[i];
                                      i := i+1
                                    END;
                                    WriteInt(s);
                                    WriteLn
                                  END Array.
                                  """;
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("1024\n", output1.ToString().NlFix());
        }

        [Fact]
        public void SimpleArraySetAndGetValueGlobal()
        {
            const string source = """
                                  MODULE Array3;
                                  VAR 
                                    a: ARRAY 32 OF INTEGER;
                                    n: INTEGER;

                                  BEGIN
                                    a[1] := 1;
                                    n := a[1];
                                    WriteInt(n);
                                    WriteLn
                                  END Array3.
                                  """;
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("1\n", output1.ToString().NlFix());
        }
    }
}
