#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardFunctionTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil.Tests/StandardFunctionTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.Tests.Libraries
{
    using System.Collections.Generic;

    using NUnit.Framework;

    using Oberon0.Compiler;
    using Oberon0.Compiler.Definitions;

    [TestFixture]
    public class StandardFunctionTests
    {
        [Test]
        public void TestReadToRecord()
        {
            const string Source = @"MODULE ReadToRecord;
VAR
  s: RECORD a : INTEGER END;

BEGIN
  ReadInt(s.a);
  s.a := s.a + 1;
  WriteInt(s.a);
  WriteLn
END ReadToRecord.";
            Module m = Oberon0Compiler.CompileString(Source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            var code = cg.DumpCode();

            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, new List<string> { "12" }, out var outputData, m));
            Assert.AreEqual("13\n", outputData.NlFix());
        }
    }
}