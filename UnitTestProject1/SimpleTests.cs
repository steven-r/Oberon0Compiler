using System.Collections.Generic;
using NUnit.Framework;
using Oberon0.Compiler.Definitions;
using Oberon0.CompilerSupport;

namespace Oberon0.Compiler.Tests
{
    [TestFixture]
    public class SimpleTests
    {
        [Test]
        public void EmptyApplication()
        {
            Module m = Oberon0Compiler.CompileString("MODULE Test; END Test.");

            Assert.AreEqual("Test", m.Name);
            Assert.AreEqual(2, m.Block.Declarations.Count);
        }

        [Test]
        public void EmptyApplication2()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
BEGIN END Test.");
            Assert.AreEqual(0, m.Block.Statements.Count);
        }

        [Test]
        public void ModuleMissingDot()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test; 
END Test", errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("missing '.' at '<EOF>'", errors[0].Message);
        }

        [Test]
        public void ModuleMissingId()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE ; 
BEGIN END Test.", errors);
            Assert.AreEqual(2, errors.Count);
            Assert.AreEqual("missing ID at ';'", errors[0].Message);
            Assert.AreEqual("The name of the module does not match the end node", errors[1].Message);
        }
    }
}
