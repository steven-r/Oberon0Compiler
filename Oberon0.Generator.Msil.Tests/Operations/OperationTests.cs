#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil.Tests/OperationTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.Tests.Operations
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using NUnit.Framework;

    using Oberon0.Compiler;
    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Tests;
    using Oberon0.CompilerSupport;

    [TestFixture]
    public class OperationTests
    {
        [Test]
        public void TestAddConstConst()
        {
            string source = @"MODULE Array;
BEGIN
  WriteInt(1+2);
  WriteLn
END Array.";
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }

            var code = sb.ToString();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual("3\n", outputData.NlFix());
        }

        [Test]
        public void TestAddConstVar()
        {
            string source = @"MODULE Array;
VAR
  a: INTEGER;

BEGIN
  a := 1;
  WriteInt(2+a);
  WriteLn
END Array.";
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }

            string code = sb.ToString();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual("3\n", outputData.NlFix());
        }

        [Test]
        public void TestAddVarConst()
        {
            string source = @"MODULE Array;
VAR
  a: INTEGER;

BEGIN
  a := 1;
  WriteInt(a+2);
  WriteLn
END Array.";
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }

            string code = sb.ToString();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual("3\n", outputData.NlFix());
        }

        [Test]
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
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }

            string code = sb.ToString();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual("-1\n", outputData.NlFix());
        }

        [Test]
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
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }

            string code = sb.ToString();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual("3\n", outputData.NlFix());
        }

        [Test]
        public void TestMulVarNegConstVar()
        {
            string source = @"MODULE Array;
VAR
  a: INTEGER;
  b: INTEGER;

BEGIN
  a := 1;
  b := -2;
  WriteInt(b*a);
  WriteLn
END Array.";
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }

            string code = sb.ToString();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual("-2\n", outputData.NlFix());
        }

        [Test]
        public void TestNegAssignmentInt()
        {
            string source = @"MODULE Array;
VAR
  b: INTEGER;

BEGIN
  b := -2;
  WriteInt(b)
END Array.";
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }

            string code = sb.ToString();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual("-2\n", outputData.NlFix());
        }

        [Test]
        public void TestNegIntVar()
        {
            string source = @"MODULE Array;
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
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }

            string code = sb.ToString();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual("-1\n1\n", outputData.NlFix());
        }

        [Test]
        public void TestNegReal()
        {
            string source = @"MODULE Array;
BEGIN
  WriteReal(-2.5)
END Array.";
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }

            string code = sb.ToString();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual($"{-2.5}\n", outputData.NlFix());
        }

        [Test]
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
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(source, errors);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }

            string code = sb.ToString();
            Assert.AreEqual(0, errors.Count());
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual($"{-1.5}\n{1.5}\n", outputData.NlFix());
        }
    }
}