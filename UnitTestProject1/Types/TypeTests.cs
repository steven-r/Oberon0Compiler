#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;
using Oberon0.TestSupport;
using Xunit;

namespace Oberon0.Compiler.Tests.Types
{
    public class TypeTests
    {
        [Fact]
        public void LookupType()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            Assert.NotNull(m.Block.LookupType("Demo"));
        }

        [Fact]
        public void LookupTypeFail()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            Assert.Null(m.Block.LookupType("?Unknown"));
        }

        [Fact]
        public void SimpleType()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            var t = m.Block.LookupType("Demo");
            var intType = m.Block.LookupType("INTEGER");
            Assert.NotNull(t);
            Assert.IsType<SimpleTypeDefinition>(t);
            var std = (SimpleTypeDefinition) t;
            Assert.Equal(intType.Type, std.Type);
        }

        [Fact]
        public void TypeDefinedTwiceError()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
TYPE
  Demo = INTEGER;
  Demo = INTEGER;

END Test.",
                "Type Demo declared twice");
        }

        [Fact]
        public void TypeEquality()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
TYPE
  Demo = INTEGER;
VAR
  i: INTEGER;
  j: Demo;

BEGIN
  i := 0;
  j := i+1;
  WriteInt(i); WriteString(', '); WriteInt(j); WriteLn
END Test.");

            var i = m.Block.LookupVar("i");
            Assert.NotNull(i);
            var j = m.Block.LookupVar("j");
            Assert.NotNull(j);
            Assert.Equal(6, m.Block.Statements.Count);
            var s1 = m.Block.Statements[0];
            var s2 = m.Block.Statements[1];
            Assert.IsType<AssignmentStatement>(s1);
            Assert.IsType<AssignmentStatement>(s2);
            var as1 = (AssignmentStatement) s1;
            var as2 = (AssignmentStatement) s2;
            Assert.IsType<ConstantIntExpression>(as1.Expression);
            Assert.IsType<BinaryExpression>(as2.Expression);
            Assert.Equal(as1.Expression.TargetType, as2.Expression.TargetType);
            Assert.NotEqual(as1.Expression.TargetType, j.Type);
            Assert.Equal(as1.Expression.TargetType.Type, j.Type.Type);
        }
    }
}
