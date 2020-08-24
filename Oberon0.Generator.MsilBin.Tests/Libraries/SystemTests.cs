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
    public class SystemTests
    {
        private readonly ITestOutputHelper _output;

        public SystemTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void SystemToStringBool()
        {
            const string source = @"MODULE ToStringTest;
VAR
  s: STRING;

BEGIN
  s := ToString(TRUE);
  WriteString(s);
  WriteLn
END ToStringTest.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, _output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output = new StringWriter();
            Runner.Execute(assembly, output);
            Assert.Equal("True\n", output.ToString().NlFix());
        }

        [Fact]
        public void SystemToStringInt()
        {
            const string source = @"MODULE ToStringTest;
VAR
  s: STRING;

BEGIN
  s := ToString(12);
  WriteString(s);
  WriteLn
END ToStringTest.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, _output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output = new StringWriter();
            Runner.Execute(assembly, output);
            Assert.Equal("12\n", output.ToString().NlFix());
        }

        [Fact]
        public void SystemToStringReal()
        {
            const string source = @"MODULE ToStringTest;
VAR
  s: STRING;

BEGIN
  s := ToString(12.5);
  WriteString(s);
  WriteLn
END ToStringTest.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, _output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output = new StringWriter();
            Runner.Execute(assembly, output);
            Assert.Equal("12.5\n", output.ToString().NlFix());
        }
    }
}
