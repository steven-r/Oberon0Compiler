using NUnit.Framework;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;

namespace UnitTestProject1
{
    [TestFixture]
    class ConstTests
    {
        [Test]
        public void ConstSimple()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test;
CONST
  Test = 1;

 END Test.");

            Assert.AreEqual("Test", m.Name);
            Assert.AreEqual(3, m.Block.Declarations.Count);
        }

        [Test]
        public void ConstSimpleExpr()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test;
CONST
  Test = 1+1;

 END Test.");

            Assert.AreEqual(3, m.Block.Declarations.Count);
            Assert.IsAssignableFrom(typeof(ConstDeclaration), m.Block.Declarations[2]);
            Assert.AreEqual(2, ((ConstDeclaration)m.Block.Declarations[2]).Value.ToInt32());
        }

        [Test]
        public void ConstConstExpr()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test;
CONST
  Test1 = 2;
  Test = 1+Test1;

 END Test.");

            Assert.AreEqual(4, m.Block.Declarations.Count);
            Assert.IsAssignableFrom(typeof(ConstDeclaration), m.Block.Declarations[2]);
            Assert.AreEqual(2, ((ConstDeclaration)m.Block.Declarations[2]).Value.ToInt32());
            Assert.IsAssignableFrom(typeof(ConstDeclaration), m.Block.Declarations[3]);
            Assert.AreEqual(3, ((ConstDeclaration)m.Block.Declarations[3]).Value.ToInt32());
        }

    }
}
