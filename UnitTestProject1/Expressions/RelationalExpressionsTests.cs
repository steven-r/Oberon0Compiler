#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Linq;
using Xunit;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Solver;
using Oberon0.Compiler.Statements;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests.Expressions
{
    public class RelationalExpressionsTests
    {
        private Module CompileString(string operations, params string[] expectedErrors)
        {
            return TestHelper.CompileString(
                $"MODULE test; CONST true_ = TRUE; false_ = FALSE; VAR a,b,c,d,e,f,g,h,x,y,z: BOOLEAN; BEGIN {operations} END test.",
                expectedErrors);
        }

        [Fact]
        public void ExpressionNot()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(OberonGrammarLexer.NOT, new ConstantBoolExpression(true), null, m.Block, null);
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
                m.Block, 
                null);
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
                m.Block, 
                null);
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
                m.Block, 
                null);
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
                m.Block, 
                null);
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
                m.Block,
                null);
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
                m.Block,
                null);
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
                m.Block,
                null);
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
                m.Block,
                null);
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
                m.Block,
                null);
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
                m.Block,
                null);
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
                m.Block,
                null);
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
                m.Block,
                null);
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
                m.Block,
                null);
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
                m.Block,
                null);
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
                m.Block,
                null);
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
                m.Block,
                null);
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
                m.Block,
                null);
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
                m.Block,
                null);
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
            var m = CompileString("x := a & b");
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
            var m = CompileString("x := a & TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.Equal(OberonGrammarLexer.AND, binExpr.Operation.Metadata.Operation);
        }

        [Fact]
        public void TestAndConstError()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => CompileString("x := TRUE & 1"));
            Assert.Equal("Cannot find operation '&' (Bool, Int)", ex.Message);
        }

        [Fact]
        public void TestAndConstFalse()
        {
            var m = CompileString("x := TRUE & FALSE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.False(boolExpression.ToBool());
        }

        [Fact]
        public void TestAndConstTrue()
        {
            var m = CompileString("x := TRUE & TRUE");
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
            var m = CompileString("x := ~false_");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.True(boolExpression.ToBool());
        }

        [Fact]
        public void TestNotDirect()
        {
            var m = CompileString("x := ~FALSE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.True(boolExpression.ToBool());
        }

        [Fact]
        public void TestNotVarFalse()
        {
            var m = CompileString("a := FALSE; x := ~a");
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
            var m = CompileString("x := a OR b");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.Equal(OberonGrammarLexer.OR, binExpr.Operation.Metadata.Operation);
        }

        [Fact]
        public void TestOr2()
        {
            var m = CompileString("x := a OR TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.Equal(OberonGrammarLexer.OR, binExpr.Operation.Metadata.Operation);
        }

        [Fact]
        public void TestOrConstTrue()
        {
            var m = CompileString("x := FALSE OR TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.True(boolExpression.ToBool());
        }
    }
}