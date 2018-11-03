#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatementTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil.Tests/StatementTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.Tests.Statements
{
    using System.IO;
    using System.Text;

    using NUnit.Framework;

    using Oberon0.Compiler;
    using Oberon0.Compiler.Definitions;

    [TestFixture]
    public class StatementTests
    {
        [Test]
        public void RepeatTest()
        {
            string source = @"MODULE Test; 
VAR 
  i: INTEGER;

BEGIN
  i := 1;
  REPEAT
      WriteInt(i);
      WriteLn;
      i := i+1;
  UNTIL i > 5
END Test.";

            Module m = Oberon0Compiler.CompileString(source);

            CodeGenerator cg = new CodeGenerator(m);

            cg.Generate();
            StringBuilder sb = new StringBuilder();
            using (StringWriter w = new StringWriter(sb))
            {
                cg.DumpCode(w);
            }

            var code = sb.ToString();
            Assert.IsTrue(MsilTestHelper.CompileRunTest(code, null, out var outputData, m));
            Assert.AreEqual("1\n2\n3\n4\n5\n", outputData.NlFix());
        }
    }
}