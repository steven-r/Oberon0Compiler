#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RelationalExpressionsTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/RelationalExpressionsTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests.Expressions
{
    using System;
    using System.Linq;

    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Solver;
    using Oberon0.Compiler.Statements;
    using Oberon0.Compiler.Types;
    using Oberon0.TestSupport;

    [TestFixture]
    public class RelationalExpressionsTests
    {
        [Test]
        public void ExpressionNot()
        {
            var m = new Module();
            var e = BinaryExpression.Create(OberonGrammarLexer.NOT, new ConstantBoolExpression(true), null, m.Block, null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.ToBool());
        }

        [Test]
        public void ExpressionNot2()
        {
            var m = new Module();
            m.Block.Declarations.Add(new Declaration("a", m.Block.LookupType("BOOLEAN")));
            var e = BinaryExpression.Create(
                OberonGrammarLexer.NOT,
                VariableReferenceExpression.Create(m.Block, m.Block.LookupVar("a"), null),
                null,
                m.Block, 
                null);
            var result = ConstantSolver.Solve(e, m.Block) as BinaryExpression;
            Assert.IsNotNull(result);
        }

        [Test]
        public void ExpressionVarNotFound()
        {
            var m = new Module();
            m.Block.Declarations.Add(new Declaration("a", m.Block.LookupType("BOOLEAN")));
            var var = VariableReferenceExpression.Create(m.Block, m.Block.LookupVar("b"), null);
            Assert.IsNull(var);
        }

        [Test]
        public void ExpressionRelEquals()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.EQUAL,
                ConstantExpression.Create(4),
                ConstantExpression.Create(4),
                m.Block, 
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.ToBool());
        }

        [Test]
        public void ExpressionRelEquals2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.EQUAL,
                ConstantExpression.Create(4),
                ConstantExpression.Create(10),
                m.Block, 
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.ToBool());
        }

        [Test]
        public void ExpressionRelGe()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.GE,
                ConstantExpression.Create(10),
                ConstantExpression.Create(4),
                m.Block, 
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.ToBool());
        }

        [Test]
        public void ExpressionRelGe1()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.GE,
                ConstantExpression.Create(4),
                ConstantExpression.Create(4),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.ToBool());
        }

        [Test]
        public void ExpressionRelGe2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.GE,
                ConstantExpression.Create(4),
                ConstantExpression.Create(10),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.ToBool());
        }

        [Test]
        public void ExpressionRelGt()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.GT,
                ConstantExpression.Create(10),
                ConstantExpression.Create(4),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.ToBool());
        }

        [Test]
        public void ExpressionRelGt1()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.GT,
                ConstantExpression.Create(4),
                ConstantExpression.Create(4),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.ToBool());
        }

        [Test]
        public void ExpressionRelGt2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.GT,
                ConstantExpression.Create(4),
                ConstantExpression.Create(10),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.ToBool());
        }

        [Test]
        public void ExpressionRelLe()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.LE,
                ConstantExpression.Create(10),
                ConstantExpression.Create(4),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.ToBool());
        }

        [Test]
        public void ExpressionRelLe1()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.LE,
                ConstantExpression.Create(4),
                ConstantExpression.Create(4),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.ToBool());
        }

        [Test]
        public void ExpressionRelLe2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.LT,
                ConstantExpression.Create(4),
                ConstantExpression.Create(10),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.ToBool());
        }

        [Test]
        public void ExpressionRelLt()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.LT,
                ConstantExpression.Create(10),
                ConstantExpression.Create(4),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.ToBool());
        }

        [Test]
        public void ExpressionRelLt1()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.LT,
                ConstantExpression.Create(4),
                ConstantExpression.Create(4),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.ToBool());
        }

        [Test]
        public void ExpressionRelLt2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.LT,
                ConstantExpression.Create(4),
                ConstantExpression.Create(10),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.ToBool());
        }

        [Test]
        public void ExpressionRelNotEquals()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.NOTEQUAL,
                ConstantExpression.Create(4),
                ConstantExpression.Create(4),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(false, result.ToBool());
        }

        [Test]
        public void ExpressionRelNotEquals2()
        {
            var m = new Module();
            var e = BinaryExpression.Create(
                OberonGrammarLexer.NOTEQUAL,
                ConstantExpression.Create(4),
                ConstantExpression.Create(10),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as ConstantBoolExpression;
            Assert.IsNotNull(result);
            Assert.AreEqual(true, result.ToBool());
        }

        [Test]
        public void ExpressionRelVar()
        {
            var m = new Module();
            m.Block.Declarations.Add(new Declaration("a", m.Block.LookupType("INTEGER")));
            var e = BinaryExpression.Create(
                OberonGrammarLexer.NOTEQUAL,
                VariableReferenceExpression.Create(m.Block, m.Block.LookupVar("a"), null),
                ConstantExpression.Create(10),
                m.Block,
                null);
            var result = ConstantSolver.Solve(e, m.Block) as BinaryExpression;
            Assert.IsNotNull(result);
            Assert.IsFalse(result.IsConst);
        }

        /* and */
        [Test]
        public void TestAnd1()
        {
            var m = this.CompileString("x := a & b");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.AreEqual(OberonGrammarLexer.AND, binExpr.Operation.Metadata.Operation);
            Assert.AreEqual(OberonGrammarLexer.AND, binExpr.Operator);
        }

        [Test]
        public void TestAnd2()
        {
            var m = this.CompileString("x := a & TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.AreEqual(OberonGrammarLexer.AND, binExpr.Operation.Metadata.Operation);
        }

        [Test]
        public void TestAndConstError()
        {
            var ex = Assert.Throws<InvalidOperationException>(() => this.CompileString("x := TRUE & 1"));
            Assert.AreEqual("Cannot find operation '&' (Bool, Int)", ex.Message);
        }

        [Test]
        public void TestAndConstFalse()
        {
            var m = this.CompileString("x := TRUE & FALSE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.AreEqual(false, boolExpression.ToBool());
        }

        [Test]
        public void TestAndConstTrue()
        {
            var m = this.CompileString("x := TRUE & TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.AreEqual(true, boolExpression.ToBool());
        }

        [Test]
        public void TestNotConst()
        {
            var m = this.CompileString("x := ~false_");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.AreEqual(true, boolExpression.ToBool());
        }

        [Test]
        public void TestNotDirect()
        {
            var m = this.CompileString("x := ~FALSE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.AreEqual(true, boolExpression.ToBool());
        }

        [Test]
        public void TestNotVarFalse()
        {
            var m = this.CompileString("a := FALSE; x := ~a");
            Assert.AreEqual(2, m.Block.Statements.Count);
            var assignment = m.Block.Statements[1] as AssignmentStatement;
            Assert.NotNull(assignment);
            var binaryExpression = assignment.Expression as BinaryExpression;
            Assert.NotNull(binaryExpression);
            var varRef = binaryExpression.LeftHandSide as VariableReferenceExpression;
            Assert.NotNull(varRef);
            Assert.AreEqual("a", varRef.Declaration.Name);
        }

        /* or */
        [Test]
        public void TestOr1()
        {
            var m = this.CompileString("x := a OR b");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.AreEqual(OberonGrammarLexer.OR, binExpr.Operation.Metadata.Operation);
        }

        [Test]
        public void TestOr2()
        {
            var m = this.CompileString("x := a OR TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var binExpr = assignment.Expression as BinaryExpression;
            Assert.NotNull(binExpr);
            Assert.AreEqual(OberonGrammarLexer.OR, binExpr.Operation.Metadata.Operation);
        }

        [Test]
        public void TestOrConstTrue()
        {
            var m = this.CompileString("x := FALSE OR TRUE");
            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantBoolExpression;
            Assert.NotNull(boolExpression);
            Assert.AreEqual(true, boolExpression.ToBool());
        }

        [Test]
        public void TestIntAssign()
        {
            var m = TestHelper.CompileString(
                "MODULE test; CONST true_ = TRUE; false_ = FALSE; VAR a,b,c,d,e,f,g,h,x,y,z: BOOLEAN; BEGIN b := 1 END test.");

            var assignment = m.Block.Statements.First() as AssignmentStatement;
            Assert.NotNull(assignment);
            var boolExpression = assignment.Expression as ConstantIntExpression;
            Assert.NotNull(boolExpression);
            Assert.AreEqual(1, boolExpression.ToInt32());
            Assert.AreEqual(true, boolExpression.ToBool());
        }

        private Module CompileString(string operations, params string[] expectedErrors)
        {
            return TestHelper.CompileString(
                $"MODULE test; CONST true_ = TRUE; false_ = FALSE; VAR a,b,c,d,e,f,g,h,x,y,z: BOOLEAN; BEGIN {operations} END test.",
                expectedErrors);
        }
    }
}