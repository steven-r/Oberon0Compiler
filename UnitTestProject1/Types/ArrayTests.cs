#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Linq;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;
using Oberon0.Test.Support;
using Xunit;

namespace Oberon0.Compiler.Tests.Types
{
    public class ArrayTests
    {
        [Fact]
        public void ArrayFail1TestIndex0()
        {
            TestHelper.CompileString(
                @"
MODULE test; 
TYPE 
    aType= ARRAY 5 OF INTEGER;
VAR 
  a : aType;
                
BEGIN
    a[0] := 1;
END test.",
                "Array index out of bounds", "Left & right side do not match types");
        }

        [Fact]
        public void ArrayTestIndex5()
        {
            var m = TestHelper.CompileString(
                @"MODULE test; 
TYPE 
    aType= ARRAY 5 OF INTEGER;
VAR 
  a : aType;
                
BEGIN
    a[5] := 1;
END test.");
            Assert.NotNull(m);
            Assert.Single(m.Block.Statements);
            Assert.IsType<AssignmentStatement>(m.Block.Statements[0]);
            var statement = (AssignmentStatement) m.Block.Statements[0];

            Assert.NotNull(statement.Selector);
            Assert.IsType<IndexSelector>(statement.Selector.First());

            var selector = (IndexSelector) statement.Selector.First();
            Assert.True(selector.IndexDefinition.IsConst);
            Assert.Equal(5, ((ConstantExpression) selector.IndexDefinition).ToInt32());
        }

        [Fact]
        public void ArrayTestIndexArraySimple()
        {
            TestHelper.CompileString(
                @"MODULE test; 
TYPE 
    aType= ARRAY 5 OF INTEGER;
VAR 
  a : RECORD b: INTEGER END;
                
BEGIN
    a[1] := 1;
END test.",
                "Array reference expected", "Left & right side do not match types");
        }

        [Fact]
        public void ArrayTestIndexRealFail()
        {
            TestHelper.CompileString(
                @"MODULE test; 
TYPE 
    aType= ARRAY 5 OF INTEGER;
VAR 
  a : aType;
                
BEGIN
    a[1.234] := 1;
END test.",
                "Array reference must be INTEGER", "Left & right side do not match types");
        }

        [Fact]
        public void ArrayType()
        {
            var m = TestHelper.CompileString(
                @"
MODULE test; 
TYPE 
    aType= ARRAY 5 OF INTEGER;
VAR 
  a : aType;
                
END test.");
            var type = m.Block.LookupType("aType");
            Assert.NotNull(type);
            var arrayType = type as ArrayTypeDefinition;
            Assert.NotNull(arrayType);
            Assert.Equal("ARRAY 5 OF INTEGER", arrayType.ToString());
        }
    }
}
