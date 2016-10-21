using System;
using NUnit.Framework;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Solver;

namespace UnitTestProject1
{
    [TestFixture]
    public class SimpleExpressionTest
    {
        [Test]
        public void Add1()
        {
            var e = BinaryExpression.Create(TokenType.Add,
                        ConstantExpression.Create(1),
                        ConstantExpression.Create(1));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToInt32());
        }

        [Test]
        public void Add2()
        {
            var e = BinaryExpression.Create(TokenType.Add,
                        ConstantExpression.Create(1),
                        ConstantExpression.Create(1.42));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2.42, result.ToDouble());
        }

        [Test]
        public void Sub1()
        {
            var e = BinaryExpression.Create(TokenType.Sub,
                        ConstantExpression.Create(1),
                        ConstantExpression.Create(1));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ToInt32());
        }

        [Test]
        public void Sub2()
        {
            var e = BinaryExpression.Create(TokenType.Sub,
                        ConstantExpression.Create(1.5),
                        ConstantExpression.Create(1));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(0.5, result.ToDouble());
        }

        [Test]
        public void Mult1()
        {
            var e = BinaryExpression.Create(TokenType.Mul,
                        ConstantExpression.Create(6),
                        ConstantExpression.Create(7));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(42, result.ToInt32());
        }

        [Test]
        public void Div1()
        {
            var e = BinaryExpression.Create(TokenType.Div,
                        ConstantExpression.Create(10),
                        ConstantExpression.Create(2));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.ToInt32());
        }

        [Test]
        public void Div2()
        {
            var e = BinaryExpression.Create(TokenType.Div,
                        ConstantExpression.Create(10),
                        ConstantExpression.Create(4));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToInt32());
        }

        [Test]
        public void Div3()
        {
            var e = BinaryExpression.Create(TokenType.Div,
                        ConstantExpression.Create("10.0"),
                        ConstantExpression.Create(4));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2.5, result.ToDouble());
        }

        [Test]
        public void Div0()
        {
            var e = BinaryExpression.Create(TokenType.Div,
                        ConstantExpression.Create("10.0"),
                        ConstantExpression.Create(0));
            Assert.Throws<ArithmeticException>(() => { ConstantSolver.Solve(e, new Block()); });
        }

    }
}
