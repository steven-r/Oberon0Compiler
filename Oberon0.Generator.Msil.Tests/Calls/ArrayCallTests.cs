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
    public class ArrayCallTests
    {
        [Test]
        public void TestArrayCallByValue()
        {
            string source = @"MODULE Test; 
VAR 
    arr: ARRAY 5 OF INTEGER; 

    PROCEDURE TestArray(a: ARRAY 5 OF INTEGER);
    BEGIN
        IF (a[1] # 1) THEN WriteString('a is 0') END;
        a[1] := 2
    END TestArray;

BEGIN
    arr[1] := 1;
    TestArray(arr);
    WriteBool(arr[1] = 1);
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
        public void TestArrayCallByReference()
        {
            string source = @"MODULE Test; 
TYPE 
  tArray = ARRAY 5 OF INTEGER;
VAR 
    arr: tArray; 

    PROCEDURE TestArray(VAR a: tArray);
    BEGIN
        IF (a[1] # 1) THEN WriteString('a is 0') END;
        a[1] := 2
    END TestArray;

BEGIN
    arr[1] := 1;
    TestArray(arr);
    WriteBool(arr[1] = 2);
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