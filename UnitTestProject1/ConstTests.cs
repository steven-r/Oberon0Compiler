using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

BEGIN END.");

            Assert.AreEqual("Test", m.Name);
            Assert.AreEqual(1, m.Block.Declarations.Count);
        }

        [Test]
        public void ConstSimpleExpr()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test;
CONST
  Test = 1+1;

BEGIN END.");

            Assert.AreEqual(1, m.Block.Declarations.Count);
            Assert.IsAssignableFrom(typeof(ConstDeclaration), m.Block.Declarations[0]);
            Assert.AreEqual(2, ((ConstDeclaration)m.Block.Declarations[0]).Value.ToInt32());
        }

    }
}
