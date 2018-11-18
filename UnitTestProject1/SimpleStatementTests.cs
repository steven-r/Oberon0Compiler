#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleStatementTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/SimpleStatementTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Statements;
    using Oberon0.TestSupport;

    [TestFixture]
    public class SimpleStatementTests
    {
        [Test]
        public void InvalidParameterCount()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
BEGIN 
    WriteInt
END Test.
",
                "No procedure/function with prototype 'WriteInt()' found");
        }

        [Test]
        public void SimpleAssignment()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 1
END Test.
");
            Assert.AreEqual(1, m.Block.Statements.Count);
            Assert.IsAssignableFrom(typeof(AssignmentStatement), m.Block.Statements[0]);
            AssignmentStatement ast = (AssignmentStatement)m.Block.Statements[0];
            Assert.AreEqual("x", ast.Variable.Name);
            Assert.IsAssignableFrom(typeof(ConstantIntExpression), ast.Expression);
            ConstantIntExpression cie = (ConstantIntExpression)ast.Expression;
            Assert.AreEqual(1, cie.Value);
        }

        [Test]
        public void SimpleRepeat()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 1;
    REPEAT
        
    UNTIL x > 0
END Test.
");
            Assert.AreEqual(2, m.Block.Statements.Count);
            Assert.IsAssignableFrom(typeof(AssignmentStatement), m.Block.Statements[0]);
            AssignmentStatement ast = (AssignmentStatement)m.Block.Statements[0];
            Assert.AreEqual("x", ast.Variable.Name);
            Assert.IsAssignableFrom(typeof(ConstantIntExpression), ast.Expression);
            ConstantIntExpression cie = (ConstantIntExpression)ast.Expression;
            Assert.AreEqual(1, cie.Value);

            Assert.IsAssignableFrom(typeof(RepeatStatement), m.Block.Statements[1]);
            RepeatStatement rs = (RepeatStatement)m.Block.Statements[1];
            Assert.AreEqual(0, rs.Block.Statements.Count);
        }

        [Test]
        public void SimpleRepeatFailCondition()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 1;
    REPEAT
        
    UNTIL 0
END Test.
",
                "The condition needs to return a logical condition");
        }

        /// <summary>
        /// The two statements.
        /// </summary>
        [Test]
        public void TwoStatements()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 1;
    x := 2
END Test.
");
            Assert.AreEqual(2, m.Block.Statements.Count);
            Assert.IsAssignableFrom(typeof(AssignmentStatement), m.Block.Statements[0]);
            AssignmentStatement ast = (AssignmentStatement)m.Block.Statements[0];
            Assert.AreEqual("x", ast.Variable.Name);
            Assert.IsAssignableFrom(typeof(ConstantIntExpression), ast.Expression);
            ConstantIntExpression cie = (ConstantIntExpression)ast.Expression;
            Assert.AreEqual(1, cie.Value);

            Assert.IsAssignableFrom(typeof(AssignmentStatement), m.Block.Statements[1]);
            ast = (AssignmentStatement)m.Block.Statements[1];
            Assert.AreEqual("x", ast.Variable.Name);
            Assert.IsAssignableFrom(typeof(ConstantIntExpression), ast.Expression);
            cie = (ConstantIntExpression)ast.Expression;
            Assert.AreEqual(2, cie.Value);
        }

        [Test]
        public void TestAssignableBoolInt()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: BOOLEAN;

BEGIN 
    x := 1;
END Test.
");
            Assert.AreEqual(1, m.Block.Statements.Count);
            Assert.IsAssignableFrom(typeof(AssignmentStatement), m.Block.Statements[0]);
            AssignmentStatement ast = (AssignmentStatement)m.Block.Statements[0];
            Assert.AreEqual("x", ast.Variable.Name);
            Assert.IsAssignableFrom(typeof(ConstantIntExpression), ast.Expression);
            ConstantIntExpression cie = (ConstantIntExpression)ast.Expression;
            Assert.AreEqual(1, cie.Value);
        }

        [Test]
        public void TestAssignableUnaryInt()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 0;
    x := -x;
END Test.
");
            Assert.AreEqual(2, m.Block.Statements.Count);
            Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[1]);
            AssignmentStatement ast = (AssignmentStatement)m.Block.Statements[1];
            UnaryExpression expr = ast.Expression as UnaryExpression;
            Assert.NotNull(expr);
            Assert.IsTrue(expr.IsUnary);
            Assert.IsFalse(expr.IsConst);
            Assert.AreEqual("x:INTEGER := MINUS (INTEGER) -> INTEGER", ast.ToString());
        }

        [Test]
        public void TestAssignArray()
        {
            var errors = new List<CompilerError>();
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: ARRAY 5 OF INTEGER;
  y: ARRAY 5 OF INTEGER;

BEGIN 
    x := y;
END Test.
", 
                errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Left & right side do not match types", errors.First().Message);
        }

        [Test]
        public void TestAssignableBoolReal()
        {
            var errors = new List<CompilerError>();
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: BOOLEAN;

BEGIN 
    x := 1.234;
END Test.
",
                errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Left & right side do not match types", errors.First().Message);
        }

        [Test]
        public void TestAssignableAddVars()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x, y: INTEGER;

BEGIN 
    x := 2;
    y := x + y
END Test.
");
            Assert.That(m.Block.Statements.Count, Is.EqualTo(2));
            Assert.That(m.Block.Statements[1], Is.AssignableTo<AssignmentStatement>());
            AssignmentStatement ast = (AssignmentStatement)m.Block.Statements[1];
            BinaryExpression expr = ast.Expression as BinaryExpression;
            Assert.That(expr, Is.Not.Null);
            Assert.That(expr.IsUnary, Is.False);
            Assert.That(expr.IsConst, Is.False);
            Assert.That(ast.ToString(), Is.EqualTo("y:INTEGER := PLUS (INTEGER, INTEGER) -> INTEGER"));
        }

        [Test]
        public void TestAssignableVarNotFound()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := y
END Test.
",
                "Unknown identifier: y");
        }

        [Test]
        public void TestAssignableArraySimpleFail()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
TYPE
  t = RECORD a: INTEGER END;
  a = ARRAY 5 OF INTEGER;

VAR
  x : t;
  y : a;

BEGIN 
    y := x
END Test.
",
                "Left & right side do not match types");
        }

        [Test]
        public void TestAssignableFailSymbol()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: ARRAY 5 OF INTEGER;

BEGIN 
    x[1] = 2
END Test.
",
                "mismatched input '=' expecting ':='",
                "Cannot parse right side of assignment");
        }

        [Test]
        public void TestAssignFailVarNotFound()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    y := 2
END Test.
",
                "Variable y not known");
        }

        [Test]
        public void TestWhileNotBool()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
BEGIN 
    WHILE 1+1 DO
    END
END Test.
",
                "The condition needs to return a logical condition");
        }

        [Test]
        public void TestRepeatNotBool()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
BEGIN 
    REPEAT
    UNTIL 1+1
END Test.
",
                "The condition needs to return a logical condition");
        }

        [Test]
        public void TestIfNotBool()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
BEGIN 
    IF 1+1 THEN WriteString('Yes') ELSE WriteString ('No') END
END Test.
",
                "The condition needs to return a logical condition");
        }
    }
}