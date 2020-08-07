#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Linq;
using NUnit.Framework;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests.Types
{
    [TestFixture]
    public class ArrayTests
    {
        [Test]
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
                "Array index out of bounds");
        }

        [Test]
        public void ArrayTestIndex5()
        {
            Module m = TestHelper.CompileString(
                @"MODULE test; 
TYPE 
    aType= ARRAY 5 OF INTEGER;
VAR 
  a : aType;
                
BEGIN
    a[5] := 1;
END test.");
            Assert.NotNull(m);
            Assert.AreEqual(1, m.Block.Statements.Count);
            Assert.IsInstanceOf<AssignmentStatement>(m.Block.Statements[0]);
            var statement = (AssignmentStatement)m.Block.Statements[0];

            Assert.NotNull(statement.Selector);
            Assert.IsInstanceOf<IndexSelector>(statement.Selector.First());

            var selector = (IndexSelector)statement.Selector.First();
            Assert.IsTrue(selector.IndexDefinition.IsConst);
            Assert.AreEqual(5, ((ConstantExpression)selector.IndexDefinition).ToInt32());
        }

        [Test]
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
                "Array reference expected");
        }

        [Test]
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
                "Array reference must be INTEGER");
        }

        [Test]
        public void ArrayType()
        {
            Module m = TestHelper.CompileString(
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
            Assert.AreEqual("ARRAY 5 OF INTEGER", arrayType.ToString());
        }
    }
}