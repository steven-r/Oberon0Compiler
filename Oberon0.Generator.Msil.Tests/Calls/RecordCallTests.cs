#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayCallTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil.Tests/ArrayCallTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.Tests.Calls
{
    using NUnit.Framework;

    using Oberon0.Compiler;
    using Oberon0.Compiler.Definitions;

    [TestFixture]
    public class RecordCallTests
    {
        [Test]
        public void TestRecordCallByValue()
        {
            string source = @"MODULE Test;
TYPE
   rType = RECORD
     a: INTEGER;
     b: REAL;
     c: BOOLEAN
   END;

VAR 
    r: rType;

    PROCEDURE TestRecord(r: rType);
    BEGIN
        IF (r.a # 1) THEN WriteString(' a error') END;
        IF ~r.c THEN WriteString('c error') END;
        IF ABS(r.b - 1.2345) > EPSILON THEN WriteString('b error') END;
        r.a := -1;
        r.b := -1.2345;
        r.c := FALSE;
    END TestRecord;

BEGIN
    r.a := 1;
    r.b := 1.2345;
    r.c := TRUE;
    TestRecord(r);
    WriteBool((r.a = 1) & (ABS(r.b - 1.2345) < EPSILON) & r.c);
    WriteLn 
END Test.";
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData, m));
            Assert.AreEqual($"{true}\n", outputData.NlFix());
        }

        [Test]
        public void TestRecordCallByReference()
        {
            string source = @"MODULE Test; 
TYPE
   rType = RECORD
     a: INTEGER;
     b: REAL;
     c: BOOLEAN
   END;

VAR 
    r: rType;

    PROCEDURE TestRecord(VAR r: rType);
    BEGIN
        IF (r.a # 1) THEN WriteString(' a error') END;
        IF ~r.c THEN WriteString('c error') END;
        IF ABS(r.b - 1.2345) > EPSILON THEN WriteString('b error') END;
        r.a := -1;
        r.b := -1.2345;
        r.c := FALSE;
    END TestRecord;

BEGIN
    r.a := 1;
    r.b := 1.2345;
    r.c := TRUE;
    TestRecord(r);
    WriteBool((r.a = -1) & (ABS(r.b - 1.2345) > EPSILON) & ~r.c);
    WriteLn 
END Test.";
            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData, m));
            Assert.AreEqual($"{true}\n", outputData.NlFix());
        }
    }
}