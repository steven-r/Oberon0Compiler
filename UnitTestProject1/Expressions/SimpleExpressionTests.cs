#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Solver;
using Oberon0.Compiler.Types;
using Xunit;

namespace Oberon0.Compiler.Tests.Expressions
{
    public class SimpleExpressionTests
    {
        [Fact]
        public void ExpressionAdd1()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.PLUS,
                ConstantExpression.Create(1),
                ConstantExpression.Create(1),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.NotNull(result);
            Assert.Equal(2, result.ToInt32());
        }

        [Fact]
        public void ExpressionAdd2()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.PLUS,
                ConstantExpression.Create(1),
                ConstantExpression.Create(1.42),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.NotNull(result);
            Assert.Equal(2.42, result.ToDouble());
        }

        [Fact]
        public void ExpressionAddRes0()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.PLUS,
                ConstantExpression.Create(1),
                ConstantDoubleExpression.Zero,
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.NotNull(result);
            Assert.Equal(1, result.ToInt32());
            Assert.Equal(1.0, result.ToDouble());
            Assert.True(result.ToBool());
            Assert.False(result.IsUnary);
            Assert.True(result.IsConst);
        }

        [Fact]
        public void ExpressionAnd()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.AND,
                ConstantExpression.Create(false),
                ConstantExpression.Create("true"),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.NotNull(result);
            Assert.False(result.ToBool());
        }

        [Fact]
        public void ExpressionDiv0()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.DIV,
                ConstantExpression.Create("10.0"),
                ConstantIntExpression.Zero,
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.NotNull(result);
            Assert.True(double.IsInfinity(result.ToDouble()));
        }

        [Fact]
        public void ExpressionDiv1()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.DIV,
                ConstantExpression.Create(10),
                ConstantExpression.Create(2),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.NotNull(result);
            Assert.Equal(5, result.ToInt32());
        }

        [Fact]
        public void ExpressionDiv2()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.DIV,
                ConstantExpression.Create(10),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.NotNull(result);
            Assert.Equal(2, result.ToInt32());
        }

        [Fact]
        public void ExpressionDiv3()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.DIV,
                ConstantExpression.Create("10.0"),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.NotNull(result);
            Assert.Equal(2.5, result.ToDouble());
        }

        [Fact]
        public void ExpressionInvalidConst()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => ConstantExpression.Create("Test"));
            Assert.Equal("Unknown constant 'Test'", ex.Message);
        }

        [Fact]
        public void ExpressionMod()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.MOD,
                ConstantExpression.Create(10),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.NotNull(result);
            Assert.Equal(2, result.ToInt32());
        }

        [Fact]
        public void ExpressionMod2()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.MOD,
                ConstantExpression.Create(10.5),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.NotNull(result);
            Assert.Equal(2.5, result.ToDouble());
        }

        [Fact]
        public void ExpressionMult1()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.STAR,
                ConstantExpression.Create(6),
                ConstantExpression.Create(7),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.NotNull(result);
            Assert.Equal(42, result.ToInt32());
        }

        [Fact]
        public void ExpressionMult2()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.STAR,
                ConstantExpression.Create(6.1),
                ConstantExpression.Create(7),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.NotNull(result);
            Assert.True(result.ToDouble() - 42.7 < double.Epsilon);
        }

        [Fact]
        public void ExpressionSub1()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.MINUS,
                ConstantExpression.Create(1),
                ConstantExpression.Create(1),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.NotNull(result);
            Assert.Equal(0, result.ToInt32());
        }

        [Fact]
        public void ExpressionSub2()
        {
            var m = new Module(null);
            var e = BinaryExpression.Create(
                OberonGrammarLexer.MINUS,
                ConstantExpression.Create(1.5),
                ConstantExpression.Create(1),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.NotNull(result);
            Assert.Equal(0.5, result.ToDouble());
        }

        [Fact]
        public void ThrowOnUnknownExpressionType()
        {
            var m = new Module(null);
            var e = new UnknownExpression();
            Assert.Throws<InvalidOperationException>(() => ConstantSolver.Solve(e, m.Block));
        }


        [Fact]
        public void TestUnaryOpException()
        {
            var m = new Module(null);
            var e = Assert.Throws<ArgumentException>(() => BinaryExpression.Create(OberonGrammarLexer.PLUS,
                ConstantExpression.Create(false), null, m.Block));
            Assert.Equal("Cannot find operation '+' (Bool, Any)", e.Message);
        }

        private class UnknownExpression : Expression
        {
            public UnknownExpression()
            {
                TargetType = SimpleTypeDefinition.IntType;
            }
        }
    }
}
