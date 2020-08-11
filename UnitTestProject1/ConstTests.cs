#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Xunit;
using Oberon0.Compiler.Definitions;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests
{
    public class ConstTests
    {
        [Fact]
        public void ConstConstExpr()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test;
CONST
  Test1 = 2;
  Test = 1+Test1;

 END Test.");

            var t = m.Block.LookupVar("Test");
            var t1 = m.Block.LookupVar("Test1");
            Assert.NotNull(t);
            Assert.NotNull(t1);
            Assert.IsType<ConstDeclaration>(t);
            Assert.IsType<ConstDeclaration>(t1);
            var tp = (ConstDeclaration)t;
            var tp1 = (ConstDeclaration)t1;
            Assert.Equal("Test", tp.Name);
            Assert.Same(m.Block.LookupType("INTEGER"), tp.Type);
            Assert.Equal(3, tp.Value.ToInt32());

            Assert.Equal("Test1", tp1.Name);
            Assert.Same(m.Block.LookupType("INTEGER"), tp1.Type);
            Assert.Equal(2, tp1.Value.ToInt32());
            Assert.Equal("Const Test1 = 2", t1.ToString());
        }

        [Fact]
        public void ConstSimple()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test;
CONST
  Test = 1;

 END Test.");

            var c = m.Block.LookupVar("Test");
            Assert.NotNull(c);
            Assert.IsType<ConstDeclaration>(c);
            var cp = (ConstDeclaration)c;
            Assert.Equal("Test", cp.Name);
            Assert.Same(m.Block.LookupType("INTEGER"), cp.Type);
            Assert.Equal(1, cp.Value.ToInt32());
        }

        [Fact]
        public void ConstSimpleExpr()
        {
            Module m = Oberon0Compiler.CompileString(
                @"MODULE Test;
CONST
  Test = 1+1;

 END Test.");

            var c = m.Block.LookupVar("Test");
            Assert.NotNull(c);
            Assert.IsType<ConstDeclaration>(c);
            var cp = (ConstDeclaration)c;
            Assert.Equal("Test", cp.Name);
            Assert.Equal(m.Block.LookupType("INTEGER"), cp.Type);
            Assert.Equal(2, cp.Value.ToInt32());
        }

        [Fact]
        public void ConstSimpleFailDuplicate()
        {
            TestHelper.CompileString(
                @"MODULE Test;
CONST
  Test = 1;
  Test = 2;

 END Test.",
                "A variable/constant with this name has been defined already");
        }

        [Fact]
        public void ConstSimpleFailVarReference()
        {
            TestHelper.CompileString(
                @"MODULE Test;
VAR
  Test : INTEGER;
CONST
  Test1 = 2+Test;

 END Test.",
                "A constant must resolve during compile time");
        }
    }
}