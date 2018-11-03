#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelationTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil.Tests/RelationTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.Tests.Expressions
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.TestSupport;

    [TestFixture]
    public class RelationTests
    {
        [Test]
        public void TestGreaterEqual()
        {
            string source = @"
MODULE TestGreaterEqual;
VAR
  x, y: INTEGER;
  res: BOOLEAN;

BEGIN
    ReadInt(x);
    ReadInt(y);
    IF (x >= y) THEN res := TRUE ELSE res := FALSE END;
    WriteBool(res);
    WriteLn
END TestGreaterEqual.
";
            Module m = TestHelper.CompileString(source);
            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "5", "3" }, out var outputData, m));
            Assert.AreEqual($"{true}\n".NlFix(), outputData.NlFix());

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "3", "5" }, out outputData, m));
            Assert.AreEqual($"{false}\n".NlFix(), outputData.NlFix());

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "5", "5" }, out outputData, m));
            Assert.AreEqual($"{true}\n".NlFix(), outputData.NlFix());
        }

        [Test]
        public void TestLessEqual()
        {
            string source = @"
MODULE TestLessEqual;
VAR
  x, y: INTEGER;
  res: BOOLEAN;

BEGIN
    ReadInt(x);
    ReadInt(y);
    IF (x <= y) THEN res := TRUE ELSE res := FALSE END;
    WriteBool(res);
    WriteLn
END TestLessEqual.
";
            Module m = TestHelper.CompileString(source);
            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "5", "3" }, out var outputData, m));
            Assert.AreEqual($"{false}\n".NlFix(), outputData.NlFix());

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "3", "5" }, out outputData, m));
            Assert.AreEqual($"{true}\n".NlFix(), outputData.NlFix());

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "5", "5" }, out outputData, m));
            Assert.AreEqual($"{true}\n".NlFix(), outputData.NlFix());
        }
    }
}