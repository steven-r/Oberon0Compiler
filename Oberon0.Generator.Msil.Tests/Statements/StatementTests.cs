using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oberon0.Generator.Msil.Tests.Statements
{
    using System.IO;

    using NUnit.Framework;

    using Oberon0.Compiler;
    using Oberon0.Compiler.Definitions;

    [TestFixture]
    public class StatementTests
    {
        [Test]
        public void RepeatTest()
        {
            string source = @"MODULE Test; 
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
            Assert.AreEqual("1\n2\n3\n4\n5\n", outputData.NlFix());

        }
    }
}
