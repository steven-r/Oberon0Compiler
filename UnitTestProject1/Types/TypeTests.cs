#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/TypeTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests.Types
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Statements;
    using Oberon0.Compiler.Types;
    using Oberon0.TestSupport;

    [TestFixture]
    public class TypeTests
    {
        [Test]
        public void LookupType()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            Assert.NotNull(m.Block.LookupType("Demo"));
        }

        [Test]
        public void LookupTypeFail()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            Assert.IsNull(m.Block.LookupType("?Unknown"));
        }

        [Test]
        public void SimpleType()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            var t = m.Block.LookupType("Demo");
            var intType = m.Block.LookupType("INTEGER");
            Assert.NotNull(t);
            Assert.IsInstanceOf<SimpleTypeDefinition>(t);
            SimpleTypeDefinition std = (SimpleTypeDefinition)t;
            Assert.AreEqual(intType.Type, std.Type);
        }

        [Test]
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

        [Test]
        public void TypeEquality()
        {
            Module m = TestHelper.CompileString(
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
            Assert.AreEqual(6, m.Block.Statements.Count);
            var s1 = m.Block.Statements[0];
            var s2 = m.Block.Statements[1];
            Assert.IsInstanceOf<AssignmentStatement>(s1);
            Assert.IsInstanceOf<AssignmentStatement>(s2);
            var as1 = (AssignmentStatement)s1;
            var as2 = (AssignmentStatement)s2;
            Assert.IsInstanceOf<ConstantIntExpression>(as1.Expression);
            Assert.IsInstanceOf<BinaryExpression>(as2.Expression);
            Assert.AreEqual(as1.Expression.TargetType, as2.Expression.TargetType);
            Assert.AreNotEqual(as1.Expression.TargetType, j.Type);
            Assert.AreEqual(as1.Expression.TargetType.Type, j.Type.Type);
        }
    }
}