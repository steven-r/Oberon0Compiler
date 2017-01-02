using System.Collections.Generic;
using NUnit.Framework;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Tests
{
    [TestFixture]
    public class VarDeclarationTests
    {
        [Test]
        public void ArrayOfArray()
        {
            Module m = Oberon0Compiler.CompileString(@"MODULE Test; 
VAR
  id: ARRAY 5 OF ARRAY 10 OF INTEGER;
 END Test.");

            var id = m.Block.LookupVar("id");
            var intType = m.Block.LookupType("INTEGER");
            Assert.NotNull(id);
            Assert.IsInstanceOf<ArrayTypeDefinition>(id.Type);

            ArrayTypeDefinition atd = (ArrayTypeDefinition)id.Type;

            Assert.AreEqual(5, atd.Size);
            Assert.IsInstanceOf<ArrayTypeDefinition>(atd.ArrayType);

            ArrayTypeDefinition atd1 = (ArrayTypeDefinition)atd.ArrayType;
            Assert.AreEqual(10, atd1.Size);
            Assert.AreEqual(intType, atd1.ArrayType);
        }

        [Test]
        public void OneVar()
        {
            Module m = Oberon0Compiler.CompileString(@"MODULE Test; 
VAR
  id: INTEGER;
 END Test.");

            Assert.AreNotEqual(null, m.Block.LookupVar("id"));
            Assert.AreEqual(BaseType.IntType, m.Block.LookupVar("id").Type.BaseType);
        }

        [Test]

        public void SimpleArray()
        {
            Module m = Oberon0Compiler.CompileString(@"MODULE Test; 
VAR
  id: ARRAY 5 OF INTEGER;
 END Test.");

            var id = m.Block.LookupVar("id");
            var intType = m.Block.LookupType("INTEGER");
            Assert.NotNull(id);
            Assert.IsInstanceOf<ArrayTypeDefinition>(id.Type);

            ArrayTypeDefinition atd = (ArrayTypeDefinition)id.Type;

            Assert.AreEqual(5, atd.Size);
            Assert.AreEqual(intType, atd.ArrayType);
        }


        [Test]

        public void TwoVars1()
        {
            Module m = Oberon0Compiler.CompileString(@"MODULE Test; 
VAR
  id: INTEGER;
  id1: INTEGER;
 END Test.");

            var id = m.Block.LookupVar("id");
            Assert.AreNotEqual(null, id);
            Assert.AreEqual(BaseType.IntType, id.Type.BaseType);
            var id1 = m.Block.LookupVar("id1");
            Assert.AreNotEqual(null, id1);
            Assert.AreEqual(BaseType.IntType, id1.Type.BaseType);
        }

        [Test]
        public void TwoVars2()
        {
            Module m = Oberon0Compiler.CompileString(@"MODULE Test; 
VAR
  id, id1: INTEGER;
 END Test.");

            var id = m.Block.LookupVar("id");
            var id1 = m.Block.LookupVar("id1");
            Assert.AreNotEqual(null, id);
            Assert.AreEqual(BaseType.IntType, id.Type.BaseType);
            Assert.AreNotEqual(null, id1);
            Assert.AreEqual(BaseType.IntType, id1.Type.BaseType);
        }

        [Test]
        public void TwoVarsFail1()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
  id: INTEGER;
  id: INTEGER;
 END Test.", errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Variable declared twice", errors[0].Message);
        }

        [Test]
        public void TwoVarsFail2()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
  id, id: INTEGER;
 END Test.", errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Variable declared twice", errors[0].Message);
        }

        [Test]
        public void VarWithoutId()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
 END Test.", errors);
            Assert.AreEqual(3, errors.Count);
        }
    }
}
