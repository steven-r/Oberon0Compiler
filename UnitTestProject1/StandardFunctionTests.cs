using System.Collections.Generic;
using NUnit.Framework;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Tests
{
    using Oberon0.TestSupport;

    [TestFixture]
    public class StandardFunctionTests
    {
        [Test]
        public void TestReadInt()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
  Demo : INTEGER;
BEGIN
  ReadInt(Demo)
END Test.");
            Assert.AreEqual(1, m.Block.Statements.Count);

        }

        [Test]
        public void TestReadReal()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
  Demo : REAL;
BEGIN
  ReadReal(Demo)
END Test.");
            Assert.AreEqual(1, m.Block.Statements.Count);

        }

        [Test]
        public void TestReadRealFailIntType()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
  Demo : INTEGER;
BEGIN
  ReadReal(Demo)
END Test.", errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Parameter any mismatch. Expected REAL, found INTEGER", errors[0].Message);
        }

        [Test]
        public void TestReadIntFailBoolType()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
  Demo : BOOLEAN;
BEGIN
  ReadInt(Demo)
END Test.", errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Parameter any mismatch. Expected INTEGER, found BOOLEAN", errors[0].Message);
        }

        [Test]
        public void TestReadIntFailNumber()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
  Demo : BOOLEAN;
BEGIN
  ReadInt(1)
END Test.", errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Parameter any requires a variable reference, not an expression", errors[0].Message);
        }

        [Test]
        public void TestReadIntFailString()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
  Demo : STRING;
BEGIN
  ReadInt(Demo)
END Test.", errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Parameter any mismatch. Expected INTEGER, found STRING", errors[0].Message);
        }

    }
}
