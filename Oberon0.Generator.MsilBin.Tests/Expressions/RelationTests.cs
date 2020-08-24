#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Generator.MsilBin.Tests.Expressions
{
    public class RelationTests
    {
        private readonly ITestOutputHelper _output;

        public RelationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void TestGreaterEqual()
        {
            const string source = @"
MODULE TestGreaterEqual;
VAR
  x, y: INTEGER;
  res: BOOLEAN;

BEGIN
    ReadInt(x);
    ReadInt(y);
    IF (x >= y) THEN res := TRUE ELSE res := FALSE END;
    WriteBool(res);
    WriteLn
END TestGreaterEqual.
";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, _output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output = new StringWriter();
            Runner.Execute(assembly, output, new StringReader("5" + Environment.NewLine + "3"));
            Assert.Equal($"{true}\n", output.ToString().NlFix());

            var sb = output.GetStringBuilder();
            sb.Remove(0, sb.Length);
            Runner.Execute(assembly, output, new StringReader("3" + Environment.NewLine + "5"));
            Assert.Equal($"{false}\n", output.ToString().NlFix());

            sb = output.GetStringBuilder();
            sb.Remove(0, sb.Length);
            Runner.Execute(assembly, output, new StringReader("5" + Environment.NewLine + "5"));
            Assert.Equal($"{true}\n", output.ToString().NlFix());
        }

        [Fact]
        public void TestLessEqual()
        {
            const string source = @"
MODULE TestLessEqual;
VAR
  x, y: INTEGER;
  res: BOOLEAN;

BEGIN
    ReadInt(x);
    ReadInt(y);
    IF (x <= y) THEN res := TRUE ELSE res := FALSE END;
    WriteBool(res);
    WriteLn
END TestLessEqual.
";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, _output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output = new StringWriter();
            Runner.Execute(assembly, output, new StringReader("5" + Environment.NewLine + "3"));
            Assert.Equal($"{false}\n", output.ToString().NlFix());

            var sb = output.GetStringBuilder();
            sb.Remove(0, sb.Length);
            Runner.Execute(assembly, output, new StringReader("5" + Environment.NewLine + "5"));
            Assert.Equal($"{true}\n", output.ToString().NlFix());

            sb = output.GetStringBuilder();
            sb.Remove(0, sb.Length);
            Runner.Execute(assembly, output, new StringReader("3" + Environment.NewLine + "5"));
            Assert.Equal($"{true}\n", output.ToString().NlFix());
        }
    }
}
