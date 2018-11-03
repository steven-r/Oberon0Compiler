#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RealWorldTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil.Tests/RealWorldTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.Tests.Complex
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.TestSupport;

    [TestFixture]
    public class RealWorldTests
    {
        [Test]
        public void Divide()
        {
            string source = @"
MODULE DivisionTest;
VAR
  x, y, q, r: INTEGER;

PROCEDURE Divide(x: INTEGER; y: INTEGER; VAR q: INTEGER; VAR r: INTEGER);
VAR 
    w: INTEGER;
    negatex: BOOLEAN;
    negatey: BOOLEAN;

BEGIN 
    negatex := x < 0;
    negatey := y < 0;
    IF (negatex) THEN x := -x END;
    IF (negatey) THEN y := -y END;
    r := x; 
    q := 0; 
    w := y;
    WHILE w <= r DO 
        w := 2*w 
    END ;
    WHILE w > y DO
        q := 2*q; 
        w := w DIV 2;
        IF w <= r THEN 
            r := r - w; 
            q := q + 1 
        END
    END;
    IF (negatex & ~negatey) OR (~negatex & negatey) THEN
        q := -q;
        r := -r
    END
END Divide;

BEGIN 
 ReadInt(x);
 ReadInt(y);
 Divide(x, y, q, r);
 WriteInt(q);
 WriteString('/');
 WriteInt(r);
 WriteLn
END DivisionTest.
";
            Module m = TestHelper.CompileString(source);
            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "20", "2" }, out var outputData, m));
            Assert.AreEqual("10/0\n".NlFix(), outputData.NlFix());

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "15", "-5" }, out outputData, m));
            Assert.AreEqual("-3/0\n".NlFix(), outputData.NlFix());

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "20", "3" }, out outputData, m));
            Assert.AreEqual("6/2\n".NlFix(), outputData.NlFix());
        }

        [Test]
        public void Multiply()
        {
            string source = @"
MODULE MultiplyTest;
VAR
  x, y, z: INTEGER;

    PROCEDURE Multiply(x: INTEGER; y: INTEGER; VAR z: INTEGER);
    VAR
      negate : BOOLEAN;
    BEGIN 
        negate := x < 0;
        IF (negate) THEN x := -x END;
        z := 0;
        WHILE x > 0 DO
            IF x MOD 2 = 1 THEN 
                z := z + y 
            END;
            y := 2*y; 
            x := x DIV 2
        END ;
        IF (negate) THEN z := -z END;
    END Multiply;

BEGIN 
 ReadInt(x);
 ReadInt(y);
 Multiply(x, y, z);
 WriteInt(z);
 WriteLn
END MultiplyTest.
";
            Module m = TestHelper.CompileString(source);
            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "5", "3" }, out var outputData, m));
            Assert.AreEqual("15\n".NlFix(), outputData.NlFix());

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "5", "0" }, out outputData, m));
            Assert.AreEqual("0\n".NlFix(), outputData.NlFix());

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "20", "1" }, out outputData, m));
            Assert.AreEqual("20\n".NlFix(), outputData.NlFix());

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "1", "20" }, out outputData, m));
            Assert.AreEqual("20\n".NlFix(), outputData.NlFix());

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "-10", "-20" }, out outputData, m));
            Assert.AreEqual("200\n".NlFix(), outputData.NlFix());

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "15", "-20" }, out outputData, m));
            Assert.AreEqual("-300\n".NlFix(), outputData.NlFix());

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "-15", "20" }, out outputData, m));
            Assert.AreEqual("-300\n".NlFix(), outputData.NlFix());
        }
    }
}