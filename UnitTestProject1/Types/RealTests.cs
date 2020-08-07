#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using NUnit.Framework;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Statements;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests.Types
{
    [TestFixture]
    public class RealTests
    {
        [Test]
        public void TestAssignmentBoolFail()
        {
            TestHelper.CompileString(
                @"MODULE TestAssignment;
VAR r: REAL;

BEGIN
    r := FALSE;
END TestAssignment.
",
                "Left & right side do not match types");
        }

        [Test]
        public void TestAssignmentBoolVarFail()
        {
            TestHelper.CompileString(
                @"MODULE TestAssignment;
VAR r: REAL;
   i: BOOLEAN;

BEGIN
    i := TRUE;
    r := i;
END TestAssignment.",
                "Left & right side do not match types");
        }

        [Test]
        public void TestAssignmentInt()
        {
            var m = TestHelper.CompileString(
                @"MODULE TestAssignment;
VAR r: REAL;

BEGIN
    r := 1;
END TestAssignment.
");
            Assert.NotNull(m);
            Assert.AreEqual(1, m.Block.Statements.Count);
            AssignmentStatement statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsInstanceOf<ConstantIntExpression>(statement.Expression);
            Assert.AreEqual("r:REAL := 1", statement.ToString());
        }

        [Test]
        public void TestAssignmentIntVar()
        {
            var m = TestHelper.CompileString(
                @"MODULE TestAssignment;
VAR r: REAL;
   i: INTEGER;

BEGIN
    i := 1;
    r := i;
END TestAssignment.");
            Assert.NotNull(m);
            Assert.AreEqual(2, m.Block.Statements.Count);
            AssignmentStatement statement = m.Block.Statements[1] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.AreEqual("r:REAL := i(INTEGER)", statement.ToString());
        }

        [Test]
        public void TestAssignmentMult()
        {
            var m = TestHelper.CompileString(
                @"MODULE TestAssignment;
VAR r: REAL;

BEGIN
    r := 2 * 1.5;
END TestAssignment.");
            Assert.NotNull(m);
            Assert.AreEqual(1, m.Block.Statements.Count);
            AssignmentStatement statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsInstanceOf<ConstantDoubleExpression>(statement.Expression);
            Assert.AreEqual($"r:REAL := 3", statement.ToString());
        }

        [Test]
        public void TestAssignmentReal()
        {
            var m = TestHelper.CompileString(
                @"MODULE TestAssignment;
VAR r: REAL;

BEGIN
    r := 1.234;
END TestAssignment.");
            Assert.NotNull(m);
            Assert.AreEqual(1, m.Block.Statements.Count);
            AssignmentStatement statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsInstanceOf<ConstantDoubleExpression>(statement.Expression);
            Assert.AreEqual($"r:REAL := {1.234}", statement.ToString());
        }

        [Test]
        public void TestAssignmentRealNeg()
        {
            var m = TestHelper.CompileString(
                @"MODULE TestAssignment;
VAR r: REAL;

BEGIN
    r := -1.234;
END TestAssignment.");
            Assert.NotNull(m);
            Assert.AreEqual(1, m.Block.Statements.Count);
            AssignmentStatement statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.IsInstanceOf<ConstantDoubleExpression>(statement.Expression);
            Assert.AreEqual($"r:REAL := {-1.234}", statement.ToString());
        }


        [Test]
        public void TestEpsilon()
        {
            var m = TestHelper.CompileString(
                @"MODULE TestAssignment;
VAR r: REAL;

BEGIN
    r := EPSILON;
END TestAssignment.");
            Assert.NotNull(m);
            Assert.AreEqual(1, m.Block.Statements.Count);
            AssignmentStatement statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.AreEqual($"r:REAL := {double.Epsilon}", statement.ToString());
        }


        [Test]
        public void TestEpsilonDiv2()
        {
            var m = TestHelper.CompileString(
                @"MODULE TestAssignment;
VAR r: REAL;

BEGIN
    r := EPSILON DIV 2;
END TestAssignment.");
            Assert.NotNull(m);
            Assert.AreEqual(1, m.Block.Statements.Count);
            AssignmentStatement statement = m.Block.Statements[0] as AssignmentStatement;
            Assert.NotNull(statement);
            Assert.AreEqual($"r:REAL := 0", statement.ToString());
        }
    }
}