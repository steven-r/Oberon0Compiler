#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using NUnit.Framework;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Statements;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests.Types
{
    [TestFixture]
    public class BoolTests
    {
        /// <summary>
        /// Cheat logic to cover not used functions required by interfaces / base classes
        /// </summary>
        [Test]
        public void TestAssignCheat()
        {
            var m = TestHelper.CompileString(
                @"MODULE TestAssignment;
VAR r: BOOLEAN;

BEGIN
    r := TRUE;
END TestAssignment.
");
            Assert.NotNull(m);
            Assert.AreEqual(1, m.Block.Statements.Count);
            AssignmentStatement statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsInstanceOf<ConstantBoolExpression>(statement.Expression);
            var boolExpression = statement.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.AreEqual($"r:BOOLEAN := {true}", statement.ToString());
            Assert.Throws<NotImplementedException>(() => boolExpression.ToDouble());
            Assert.Throws<NotImplementedException>(() => boolExpression.ToInt32());
        }

        [Test]
        public void TestAssignConst()
        {
            var m = TestHelper.CompileString(
                @"MODULE TestAssignment;
VAR r: BOOLEAN;

BEGIN
    r := TRUE;
END TestAssignment.
");
            Assert.NotNull(m);
            Assert.AreEqual(1, m.Block.Statements.Count);
            AssignmentStatement statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsInstanceOf<ConstantBoolExpression>(statement.Expression);
            Assert.AreEqual($"r:BOOLEAN := {true}", statement.ToString());
        }
    }
}
