using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oberon0.Generator.Msil.Tests.Types
{
    using NUnit.Framework;

    using Oberon0.Compiler;
    using Oberon0.Compiler.Definitions;

    [TestFixture]
    public class VarTests
    {
        [Test]
        public void TestReservedWordIssue23()
        {
            string source = @"MODULE Test; 
VAR 
    int32: INTEGER; 

BEGIN
    int32 := 1;
    WriteInt(int32);
    WriteLn 
END Test.";
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData, m));
            Assert.AreEqual("1\n", outputData.NlFix());
        }
    }
}
