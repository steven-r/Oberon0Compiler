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
    using NUnit.Framework;

    using Oberon0.Compiler;
    using Oberon0.Compiler.Definitions;

    [TestFixture]
    public class RealTests
    {
        [Test]
        public void TestRealFromConst()
        {
            CheckCodeReal("z := c;", $"{1.2}\n");
        }

        [Test]
        public void TestRealFromInt()
        {
            CheckCodeReal("z := 1;", "1\n");
        }

        [Test]
        public void TestRealFromIntConst()
        {
            CheckCodeReal("z := i;", "42\n");
        }

        [Test]
        public void TestRealFromReal()
        {
            CheckCodeReal("z := 1.2;", $"{1.2}\n");
        }

        [Test]
        public void TestRealFromPlus1()
        {
            CheckCodeReal("z := 1.2+1.2;", $"{2.4}\n");
        }

        [Test]
        public void TestRealFromPlus2()
        {
            CheckCodeReal("r := 1.2; z := r+1.2;", $"{2.4}\n");
        }

        [Test]
        public void TestRealFromPlus3()
        {
            CheckCodeReal("z := c+c;", $"{2.4}\n");
        }

        [Test]
        public void TestRealComplex1()
        {
            CheckCodeReal("r := 1.2345678; s := 7.2; z := r*s DIV 3;", $"{2.96296272}\n");
        }

        [Test]
        public void TestRealDiv0()
        {
            CheckCodeReal("r := 1.2345678; s := 0; z := r DIV s;", $"Infinity\n");
        }

        [Test]
        public void TestEpsilon()
        {
            string source = @"MODULE Test; 
CONST 
    expected = 10.8511834932;
VAR 
    r, s, z: REAL; 
    b, c: BOOLEAN;

BEGIN
    r := 1.5;
    s := 7.2341223288;
    z := r * s;
    b := (r - expected) < EPSILON;
    c := r = expected;
    WriteBool(b);
    WriteString(',');
    WriteBool(c);
    WriteLn 
END Test.";
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData, m));
            Assert.AreEqual($"{true},{false}\n", outputData.NlFix());
        }

        private void CheckCodeReal(string sourceCode, string expectedResults)
        {
            string source = $@"MODULE Test; CONST c = 1.2; i = 42; VAR r, s, t, x, y, z: REAL; 
BEGIN {sourceCode} IF isinfinity(z) THEN WriteString('Infinity') ELSE WriteReal(z) END; WriteLn END Test.";

            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData, m));
            Assert.AreEqual(expectedResults, outputData.NlFix());
        }
    }
}