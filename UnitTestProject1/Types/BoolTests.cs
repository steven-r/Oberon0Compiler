#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using Xunit;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Statements;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests.Types
{
    public class BoolTests
    {
        /// <summary>
        /// Cheat logic to cover not used functions required by interfaces / base classes
        /// </summary>
        [Fact]
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
            Assert.Single(m.Block.Statements);
            AssignmentStatement statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsType<ConstantBoolExpression>(statement.Expression);
            var boolExpression = statement.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.Equal($"r:BOOLEAN := {true}", statement.ToString());
            Assert.Throws<NotImplementedException>(() => boolExpression.ToDouble());
            Assert.Throws<NotImplementedException>(() => boolExpression.ToInt32());
        }

        [Fact]
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
            Assert.Single(m.Block.Statements);
            AssignmentStatement statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsType<ConstantBoolExpression>(statement.Expression);
            Assert.Equal($"r:BOOLEAN := {true}", statement.ToString());
        }
    }
}
