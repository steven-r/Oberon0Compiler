#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil.Tests/ArrayTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.Tests.Arrays
{
    using System.IO;
    using System.Text;

    using NUnit.Framework;

    using Oberon0.Compiler;
    using Oberon0.Compiler.Definitions;

    [TestFixture]
    public class ArrayTests
    {
        [Test]
        public void SimpleArrayDefinition()
        {
            const string Source = @"MODULE Array;
VAR 
  a: ARRAY 32 OF INTEGER;
  b: ARRAY 32 OF BOOLEAN;

END Array.";
            Module m = Oberon0Compiler.CompileString(Source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();

            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }

            Assert.IsTrue(MsilTestHelper.CompileRunTest(sb.ToString(), null, out var outputData));
            Assert.IsTrue(string.IsNullOrEmpty(outputData));
        }

        [Test]
        public void SimpleArrayDefinitionLocal()
        {
            const string Source = @"MODULE Array;
VAR 
  a: ARRAY 32 OF INTEGER;
  b: ARRAY 32 OF INTEGER;
  i: INTEGER;
  s: INTEGER;

BEGIN
  i := 0;
  WHILE (i < 32) DO
    a[i] := i;
    b[i] := 32-i;
    i := i+1
  END;
  i := 0;
  WHILE (i < 32) DO
    s := s + a[i] + b[i];
    i := i+1
  END;
  WriteInt(s);
  WriteLn
END Array.";
            Module m = Oberon0Compiler.CompileString(Source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual("1024\n", outputData.NlFix());
        }

        [Test]
        public void SimpleArraySetAndGetValueGlobal()
        {
            const string Source = @"MODULE Array;
VAR 
  a: ARRAY 32 OF INTEGER;
  n: INTEGER;

BEGIN
  a[0] := 1;
  n := a[0];
  WriteInt(n);
  WriteLn
END Array.";
            Module m = Oberon0Compiler.CompileString(Source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();

            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }

            var code = sb.ToString();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData));
            Assert.AreEqual("1\n", outputData.NlFix());
        }
    }
}