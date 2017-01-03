using System;
using System.Security.Cryptography.X509Certificates;
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
            var m = new Module();
            var e = BinaryExpression.Create(OberonGrammarLexer.PLUS,
                        ConstantExpression.Create(1),
                        ConstantExpression.Create(1), m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToInt32());
        }

        [Test]
        public void ExpressionAdd2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(OberonGrammarLexer.PLUS,
                        ConstantExpression.Create(1),
                        ConstantExpression.Create(1.42), m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2.42, result.ToDouble());
        }

        [Test]
        public void ExpressionDiv0()
        {
            var m = new Module();
            var e = BinaryExpression.Create(OberonGrammarLexer.DIV,
                        ConstantExpression.Create("10.0"),
                        ConstantExpression.Create(0), m.Block);
            Assert.Throws<DivideByZeroException>(() => { ConstantSolver.Solve(e, m.Block); });
        }

        [Test]
        public void ExpressionDiv1()
        {
            var m = new Module();
            var e = BinaryExpression.Create(OberonGrammarLexer.DIV,
                        ConstantExpression.Create(10),
                        ConstantExpression.Create(2), m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result.ToInt32());
        }

        [Test]
        public void ExpressionDiv2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(OberonGrammarLexer.DIV,
                        ConstantExpression.Create(10),
                        ConstantExpression.Create(4), m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToInt32());
        }

        [Test]
        public void ExpressionDiv3()
        {
            var m = new Module();
            var e = BinaryExpression.Create(OberonGrammarLexer.DIV,
                        ConstantExpression.Create("10.0"),
                        ConstantExpression.Create(4), m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2.5, result.ToDouble());
        }

        [Test]
        public void ExpressionMult1()
        {
            var m = new Module();
            var e = BinaryExpression.Create(OberonGrammarLexer.MULT,
                        ConstantExpression.Create(6),
                        ConstantExpression.Create(7), m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(42, result.ToInt32());
        }

        [Test]
        public void ExpressionMult2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(OberonGrammarLexer.MULT,
                        ConstantExpression.Create(6.1),
                        ConstantExpression.Create(7), m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.IsTrue(42.7m==result.ToDouble());
        }

        [Test]
        public void ExpressionSub1()
        {
            var m = new Module();
            var e = BinaryExpression.Create(OberonGrammarLexer.MINUS,
                        ConstantExpression.Create(1),
                        ConstantExpression.Create(1), m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.ToInt32());
        }

        [Test]
        public void ExpressionSub2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(OberonGrammarLexer.MINUS,
                        ConstantExpression.Create(1.5),
                        ConstantExpression.Create(1), m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantDoubleExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(0.5, result.ToDouble());
        }

        [Test]
        public void ExpressionMod()
        {
            var m = new Module();
            var e = BinaryExpression.Create(OberonGrammarLexer.MOD,
                        ConstantExpression.Create(10),
                        ConstantExpression.Create(4), m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantIntExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.ToInt32());
        }

        [Test]
        public void ExpressionNot()
        {
            var m = new Module();
            var e = BinaryExpression.Create(OberonGrammarLexer.NOT,
                        new ConstantBoolExpression(true),
                        null, m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.ToBool());
        }

        [Test]
        public void ExpressionNot2()
        {
            var m = new Module();
            m.Block.Declarations.Add(new Declaration("a", m.Block.LookupType("BOOLEAN")));
            var e = BinaryExpression.Create(OberonGrammarLexer.NOT,
                        VariableReferenceExpression.Create(m.Block, "a", null), 
                        null, m.Block);
            var result = ConstantSolver.Solve(e, m.Block) as BinaryExpression;
            Assert.IsNotNull(result);
        }
    }
}
