using System.Collections.Generic;
using NUnit.Framework;
using Oberon0.Compiler.Definitions;
using Oberon0.CompilerSupport;

namespace Oberon0.Compiler.Tests
{
    [TestFixture]
    public class ConstTests
    {
        [Test]
        public void ConstConstExpr()
        {
            Module m = TestHelper.CompileString(@"MODULE Test;
CONST
  Test1 = 2;
  Test = 1+Test1;

 END Test.");

            var t = m.Block.LookupVar("Test");
            var t1 = m.Block.LookupVar("Test1");
            Assert.IsNotNull(t);
            Assert.IsNotNull(t1);
            Assert.IsInstanceOf<ConstDeclaration>(t);
            Assert.IsInstanceOf<ConstDeclaration>(t1);
            var tp = (ConstDeclaration)t;
            var tp1 = (ConstDeclaration)t1;
            Assert.AreEqual("Test", tp.Name);
            Assert.AreEqual(m.Block.LookupType("INTEGER"), tp.Type);
            Assert.AreEqual(3, tp.Value.ToInt32());

            Assert.AreEqual("Test1", tp1.Name);
            Assert.AreEqual(m.Block.LookupType("INTEGER"), tp1.Type);
            Assert.AreEqual(2, tp1.Value.ToInt32());
        }

        [Test]
        public void ConstSimple()
        {
            Module m = TestHelper.CompileString(@"MODULE Test;
CONST
  Test = 1;

 END Test.");

            var c = m.Block.LookupVar("Test");
            Assert.IsNotNull(c);
            Assert.IsInstanceOf<ConstDeclaration>(c);
            var cp = (ConstDeclaration)c;
            Assert.AreEqual("Test", cp.Name);
            Assert.AreEqual(m.Block.LookupType("INTEGER"), cp.Type);
            Assert.AreEqual(1, cp.Value.ToInt32());
        }

        [Test]
        public void ConstSimpleFailDuplicate()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test;
CONST
  Test = 1;
  Test = 2;

 END Test.", errors);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("A variable/constant with this name has been defined already", errors[0].Message);
        }

        [Test]
        public void ConstSimpleFailVarReference()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test;
VAR
  Test : INTEGER;
CONST
  Test1 = 2+Test;

 END Test.", errors);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("A constant must resolve during compile time", errors[0].Message);
        }


        [Test]
        public void ConstSimpleExpr()
        {
            Module m = Oberon0Compiler.CompileString(@"MODULE Test;
CONST
  Test = 1+1;

 END Test.");

            var c = m.Block.LookupVar("Test");
            Assert.IsNotNull(c);
            Assert.IsInstanceOf<ConstDeclaration>(c);
            var cp = (ConstDeclaration)c;
            Assert.AreEqual("Test", cp.Name);
            Assert.AreEqual(m.Block.LookupType("INTEGER"), cp.Type);
            Assert.AreEqual(2, cp.Value.ToInt32());
        }
    }
}
