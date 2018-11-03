using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oberon0.Compiler.Tests.Expressions
{
    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Statements;
    using Oberon0.TestSupport;

    [TestFixture]
    public class RelationalExpressionsTests
    {
        /* and */
        [Test]
        public void TestAnd1()
        {
            var m = CompileString("x := a & b");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.AreEqual(OberonGrammarLexer.AND, binExpr.Operation.Metadata.Operation);
        }

        [Test]
        public void TestAnd2()
        {
            var m = CompileString("x := a & TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.AreEqual(OberonGrammarLexer.AND, binExpr.Operation.Metadata.Operation);
        }

        [Test]
        public void TestAndConstTrue()
        {
            var m = CompileString("x := TRUE & TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.AreEqual(true, boolExpression.ToBool());
        }

        [Test]
        public void TestAndConstFalse()
        {
            var m = CompileString("x := TRUE & FALSE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.AreEqual(false, boolExpression.ToBool());
        }

        [Test]
        public void TestAndConstError()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => CompileString("x := TRUE & 1"));
            Assert.AreEqual("Cannot find operation '&' (Bool, Int)", ex.Message);
        }

        /* or */
        [Test]
        public void TestOr1()
        {
            var m = CompileString("x := a OR b");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.AreEqual(OberonGrammarLexer.OR, binExpr.Operation.Metadata.Operation);
        }

        [Test]
        public void TestOr2()
        {
            var m = CompileString("x := a OR TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.AreEqual(OberonGrammarLexer.OR, binExpr.Operation.Metadata.Operation);
        }

        [Test]
        public void TestOrConstTrue()
        {
            var m = CompileString("x := FALSE OR TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.AreEqual(true, boolExpression.ToBool());
        }

        private Module CompileString(string operations, params string[] expectedErrors)
        {
            return TestHelper.CompileString(
                $"MODULE test; VAR a,b,c,d,e,f,g,h,x,y,z: BOOLEAN; BEGIN {operations} END test.",
                expectedErrors);
        }
    }
}
