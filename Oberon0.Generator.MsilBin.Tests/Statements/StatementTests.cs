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

namespace Oberon0.Generator.MsilBin.Tests.Statements
{
    public class StatementTests(ITestOutputHelper output)
    {
        [Fact]
        public void RepeatTest()
        {
            const string source = @"MODULE Test; 
VAR 
  i: INTEGER;

BEGIN
  i := 1;
  REPEAT
      WriteInt(i);
      WriteLn;
      i := i+1;
  UNTIL i > 5
END Test.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("1\n2\n3\n4\n5\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestIssue25If()
        {
            const string source = @"MODULE Issue25; 
VAR 
  x: BOOLEAN;

BEGIN
    x := TRUE;
    IF (x) THEN WriteString('Yes') ELSE WriteString('No') END;
    WriteLn
END Issue25.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("Yes\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestIssue25Repeat()
        {
            const string source = @"MODULE Issue25; 
VAR 
  x: BOOLEAN;

BEGIN
    x := FALSE;
    REPEAT
        WriteString('Yes'); 
        x := TRUE
    UNTIL x;
    WriteLn
END Issue25.";

            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("Yes\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestIssue25While()
        {
            const string source = @"MODULE Issue25; 
VAR 
  x: BOOLEAN;

BEGIN
    x := FALSE;
    WriteString('Yes');
    WHILE x DO WriteString('No'); x := TRUE END;
    WriteLn
END Issue25.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.True(assembly != null);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("Yes\n", output1.ToString().NlFix());
        }
    }
}
