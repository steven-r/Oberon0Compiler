using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;
using Oberon0.Generator.Msil.Tests.Arrays;

namespace Oberon0.Generator.Msil.Tests.Operations
{
    [TestFixture]
    public class OperationTests
    {
        [Test]
        public void TestAddConstConst()
        {
            const string source = @"MODULE Array;
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
            string code = sb.ToString();
            string outputData;
            Assert.IsTrue(TestHelper.CompileRunTest(code, null, out outputData));
            Assert.AreEqual("3\n", outputData.NlFix());
        }

        [Test]
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
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }
            string code = sb.ToString();
            string outputData;
            Assert.IsTrue(TestHelper.CompileRunTest(code, null, out outputData));
            Assert.AreEqual("3\n", outputData.NlFix());
        }

        [Test]
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
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }
            string code = sb.ToString();
            string outputData;
            Assert.IsTrue(TestHelper.CompileRunTest(code, null, out outputData));
            Assert.AreEqual("3\n", outputData.NlFix());
        }


        [Test]
        public void TestAddVarVar()
        {
            const string source = @"MODULE Array;
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
            string outputData;
            Assert.IsTrue(TestHelper.CompileRunTest(code, null, out outputData));
            Assert.AreEqual("3\n", outputData.NlFix());
        }

        [Test]
        public void TestAddVarNegConstVar()
        {
            const string source = @"MODULE Array;
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
            string outputData;
            Assert.IsTrue(TestHelper.CompileRunTest(code, null, out outputData));
            Assert.AreEqual("-1\n", outputData.NlFix());
        }

        [Test]
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
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }
            string code = sb.ToString();
            string outputData;
            Assert.IsTrue(TestHelper.CompileRunTest(code, null, out outputData));
            Assert.AreEqual("-2\n", outputData.NlFix());
        }

        [Test]
        public void TestNegAssignmentInt()
        {
            const string source = @"MODULE Array;
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
            string outputData;
            Assert.IsTrue(TestHelper.CompileRunTest(code, null, out outputData));
            Assert.AreEqual("-2\n", outputData.NlFix());
        }

        [Test]
        public void TestNegReal()
        {
            const string source = @"MODULE Array;
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
            string outputData;
            Assert.IsTrue(TestHelper.CompileRunTest(code, null, out outputData));
            Assert.AreEqual($"{-2.5}\n", outputData.NlFix());
        }

    }
}
