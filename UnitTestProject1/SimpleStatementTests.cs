using System.Collections.Generic;
using NUnit.Framework;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Statements;
using Oberon0.CompilerSupport;

namespace Oberon0.Compiler.Tests
{

    [TestFixture]
    public class SimpleStatementTests
    {
        [Test]
        public void SimpleAssignment()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
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
            Module m = TestHelper.CompileString(@"MODULE Test; 
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
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
  x: INTEGER;

BEGIN 
    x := 1;
    REPEAT
        
    UNTIL 0
END Test.
", errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("The condition needs to return a logical condition", errors[0].Message);
        }

        [Test]

        public void TwoStatements()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
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
        public void InvalidParameterCount()
        {
            List<CompilerError> errors = new List<CompilerError>();
            TestHelper.CompileString(@"MODULE Test; 
BEGIN 
    WriteInt
END Test.
", errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Number of parameters expected: 1", errors[0].Message);
        }
    }
}
