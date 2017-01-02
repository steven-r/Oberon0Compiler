using System;
using NUnit.Framework;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Tests
{
    [TestFixture]
    public class SimpleExpressionTest
    {
        [Test]
        public void ExpressionAdd1()
        {
            var b = new Block();
            var e = BinaryExpression.Create(OberonGrammarLexer.PLUS,
                        ConstantExpression.Create(1),
                        ConstantExpression.Create(1), b);
            var result = ConstantSolver.Solve(e, b) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToInt32());
        }

        [Test]
        public void ExpressionAdd2()
        {
            var b = new Block();
            var e = BinaryExpression.Create(OberonGrammarLexer.PLUS,
                        ConstantExpression.Create(1),
                        ConstantExpression.Create(1.42), b);
            var result = ConstantSolver.Solve(e, b) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2.42, result.ToDouble());
        }

        [Test]
        public void ExpressionDiv0()
        {
            var b = new Block();
            var e = BinaryExpression.Create(OberonGrammarLexer.DIV,
                        ConstantExpression.Create("10.0"),
                        ConstantExpression.Create(0), b);
            Assert.Throws<ArithmeticException>(() => { ConstantSolver.Solve(e, b); });
        }

        [Test]
        public void ExpressionDiv1()
        {
            var b = new Block();
            var e = BinaryExpression.Create(OberonGrammarLexer.DIV,
                        ConstantExpression.Create(10),
                        ConstantExpression.Create(2), b);
            var result = ConstantSolver.Solve(e, b) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.ToInt32());
        }

        [Test]
        public void ExpressionDiv2()
        {
            var b = new Block();
            var e = BinaryExpression.Create(OberonGrammarLexer.DIV,
                        ConstantExpression.Create(10),
                        ConstantExpression.Create(4), b);
            var result = ConstantSolver.Solve(e, b) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToInt32());
        }

        [Test]
        public void ExpressionDiv3()
        {
            var b = new Block();
            var e = BinaryExpression.Create(OberonGrammarLexer.DIV,
                        ConstantExpression.Create("10.0"),
                        ConstantExpression.Create(4), b);
            var result = ConstantSolver.Solve(e, b) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2.5, result.ToDouble());
        }

        [Test]
        public void ExpressionMult1()
        {
            var b = new Block();
            var e = BinaryExpression.Create(OberonGrammarLexer.MULT,
                        ConstantExpression.Create(6),
                        ConstantExpression.Create(7), b);
            var result = ConstantSolver.Solve(e, b) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(42, result.ToInt32());
        }

        [Test]
        public void ExpressionSub1()
        {
            var b = new Block();
            var e = BinaryExpression.Create(OberonGrammarLexer.MINUS,
                        ConstantExpression.Create(1),
                        ConstantExpression.Create(1), b);
            var result = ConstantSolver.Solve(e, b) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ToInt32());
        }

        [Test]
        public void ExpressionSub2()
        {
            var b = new Block();
            var e = BinaryExpression.Create(OberonGrammarLexer.MINUS,
                        ConstantExpression.Create(1.5),
                        ConstantExpression.Create(1), b);
            var result = ConstantSolver.Solve(e, b) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(0.5, result.ToDouble());
        }
    }
}
