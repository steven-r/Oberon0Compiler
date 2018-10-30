#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleStatementTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/SimpleStatementTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Statements;
    using Oberon0.CompilerSupport;

    [TestFixture]
    public class SimpleStatementTests
    {
        [Test]
        public void InvalidParameterCount()
        {
            var errors = new List<CompilerError>();
            TestHelper.CompileString(
                @"MODULE Test; 
BEGIN 
    WriteInt
END Test.
",
                errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Number of parameters expected: 1", errors[0].Message);
        }

        [Test]
        public void SimpleAssignment()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 1
END Test.
");
            Assert.AreEqual(1, m.Block.Statements.Count);
            Assert.IsAssignableFrom(typeof(AssignmentStatement), m.Block.Statements[0]);
            AssignmentStatement ast = (AssignmentStatement)m.Block.Statements[0];
            Assert.AreEqual("x", ast.Variable.Name);
            Assert.IsAssignableFrom(typeof(ConstantIntExpression), ast.Expression);
            ConstantIntExpression cie = (ConstantIntExpression)ast.Expression;
            Assert.AreEqual(1, cie.Value);
        }

        [Test]
        public void SimpleRepeat()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 1;
    REPEAT
        
    UNTIL x > 0
END Test.
");
            Assert.AreEqual(2, m.Block.Statements.Count);
            Assert.IsAssignableFrom(typeof(AssignmentStatement), m.Block.Statements[0]);
            AssignmentStatement ast = (AssignmentStatement)m.Block.Statements[0];
            Assert.AreEqual("x", ast.Variable.Name);
            Assert.IsAssignableFrom(typeof(ConstantIntExpression), ast.Expression);
            ConstantIntExpression cie = (ConstantIntExpression)ast.Expression;
            Assert.AreEqual(1, cie.Value);

            Assert.IsAssignableFrom(typeof(RepeatStatement), m.Block.Statements[1]);
            RepeatStatement rs = (RepeatStatement)m.Block.Statements[1];
            Assert.AreEqual(0, rs.Block.Statements.Count);
        }

        [Test]
        public void SimpleRepeatFailCondition()
        {
            var errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 1;
    REPEAT
        
    UNTIL 0
END Test.
",
                errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("The condition needs to return a logical condition", errors[0].Message);
        }

        /// <summary>
        /// The two statements.
        /// </summary>
        [Test]
        public void TwoStatements()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 1;
    x := 2
END Test.
");
            Assert.AreEqual(2, m.Block.Statements.Count);
            Assert.IsAssignableFrom(typeof(AssignmentStatement), m.Block.Statements[0]);
            AssignmentStatement ast = (AssignmentStatement)m.Block.Statements[0];
            Assert.AreEqual("x", ast.Variable.Name);
            Assert.IsAssignableFrom(typeof(ConstantIntExpression), ast.Expression);
            ConstantIntExpression cie = (ConstantIntExpression)ast.Expression;
            Assert.AreEqual(1, cie.Value);

            Assert.IsAssignableFrom(typeof(AssignmentStatement), m.Block.Statements[1]);
            ast = (AssignmentStatement)m.Block.Statements[1];
            Assert.AreEqual("x", ast.Variable.Name);
            Assert.IsAssignableFrom(typeof(ConstantIntExpression), ast.Expression);
            cie = (ConstantIntExpression)ast.Expression;
            Assert.AreEqual(2, cie.Value);
        }

        [Test]
        public void TestAssignableBoolInt()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: BOOLEAN;

BEGIN 
    x := 1;
END Test.
");
            Assert.AreEqual(1, m.Block.Statements.Count);
            Assert.IsAssignableFrom(typeof(AssignmentStatement), m.Block.Statements[0]);
            AssignmentStatement ast = (AssignmentStatement)m.Block.Statements[0];
            Assert.AreEqual("x", ast.Variable.Name);
            Assert.IsAssignableFrom(typeof(ConstantIntExpression), ast.Expression);
            ConstantIntExpression cie = (ConstantIntExpression)ast.Expression;
            Assert.AreEqual(1, cie.Value);
        }
        [Test]
        public void TestAssignableBoolReal()
        {
            var errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x: BOOLEAN;

BEGIN 
    x := 1.234;
END Test.
",
                errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Left & right side do not match types", errors.First().Message);
        }
    }
}