#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Linq;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Solver;
using Oberon0.Compiler.Statements;
using Oberon0.Test.Support;
using Xunit;

namespace Oberon0.Compiler.Tests.Expressions
{
    public class RelationalExpressionsTests
    {
        [Fact]
        public void ExpressionNot()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(OberonGrammarLexer.NOT, new ConstantBoolExpression(true), null, m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.False(result.ToBool());
        }

        [Fact]
        public void ExpressionNot2()
        {
            var m = new Module(null);
            m.Block.Declarations.Add(new Declaration("a", m.Block.LookupType("BOOLEAN")));
            var e = BinaryExpression.Create(
                OberonGrammarLexer.NOT,
                VariableReferenceExpression.Create(m.Block.LookupVar("a"), null),
                null,
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as BinaryExpression;
            Assert.NotNull(result);
        }

        [Fact]
        public void ExpressionRelEquals()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.EQUAL,
                ConstantExpression.Create(4),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.True(result.ToBool());
        }

        [Fact]
        public void ExpressionRelEquals2()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.EQUAL,
                ConstantExpression.Create(4),
                ConstantExpression.Create(10),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.False(result.ToBool());
        }

        [Fact]
        public void ExpressionRelGe()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.GE,
                ConstantExpression.Create(10),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.True(result.ToBool());
        }

        [Fact]
        public void ExpressionRelGe1()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.GE,
                ConstantExpression.Create(4),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.True(result.ToBool());
        }

        [Fact]
        public void ExpressionRelGe2()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.GE,
                ConstantExpression.Create(4),
                ConstantExpression.Create(10),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.False(result.ToBool());
        }

        [Fact]
        public void ExpressionRelGt()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.GT,
                ConstantExpression.Create(10),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.True(result.ToBool());
        }

        [Fact]
        public void ExpressionRelGt1()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.GT,
                ConstantExpression.Create(4),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.False(result.ToBool());
        }

        [Fact]
        public void ExpressionRelGt2()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.GT,
                ConstantExpression.Create(4),
                ConstantExpression.Create(10),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.False(result.ToBool());
        }

        [Fact]
        public void ExpressionRelLe()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.LE,
                ConstantExpression.Create(10),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.False(result.ToBool());
        }

        [Fact]
        public void ExpressionRelLe1()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.LE,
                ConstantExpression.Create(4),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.True(result.ToBool());
        }

        [Fact]
        public void ExpressionRelLe2()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.LT,
                ConstantExpression.Create(4),
                ConstantExpression.Create(10),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.True(result.ToBool());
        }

        [Fact]
        public void ExpressionRelLt()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.LT,
                ConstantExpression.Create(10),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.False(result.ToBool());
        }

        [Fact]
        public void ExpressionRelLt1()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.LT,
                ConstantExpression.Create(4),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.False(result.ToBool());
        }

        [Fact]
        public void ExpressionRelLt2()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.LT,
                ConstantExpression.Create(4),
                ConstantExpression.Create(10),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.True(result.ToBool());
        }

        [Fact]
        public void ExpressionRelNotEquals()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.NOTEQUAL,
                ConstantExpression.Create(4),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.False(result.ToBool());
        }

        [Fact]
        public void ExpressionRelNotEquals2()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.NOTEQUAL,
                ConstantExpression.Create(4),
                ConstantExpression.Create(10),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.True(result.ToBool());
        }

        [Fact]
        public void ExpressionRelVar()
        {
            var m = new Module(null);
            m.Block.Declarations.Add(new Declaration("a", m.Block.LookupType("INTEGER")));
            var e = BinaryExpression.Create(
                OberonGrammarLexer.NOTEQUAL,
                VariableReferenceExpression.Create(m.Block.LookupVar("a"), null),
                ConstantExpression.Create(10),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as BinaryExpression;
            Assert.NotNull(result);
            Assert.False(result.IsConst);
        }

        [Fact]
        public void ExpressionVarNotFound()
        {
            var m = new Module(null);
            m.Block.Declarations.Add(new Declaration("a", m.Block.LookupType("BOOLEAN")));
            var var = VariableReferenceExpression.Create(m.Block.LookupVar("b"), null);
            Assert.Null(var);
        }

        /* and */
        [Fact]
        public void TestAnd1()
        {
            var m = TestHelper.CompileSingleStatement("x := a & b");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.Equal(OberonGrammarLexer.AND, binExpr.Operation.Metadata.Operation);
            Assert.Equal(OberonGrammarLexer.AND, binExpr.Operator);
        }

        [Fact]
        public void TestAnd2()
        {
            var m = TestHelper.CompileSingleStatement("x := a & TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.Equal(OberonGrammarLexer.AND, binExpr.Operation.Metadata.Operation);
        }

        [Fact]
        public void TestAndConstError()
        {
            var ex = Assert.Throws<ArgumentException>(() => TestHelper.CompileSingleStatement("x := TRUE & 1"));
            Assert.Equal("Cannot find operation '&' (Bool, Int)", ex.Message);
        }

        [Fact]
        public void TestAndConstFalse()
        {
            var m = TestHelper.CompileSingleStatement("x := TRUE & FALSE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.False(boolExpression.ToBool());
        }

        [Fact]
        public void TestAndConstTrue()
        {
            var m = TestHelper.CompileSingleStatement("x := TRUE & TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.True(boolExpression.ToBool());
        }

        [Fact]
        public void TestIntAssign()
        {
            var m = TestHelper.CompileString(
                "MODULE test; CONST true_ = TRUE; false_ = FALSE; VAR a,b,c,d,e,f,g,h,x,y,z: BOOLEAN; BEGIN b := 1 END test.");

            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantIntExpression;
            Assert.NotNull(boolExpression);
            Assert.Equal(1, boolExpression.ToInt32());
            Assert.True(boolExpression.ToBool());
        }

        [Fact]
        public void TestNotConst()
        {
            var m = TestHelper.CompileSingleStatement("x := ~false_");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.True(boolExpression.ToBool());
        }

        [Fact]
        public void TestNotDirect()
        {
            var m = TestHelper.CompileSingleStatement("x := ~FALSE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.True(boolExpression.ToBool());
        }

        [Fact]
        public void TestNotVarFalse()
        {
            var m = TestHelper.CompileSingleStatement("a := FALSE; x := ~a");
            Assert.Equal(2, m.Block.Statements.Count);
            var assignment = m.Block.Statements[1] as AssignmentStatement;
            Assert.NotNull(assignment);
            var binaryExpression = assignment.Expression as BinaryExpression;
            Assert.NotNull(binaryExpression);
            var varRef = binaryExpression.LeftHandSide as VariableReferenceExpression;
            Assert.NotNull(varRef);
            Assert.Equal("a", varRef.Declaration.Name);
        }

        /* or */
        [Fact]
        public void TestOr1()
        {
            var m = TestHelper.CompileSingleStatement("x := a OR b");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.Equal(OberonGrammarLexer.OR, binExpr.Operation.Metadata.Operation);
        }

        [Fact]
        public void TestOr2()
        {
            var m = TestHelper.CompileSingleStatement("x := a OR TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.Equal(OberonGrammarLexer.OR, binExpr.Operation.Metadata.Operation);
        }

        [Fact]
        public void TestOrConstTrue()
        {
            var m = TestHelper.CompileSingleStatement("x := FALSE OR TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.True(boolExpression.ToBool());
        }

        [Fact]
        public void ExpressionConstantStringReturnsIdentity()
        {
            const string test = "This is a test";

            var m = new Module(null);
            var e = new StringExpression(test);
            var result = ConstantSolver.Solve(e, m.Block) as StringExpression;
            Assert.NotNull(result);
            Assert.True(result.IsConst);
            Assert.Equal(test, result.Value);
        }

        [Fact]
        public void TestEqConstFalseTrueFalse()
        {
            var m = TestHelper.CompileSingleStatement("x := FALSE = TRUE");
            var assignment = Assert.IsType<AssignmentStatement>(m.Block.Statements.First());
            var boolExpression = Assert.IsType<ConstantBoolExpression>(assignment.Expression);
            Assert.False(boolExpression.ToBool());
        }

        [Fact]
        public void TestEqConstFalseFalseTrue()
        {
            var m = TestHelper.CompileSingleStatement("x := FALSE = FALSE");
            var assignment = Assert.IsType<AssignmentStatement>(m.Block.Statements.First());
            var boolExpression = Assert.IsType<ConstantBoolExpression>(assignment.Expression);
            Assert.True(boolExpression.ToBool());
        }

        [Fact]
        public void TestNotEqConstFalseFalseFalse()
        {
            var m = TestHelper.CompileSingleStatement("x := FALSE # FALSE");
            var assignment = Assert.IsType<AssignmentStatement>(m.Block.Statements.First());
            var boolExpression = Assert.IsType<ConstantBoolExpression>(assignment.Expression);
            Assert.False(boolExpression.ToBool());
        }

        [Fact]
        public void TestNotEqConstFalseTrueTrue()
        {
            var m = TestHelper.CompileSingleStatement("x := FALSE # TRUE");
            var assignment = Assert.IsType<AssignmentStatement>(m.Block.Statements.First());
            var boolExpression = Assert.IsType<ConstantBoolExpression>(assignment.Expression);
            Assert.True(boolExpression.ToBool());
        }

        [Fact]
        public void TestModIntInt()
        {
            var m = TestHelper.CompileSingleStatement("i := 10 MOD 3");
            var assignment = Assert.IsType<AssignmentStatement>(m.Block.Statements.First());
            var intExpression = Assert.IsType<ConstantIntExpression>(assignment.Expression);
            Assert.Equal(1, intExpression.ToInt32());
        }

        [Fact]
        public void TestModIntDouble()
        {
            var m = TestHelper.CompileSingleStatement("r := 10.3 MOD 3");
            var assignment = Assert.IsType<AssignmentStatement>(m.Block.Statements.First());
            var result = Assert.IsType<ConstantDoubleExpression>(assignment.Expression);
            Assert.True(Math.Abs(1.3 - result.ToDouble()) < double.Epsilon);
        }

        [Fact]
        public void ConstantSolverReturnRegularExpression()
        {
            var m = TestHelper.CompileSingleStatement("s := 5.2; r := 10.3 MOD s");
            var assignment = Assert.IsType<AssignmentStatement>(m.Block.Statements.Last());
            var bin = Assert.IsType<BinaryExpression>(assignment.Expression);
            Assert.Equal(OberonGrammarLexer.MOD, bin.Operator);
            Assert.Equal("REAL", bin.TargetType.Name);
        }
    }
}
