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

namespace Oberon0.Generator.MsilBin.Tests.Operations
{
    public class OperationTests(ITestOutputHelper output)
    {
        [Fact]
        public void TestAddConstConst()
        {
            const string source = @"MODULE Array;
BEGIN
  WriteInt(1+2);
  WriteLn
END Array.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("3\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestAddConstVar()
        {
            const string source = @"MODULE Array;
VAR
  a: INTEGER;

BEGIN
  a := 1;
  WriteInt(2+a);
  WriteLn
END Array.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("3\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestAddVarConst()
        {
            const string source = @"MODULE Array;
VAR
  a: INTEGER;

BEGIN
  a := 1;
  WriteInt(a+2);
  WriteLn
END Array.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("3\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestAddVarNegConstVar()
        {
            string source = @"MODULE Array;
VAR
  a: INTEGER;
  b: INTEGER;

BEGIN
  a := 1;
  b := -2;
  WriteInt(b+a);
  WriteLn
END Array.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("-1\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestAddVarVar()
        {
            string source = @"MODULE Array;
VAR
  a: INTEGER;
  b: INTEGER;

BEGIN
  a := 1;
  b := 2;
  WriteInt(b+a);
  WriteLn
END Array.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("3\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestEotFalse()
        {
            const string source = @"MODULE Array;
VAR
  a, b,c: BOOLEAN;

BEGIN
  b := eot();
  WriteBool(b);
  WriteLn;
END Array.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1, new StringReader("true" + Environment.NewLine));
            Assert.Equal($"{false}\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestMulVarNegConstVar()
        {
            const string source = @"MODULE Array;
VAR
  a: INTEGER;
  b: INTEGER;

BEGIN
  a := 1;
  b := -2;
  WriteInt(b*a);
  WriteLn
END Array.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("-2\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestNegAssignmentInt()
        {
            const string source = @"MODULE Array;
VAR
  b: INTEGER;

BEGIN
  b := -2;
  WriteInt(b);
  WriteLn
END Array.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("-2\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestNegIntVar()
        {
            const string source = @"MODULE Array;
VAR
  a: INTEGER;

BEGIN
  a := 1;
  a := -a;
  WriteInt(a);
  WriteLn;
  WriteInt(-a);
  WriteLn;
END Array.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal("-1\n1\n", output1.ToString().NlFix());
        }

        //TODO: Move to appropriate test section
        [Fact]
        public void TestNegReal()
        {
            const string source = @"MODULE Array;
BEGIN
  WriteReal(-2.5);
  WriteLn
END Array.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal($"{-2.5}\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestNegRealVar()
        {
            string source = @"MODULE Array;
VAR
  a: REAL;

BEGIN
  a := 1.5;
  a := -a;
  WriteReal(a);
  WriteLn;
  WriteReal(-a);
  WriteLn
END Array.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal($"{-1.5}\n{1.5}\n", output1.ToString().NlFix());
        }

        [Fact]
        public void TestReadBoolVar()
        {
            string source = @"MODULE Array;
VAR
  a,b: BOOLEAN;

BEGIN
  ReadBool(a);
  ReadBool(b);
  WriteBool(a);
  WriteLn;
  WriteBool(b);
  WriteLn;
END Array.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1,
                new StringReader("true" + Environment.NewLine + "false" + Environment.NewLine));
            Assert.Equal($"{true}\n{false}\n", output1.ToString().NlFix());
        }

        //TODO: Seems the wrong place
        [Fact]
        public void TestReadIntVar()
        {
            const string source = @"MODULE Array;
VAR
  a: INTEGER;

BEGIN
  ReadInt(a);
  WriteInt(a);
  WriteLn;
END Array.";
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);

            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1, new StringReader("12" + Environment.NewLine));
            Assert.Equal("12\n", output1.ToString().NlFix());
        }
    }
}
