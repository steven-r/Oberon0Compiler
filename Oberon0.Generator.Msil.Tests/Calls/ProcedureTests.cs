#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcedureTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil.Tests/ProcedureTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.Tests.Calls
{
    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.TestSupport;

    [TestFixture]
    public class ProcedureTests
    {
        [Test]
        public void TestProcedureBoolean()
        {
            string source = @"
MODULE ProcTest;
CONST
  true_ = TRUE;

VAR
  x, y: BOOLEAN;
  i, j: BOOLEAN;

  PROCEDURE Test(x: BOOLEAN; y: BOOLEAN; VAR a: BOOLEAN; b: BOOLEAN);
  BEGIN
    x := FALSE; y := true_;
    a := TRUE; b := FALSE;
  END Test;

BEGIN
    x := TRUE; y := x = true_;
    i := FALSE; j := FALSE;
    Test(y, x, i, j);
    WriteBool(x);
    WriteString(', ');
    WriteBool(y);
    WriteString(', ');
    WriteBool(i);
    WriteString(', ');
    WriteBool(j);
    WriteLn
END ProcTest.
";
            Module m = TestHelper.CompileString(source);
            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData, m));
            Assert.AreEqual($"{true}, {true}, {true}, {false}\n".NlFix(), outputData.NlFix());
        }

        [Test]
        public void TestProcedureInteger()
        {
            string source = @"
MODULE ProcTest;
VAR
  x, y: INTEGER;
  i, j: INTEGER;

  PROCEDURE Test(x: INTEGER; y: INTEGER; VAR a: INTEGER; b: INTEGER);
  BEGIN
    x := 0; y := -1;
    a := 10; b := 20;
  END Test;

BEGIN
    x := 42; y := 1;
    i := 2; j := 0;
    Test(y, x, i, j);
    WriteBool(x = 42);
    WriteString(', ');
    WriteBool(y = 1);
    WriteString(', ');
    WriteBool(i = 10);
    WriteString(', ');
    WriteBool(j = 0);
    WriteLn
END ProcTest.
";
            Module m = TestHelper.CompileString(source);
            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData, m));
            Assert.AreEqual($"{true}, {true}, {true}, {true}\n".NlFix(), outputData.NlFix());
        }
    }
}