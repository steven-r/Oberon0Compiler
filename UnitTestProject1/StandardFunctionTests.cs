#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardFunctionTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/StandardFunctionTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests
{
    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.TestSupport;

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