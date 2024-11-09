#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Statements;
using Oberon0.Test.Support;
using Xunit;

namespace Oberon0.Compiler.Tests.Types
{
    public class RealTests
    {
        [Fact]
#pragma warning disable S2699
        public void TestAssignmentBoolFail()
#pragma warning restore S2699
        {
            TestHelper.CompileString(
                """
                MODULE TestAssignment;
                VAR r: REAL;

                BEGIN
                    r := FALSE;
                END TestAssignment.

                """,
                "Left & right side do not match types");
        }

        [Fact]
#pragma warning disable S2699
        public void TestAssignmentBoolVarFail()
#pragma warning restore S2699
        {
            TestHelper.CompileString(
                """
                MODULE TestAssignment;
                VAR r: REAL;
                   i: BOOLEAN;

                BEGIN
                    i := TRUE;
                    r := i;
                END TestAssignment.
                """,
                "Left & right side do not match types");
        }

        [Fact]
        public void TestAssignmentInt()
        {
            var m = TestHelper.CompileString(
                """
                MODULE TestAssignment;
                VAR r: REAL;

                BEGIN
                    r := 1;
                END TestAssignment.

                """);
            Assert.NotNull(m);
            Assert.Single(m.Block.Statements);
            var statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsType<ConstantIntExpression>(statement.Expression);
            Assert.Equal("r:REAL := 1", statement.ToString());
        }

        [Fact]
        public void TestAssignmentIntVar()
        {
            var m = TestHelper.CompileString(
                """
                MODULE TestAssignment;
                VAR r: REAL;
                   i: INTEGER;

                BEGIN
                    i := 1;
                    r := i;
                END TestAssignment.
                """);
            Assert.NotNull(m);
            Assert.Equal(2, m.Block.Statements.Count);
            var statement = m.Block.Statements[1] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.Equal("r:REAL := i(INTEGER)", statement.ToString());
        }

        [Fact]
        public void TestAssignmentMult()
        {
            var m = TestHelper.CompileString(
                """
                MODULE TestAssignment;
                VAR r: REAL;

                BEGIN
                    r := 2 * 1.5;
                END TestAssignment.
                """);
            Assert.NotNull(m);
            Assert.Single(m.Block.Statements);
            var statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsType<ConstantDoubleExpression>(statement.Expression);
            Assert.Equal("r:REAL := 3", statement.ToString());
        }

        [Fact]
        public void TestAssignmentReal()
        {
            var m = TestHelper.CompileString(
                """
                MODULE TestAssignment;
                VAR r: REAL;

                BEGIN
                    r := 1.234;
                END TestAssignment.
                """);
            Assert.NotNull(m);
            Assert.Single(m.Block.Statements);
            var statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsType<ConstantDoubleExpression>(statement.Expression);
            Assert.Equal($"r:REAL := 1.234", statement.ToString());
        }

        [Fact]
        public void TestAssignmentRealNeg()
        {
            var m = TestHelper.CompileString(
                """
                MODULE TestAssignment;
                VAR r: REAL;

                BEGIN
                    r := -1.234;
                END TestAssignment.
                """);
            Assert.NotNull(m);
            Assert.Single(m.Block.Statements);
            var statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsType<ConstantDoubleExpression>(statement.Expression);
            Assert.Equal($"r:REAL := -1.234", statement.ToString());
        }


        [Fact]
        public void TestEpsilon()
        {
            var m = TestHelper.CompileString(
                """
                MODULE TestAssignment;
                VAR r: REAL;

                BEGIN
                    r := EPSILON;
                END TestAssignment.
                """);
            Assert.NotNull(m);
            Assert.Single(m.Block.Statements);
            var statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.Equal($"r:REAL := EPSILON(REAL)", statement.ToString());
        }


        [Fact]
        public void TestEpsilonDiv2()
        {
            var m = TestHelper.CompileString(
                """
                MODULE TestAssignment;
                VAR r: REAL;

                BEGIN
                    r := EPSILON DIV 2;
                END TestAssignment.
                """);
            Assert.NotNull(m);
            Assert.Single(m.Block.Statements);
            var statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            // value is not optimized away
            Assert.Equal($"r:REAL := DIV (REAL, INTEGER) -> REAL", statement.ToString());
        }
    }
}
