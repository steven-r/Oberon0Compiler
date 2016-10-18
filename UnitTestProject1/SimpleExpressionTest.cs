using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 1 }),
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 1 }));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToInt32());
        }

        [Test]
        public void Add2()
        {
            var e = BinaryExpression.Create(TokenType.Add,
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 1 }),
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 1.42 }));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2.42, result.ToDouble());
        }

        [Test]
        public void Sub1()
        {
            var e = BinaryExpression.Create(TokenType.Sub,
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 1 }),
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 1 }));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ToInt32());
        }

        [Test]
        public void Mult1()
        {
            var e = BinaryExpression.Create(TokenType.Mul,
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 6 }),
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 7 }));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(42, result.ToInt32());
        }

        [Test]
        public void Div1()
        {
            var e = BinaryExpression.Create(TokenType.Div,
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 10 }),
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 2 }));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.ToInt32());
        }

        [Test]
        public void Div2()
        {
            var e = BinaryExpression.Create(TokenType.Div,
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 10 }),
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 4 }));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToInt32());
        }

        [Test]
        public void Div3()
        {
            var e = BinaryExpression.Create(TokenType.Div,
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = "10.0" }),
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 4 }));
            var result = ConstantSolver.Solve(e, new Block()) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2.5, result.ToDouble());
        }

        [Test]
        public void Div0()
        {
            var e = BinaryExpression.Create(TokenType.Div,
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = "10.0" }),
                        ConstantExpression.Create(new Token { Type = TokenType.Num, Value = 0 }));
            Assert.Throws<ArithmeticException>(() => { ConstantSolver.Solve(e, new Block()); });
        }

    }
}
