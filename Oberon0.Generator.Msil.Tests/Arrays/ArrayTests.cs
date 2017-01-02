using System.IO;
using System.Text;
using NUnit.Framework;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Generator.Msil.Tests.Arrays
{
    [TestFixture]
    public class ArrayTests
    {
        [Test]
        public void SimpleArrayDefinition()
        {
            const string source = @"MODULE Array;
VAR 
  a: ARRAY 32 OF INTEGER;
  b: ARRAY 32 OF BOOLEAN;

END Array.";
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();

            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }
            string outputData;
            Assert.IsTrue(MsilTestHelper.CompileRunTest(sb.ToString(), null, out outputData));
            Assert.IsTrue(string.IsNullOrEmpty(outputData));
        }

        [Test]
        public void SimpleArrayDefinitionLocal()
        {
            const string source = @"MODULE Array;
VAR 
  a: ARRAY 32 OF INTEGER;
  b: ARRAY 32 OF INTEGER;
  i: INTEGER;
  s: INTEGER;

BEGIN
  i := 0;
  WHILE (i < 32) DO
    a[i] := i;
    b[i] := 32-i;
    i := i+1
  END;
  i := 0;
  WHILE (i < 32) DO
    s := s + a[i] + b[i];
    i := i+1
  END;
  WriteInt(s);
  WriteLn
END Array.";
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            string code = cg.DumpCode();
            string outputData;
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out outputData));
            Assert.AreEqual("1024\n", outputData.NlFix());
        }


        [Test]
        public void SimpleArraySetAndGetValueGlobal()
        {
            const string source = @"MODULE Array;
VAR 
  a: ARRAY 32 OF INTEGER;
  n: INTEGER;

BEGIN
  a[0] := 1;
  n := a[0];
  WriteInt(n);
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
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out outputData));
            Assert.AreEqual("1\n", outputData.NlFix());
        }
    }
}