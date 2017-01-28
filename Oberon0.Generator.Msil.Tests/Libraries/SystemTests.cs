using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Generator.Msil.Tests.Libraries
{
    [TestFixture]
    public class SystemTests
    {
        [Test]
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
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out outputData, m));
            Assert.AreEqual("12\n", outputData.NlFix());
        }

        [Test]
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
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out outputData, m));
            Assert.AreEqual("12.5\n", outputData.NlFix());
        }

        [Test]
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
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out outputData, m));
            Assert.AreEqual("True\n", outputData.NlFix());
        }

    }
}
