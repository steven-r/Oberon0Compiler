#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil.Tests/RecordTests.cs
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
    public class RecordTests
    {
        [Test]
        public void Test1()
        {
            string source = @"MODULE Test; 
TYPE 
  rType = RECORD
    a: INTEGER;
    b: STRING
  END;

VAR 
  demo: rType;

BEGIN
  demo.a := 1;
  WriteInt(demo.a);
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
    }
}