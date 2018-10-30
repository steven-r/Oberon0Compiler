#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RealTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil.Tests/RealTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.Tests.Types
{
    using System.IO;
    using System.Text;

    using NUnit.Framework;

    using Oberon0.Compiler;
    using Oberon0.Compiler.Definitions;

    [TestFixture]
    public class RealTests
    {
        [Test]
        public void TestRealFromConst()
        {
            string source = @"MODULE Test; 
CONST
  c = 1.2;

VAR 
  r: REAL;

BEGIN
  r := c;
  WriteReal(r);
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
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual($"{1.2}\n", outputData.NlFix());
        }

        [Test]
        public void TestRealFromInt()
        {
            string source = @"MODULE Test; 
VAR 
  r: REAL;

BEGIN
  r := 1;
  WriteReal(r);
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
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual("1\n", outputData.NlFix());
        }

        [Test]
        public void TestRealFromIntConst()
        {
            string source = @"MODULE Test;
CONST
  c = 42;

VAR 
  r: REAL;

BEGIN
  r := c;
  WriteReal(r);
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
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual("42\n", outputData.NlFix());
        }

        [Test]
        public void TestRealFromReal()
        {
            string source = @"MODULE Test; 
VAR 
  r: REAL;

BEGIN
  r := 1.2;
  WriteReal(r);
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
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual($"{1.2}\n", outputData.NlFix());
        }
    }
}