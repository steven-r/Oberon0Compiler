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

namespace Oberon0.Generator.MsilBin.Tests.Libraries
{
    [Collection("Sequential")]
    public class SystemTests(ITestOutputHelper output)
    {
        [Fact]
        public void SystemToStringBool()
        {
            const string source = """
                                  MODULE ToStringTest;
                                  VAR
                                    s: STRING;

                                  BEGIN
                                    s := ToString(TRUE);
                                    WriteString(s);
                                    WriteLn
                                  END ToStringTest.
                                  """;
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("True\n", output1.ToString().NlFix());
        }

        [Fact]
        public void SystemToStringInt()
        {
            const string source = """
                                  MODULE ToStringTest;
                                  VAR
                                    s: STRING;

                                  BEGIN
                                    s := ToString(12);
                                    WriteString(s);
                                    WriteLn
                                  END ToStringTest.
                                  """;
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("12\n", output1.ToString().NlFix());
        }

        [Fact]
        public void SystemToStringReal()
        {
            const string source = """
                                  MODULE ToStringTest;
                                  VAR
                                    s: STRING;

                                  BEGIN
                                    s := ToString(12.5);
                                    WriteString(s);
                                    WriteLn
                                  END ToStringTest.
                                  """;
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("12.5\n", output1.ToString().NlFix());
        }
    }
}
