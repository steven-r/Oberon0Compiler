#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using NUnit.Framework;
using Oberon0.Compiler.Definitions;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests
{
    [TestFixture]
    public class StandardFunctionTests
    {
        [Test]
        public void TestReadInt()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  Demo : INTEGER;
BEGIN
  ReadInt(Demo)
END Test.");
            Assert.AreEqual(1, m.Block.Statements.Count);
        }

        [Test]
        public void TestReadIntFailBoolType()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
  Demo : BOOLEAN;
BEGIN
  ReadInt(Demo)
END Test.",
                "No procedure/function with prototype 'ReadInt(BOOLEAN)' found");
        }

        [Test]
        public void TestReadIntFailNumber()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
  Demo : BOOLEAN;
BEGIN
  ReadInt(1)
END Test.",
                "No procedure/function with prototype 'ReadInt(INTEGER)' found");
        }

        [Test]
        public void TestReadIntFailString()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
  Demo : STRING;
BEGIN
  ReadInt(Demo)
END Test.",
                "No procedure/function with prototype 'ReadInt(STRING)' found");
        }

        [Test]
        public void TestReadReal()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  Demo : REAL;
BEGIN
  ReadReal(Demo)
END Test.");
            Assert.AreEqual(1, m.Block.Statements.Count);
        }

        [Test]
        public void TestReadRealFailIntType()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
  Demo : INTEGER;
BEGIN
  ReadReal(Demo)
END Test.",
                "No procedure/function with prototype 'ReadReal(INTEGER)' found");
        }
    }
}