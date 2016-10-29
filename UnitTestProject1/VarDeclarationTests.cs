using System;
using NUnit.Framework;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;

namespace UnitTestProject1
{
    [TestFixture]
    public class VarDeclarationTests
    {
        [Test]
        public void ArrayOfArray()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
VAR
  id: ARRAY 5 OF ARRAY 10 OF INTEGER;
 END Test.");

            Assert.AreEqual(3, m.Block.Declarations.Count);
            Assert.AreEqual(BaseType.ComplexType, m.Block.Declarations[2].Type.BaseType);
            Assert.IsAssignableFrom(typeof(ArrayTypeDefinition), m.Block.Declarations[2].Type);
            ArrayTypeDefinition atd = (ArrayTypeDefinition) m.Block.Declarations[2].Type;
            Assert.AreEqual(5, atd.Size);
            Assert.IsAssignableFrom(typeof(ArrayTypeDefinition), atd.ArrayType);
            atd = (ArrayTypeDefinition)atd.ArrayType;
            Assert.AreEqual(10, atd.Size);
            Assert.AreEqual("INTEGER", atd.ArrayType.Name);
        }

        [Test]
        public void OneVar()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
VAR
  id: INTEGER;
 END Test.");

            Assert.AreEqual(3, m.Block.Declarations.Count);
            Assert.AreEqual(BaseType.IntType, m.Block.Declarations[2].Type.BaseType);
        }

        [Test]

        public void SimpleArray()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
VAR
  id: ARRAY 5 OF INTEGER;
 END Test.");

            Assert.AreEqual(3, m.Block.Declarations.Count);
            Assert.AreEqual(BaseType.ComplexType, m.Block.Declarations[2].Type.BaseType);
            Assert.IsAssignableFrom(typeof(ArrayTypeDefinition), m.Block.Declarations[2].Type);
            ArrayTypeDefinition atd = (ArrayTypeDefinition)m.Block.Declarations[2].Type;
            Assert.AreEqual(5, atd.Size);
            Assert.AreEqual("INTEGER", atd.ArrayType.Name);
        }


        [Test]

        public void TwoVars1()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
VAR
  id: INTEGER;
  id1: INTEGER;
 END Test.");

            Assert.AreEqual(4, m.Block.Declarations.Count);
            Assert.AreEqual(BaseType.IntType, m.Block.Declarations[2].Type.BaseType);
            Assert.AreEqual(BaseType.IntType, m.Block.Declarations[3].Type.BaseType);
        }

        [Test]
        public void TwoVars2()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
VAR
  id, id1: INTEGER;
 END Test.");

            Assert.AreEqual(4, m.Block.Declarations.Count);
            Assert.AreEqual(BaseType.IntType, m.Block.Declarations[2].Type.BaseType);
            Assert.AreEqual(BaseType.IntType, m.Block.Declarations[3].Type.BaseType);
        }

        [Test]
        public void TwoVarsFail1()
        {
            var compiler = new CompilerParser();
            Exception e = Assert.Throws<Loyc.LogException>(() => { Module m = compiler.Calculate(@"MODULE Test; 
VAR
  id: INTEGER;
  id: INTEGER;
 END Test."); });
            Assert.AreEqual(e.Message, "The identifier id has been defined already");
        }

        [Test]
        public void TwoVarsFail2()
        {
            var compiler = new CompilerParser();
            Exception e = Assert.Throws<Loyc.LogException>(() => { Module m = compiler.Calculate(@"MODULE Test; 
VAR
  id, id: INTEGER;
 END Test."); });
            Assert.AreEqual(e.Message, "The identifier id has been defined already");
        }

        [Test]
        public void VarWithoutId()
        {
            var compiler = new CompilerParser();


            Exception e = Assert.Throws<Loyc.LogException>(() => { Module m = compiler.Calculate(@"MODULE Test; 
VAR
 END Test."); });
            Assert.AreEqual(e.Message, "An identifier is expected here");
        }
    }
}
