#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/ArrayTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests.Types
{
    using System.Linq;

    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Statements;
    using Oberon0.Compiler.Types;
    using Oberon0.TestSupport;

    [TestFixture]
    public class ArrayTests
    {
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

        [Test, Ignore("Wait for issue #36", Until = "2018-12-31")]
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
        [Ignore("Wait for issue #37", Until = "2018-12-31")]
        public void ArrayFail1TestIndex5()
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
            Assert.AreEqual(1, m.Block.Statements);
            Assert.IsInstanceOf<AssignmentStatement>(m.Block.Statements[0]);
            var statement = (AssignmentStatement)m.Block.Statements[0];

            Assert.NotNull(statement.Selector);
            Assert.IsInstanceOf<IndexSelector>(statement.Selector.First());

            var selector = (IndexSelector)statement.Selector.First();
            Assert.IsTrue(selector.IndexDefinition.IsConst);
            Assert.AreEqual(5, ((ConstantExpression)selector.IndexDefinition).Value);
        }

    }
}