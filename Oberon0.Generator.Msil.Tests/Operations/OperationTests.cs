using System;
using System.Collections.Generic;
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
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(source);

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
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(source);

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
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(source);

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
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(source);

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
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(source);

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
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(source);

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

    }
}
