#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using System.Linq;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Statements;
using Oberon0.Test.Support;
using Xunit;

namespace Oberon0.Compiler.Tests
{
    public class SimpleStatementTests
    {
        [Fact]
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

        [Fact]
        public void SimpleAssignment()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 1
END Test.
");
            Assert.Single(m.Block.Statements);
            Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[0]);
            var ast = (AssignmentStatement) m.Block.Statements[0];
            Assert.Equal("x", ast.Variable.Name);
            Assert.IsAssignableFrom<ConstantIntExpression>(ast.Expression);
            var cie = (ConstantIntExpression) ast.Expression;
            Assert.Equal(1, cie.Value);
        }

        [Fact]
        public void SimpleRepeat()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 1;
    REPEAT
        
    UNTIL x > 0
END Test.
");
            Assert.Equal(2, m.Block.Statements.Count);
            Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[0]);
            var ast = (AssignmentStatement) m.Block.Statements[0];
            Assert.Equal("x", ast.Variable.Name);
            Assert.IsAssignableFrom<ConstantIntExpression>(ast.Expression);
            var cie = (ConstantIntExpression) ast.Expression;
            Assert.Equal(1, cie.Value);

            Assert.IsAssignableFrom<RepeatStatement>(m.Block.Statements[1]);
            var rs = (RepeatStatement) m.Block.Statements[1];
            Assert.Empty(rs.Block.Statements);
        }

        [Fact]
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

        [Fact]
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
            Assert.Equal(2, m.Block.Statements.Count);
            Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[1]);
            var ast = (AssignmentStatement) m.Block.Statements[1];
            var expr = ast.Expression as BinaryExpression;
            Assert.NotNull(expr);
            Assert.False(expr.IsUnary);
            Assert.False(expr.IsConst);
            Assert.Equal("y:INTEGER := PLUS (INTEGER, INTEGER) -> INTEGER", ast.ToString());
        }

        [Fact]
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

        [Fact]
        public void TestAssignableBoolInt()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: BOOLEAN;

BEGIN 
    x := 1;
END Test.
");
            Assert.Single(m.Block.Statements);
            Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[0]);
            var ast = (AssignmentStatement) m.Block.Statements[0];
            Assert.Equal("x", ast.Variable.Name);
            Assert.IsAssignableFrom<ConstantIntExpression>(ast.Expression);
            var cie = (ConstantIntExpression) ast.Expression;
            Assert.Equal(1, cie.Value);
        }

        [Fact]
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
            Assert.Single(errors);
            Assert.Equal("Left & right side do not match types", errors.First().Message);
        }

        [Fact]
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

        [Fact]
        public void TestAssignableUnaryInt()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 0;
    x := -x;
END Test.
");
            Assert.Equal(2, m.Block.Statements.Count);
            Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[1]);
            var ast = (AssignmentStatement) m.Block.Statements[1];
            var expr = ast.Expression as UnaryExpression;
            Assert.NotNull(expr);
            Assert.True(expr.IsUnary);
            Assert.False(expr.IsConst);
            Assert.Equal("x:INTEGER := MINUS (INTEGER) -> INTEGER", ast.ToString());
        }

        [Fact]
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

        [Fact]
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
            Assert.Single(errors);
            Assert.Equal("Left & right side do not match types", errors.First().Message);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        /// <summary>
        ///     The two statements.
        /// </summary>
        [Fact]
        public void TwoStatements()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 1;
    x := 2
END Test.
");
            Assert.Equal(2, m.Block.Statements.Count);
            Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[0]);
            var ast = (AssignmentStatement) m.Block.Statements[0];
            Assert.Equal("x", ast.Variable.Name);
            Assert.IsAssignableFrom<ConstantIntExpression>(ast.Expression);
            var cie = (ConstantIntExpression) ast.Expression;
            Assert.Equal(1, cie.Value);

            Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[1]);
            ast = (AssignmentStatement) m.Block.Statements[1];
            Assert.Equal("x", ast.Variable.Name);
            Assert.IsAssignableFrom<ConstantIntExpression>(ast.Expression);
            cie = (ConstantIntExpression) ast.Expression;
            Assert.Equal(2, cie.Value);
        }
    }
}
