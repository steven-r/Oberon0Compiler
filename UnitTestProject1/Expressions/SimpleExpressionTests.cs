#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleExpressionTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/SimpleExpressionTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests.Expressions
{
    using System;

    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Solver;

    [TestFixture]
    public class SimpleExpressionTests
    {
        [Test]
        public void ExpressionAdd1()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.PLUS,
                ConstantExpression.Create(1),
                ConstantExpression.Create(1),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToInt32());
        }

        [Test]
        public void ExpressionAddRes0()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.PLUS,
                ConstantExpression.Create(1),
                ConstantDoubleExpression.Zero,
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.ToInt32());
            Assert.AreEqual(1.0, result.ToDouble());
            Assert.AreEqual(true, result.ToBool());
            Assert.AreEqual(false, result.IsUnary);
            Assert.AreEqual(true, result.IsConst);
        }

        [Test]
        public void ExpressionAdd2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.PLUS,
                ConstantExpression.Create(1),
                ConstantExpression.Create(1.42),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2.42, result.ToDouble());
        }

        [Test]
        public void ExpressionDiv0()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.DIV,
                ConstantExpression.Create("10.0"),
                ConstantIntExpression.Zero, 
                m.Block);
            Assert.Throws<DivideByZeroException>(() => { ConstantSolver.Solve(e, m.Block); });
        }

        [Test]
        public void ExpressionDiv1()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.DIV,
                ConstantExpression.Create(10),
                ConstantExpression.Create(2),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.ToInt32());
        }

        [Test]
        public void ExpressionDiv2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.DIV,
                ConstantExpression.Create(10),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToInt32());
        }

        [Test]
        public void ExpressionDiv3()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.DIV,
                ConstantExpression.Create("10.0"),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2.5, result.ToDouble());
        }

        [Test]
        public void ExpressionMod()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.MOD,
                ConstantExpression.Create(10),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToInt32());
        }

        [Test]
        public void ExpressionMod2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.MOD,
                ConstantExpression.Create(10.5),
                ConstantExpression.Create(4),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2.5m, result.ToDouble());
        }

        [Test]
        public void ExpressionMult1()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.MULT,
                ConstantExpression.Create(6),
                ConstantExpression.Create(7),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(42, result.ToInt32());
        }

        [Test]
        public void ExpressionMult2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.MULT,
                ConstantExpression.Create(6.1),
                ConstantExpression.Create(7),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.IsTrue(result.ToDouble() == 42.7m);
        }

        [Test]
        public void ExpressionSub1()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.MINUS,
                ConstantExpression.Create(1),
                ConstantExpression.Create(1),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ToInt32());
        }

        [Test]
        public void ExpressionSub2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.MINUS,
                ConstantExpression.Create(1.5),
                ConstantExpression.Create(1),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(0.5, result.ToDouble());
        }

        [Test]
        public void ExpressionAnd()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.AND,
                ConstantExpression.Create(false),
                ConstantExpression.Create("true"),
                m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.ToBool());
        }

        [Test]
        public void ExpressionInvalidConst()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => ConstantExpression.Create("Test"));
            Assert.AreEqual("Unknown constant 'Test'", ex.Message);
        }
    }
}