#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using Oberon0.TestSupport;
using Xunit;

namespace Oberon0.Compiler.Tests
{
    public class StandardFunctionTests
    {
        [Fact]
        public void TestReadInt()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  Demo : INTEGER;
BEGIN
  ReadInt(Demo)
END Test.");
            Assert.Single(m.Block.Statements);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void TestReadReal()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  Demo : REAL;
BEGIN
  ReadReal(Demo)
END Test.");
            Assert.Single(m.Block.Statements);
        }

        [Fact]
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
