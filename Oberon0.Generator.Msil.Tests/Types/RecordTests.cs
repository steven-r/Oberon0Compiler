using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Tests;
using Oberon0.CompilerSupport;

namespace Oberon0.Generator.Msil.Tests.Types
{
    [TestFixture]
    public class RecordTests
    {
        [Test]
        public void Test1()
        {
            const string source = @"MODULE Test; 
TYPE 
  rType = RECORD
    a: INTEGER;
    b: STRING
  END;

VAR 
  demo: rType;

BEGIN
  demo.a := 1;
  WriteInt(demo.a);
  WriteLn
END Test.";

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


    static class Test1
    {
        class NEstedClass
        {
            public int a;
            public int b;
        }

        private static NEstedClass s;

        static void Test1Demo()
        {
            s = new NEstedClass();
            s.a = 1;
        }
    }
}
