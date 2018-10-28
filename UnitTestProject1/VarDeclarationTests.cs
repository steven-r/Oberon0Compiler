using System.Collections.Generic;
using NUnit.Framework;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;
using Oberon0.CompilerSupport;

namespace Oberon0.Compiler.Tests
{
    [TestFixture]
    public class VarDeclarationTests
    {
        [Test]
        public void ArrayOfArray()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
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
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
  id: INTEGER;
 END Test.");

            Assert.AreNotEqual(null, m.Block.LookupVar("id"));
            Assert.AreEqual(BaseTypes.IntType, m.Block.LookupVar("id").Type.BaseTypes);
        }

        [Test]
        public void SimpleArray()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
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
        public void ArrayFailBooleanIndex()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test; 

VAR
  id: ARRAY TRUE OF INTEGER;
 END Test.", errors);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("The array size must return a constant integer expression", errors[0].Message);
        }


        [Test]

        public void TwoVars1()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
  id: INTEGER;
  id1: INTEGER;
 END Test.");

            var id = m.Block.LookupVar("id");
            Assert.AreNotEqual(null, id);
            Assert.AreEqual(BaseTypes.IntType, id.Type.BaseTypes);
            var id1 = m.Block.LookupVar("id1");
            Assert.AreNotEqual(null, id1);
            Assert.AreEqual(BaseTypes.IntType, id1.Type.BaseTypes);
        }

        [Test]
        public void TwoVars2()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
  id, id1: INTEGER;
 END Test.");

            var id = m.Block.LookupVar("id");
            var id1 = m.Block.LookupVar("id1");
            Assert.AreNotEqual(null, id);
            Assert.AreEqual(BaseTypes.IntType, id.Type.BaseTypes);
            Assert.AreNotEqual(null, id1);
            Assert.AreEqual(BaseTypes.IntType, id1.Type.BaseTypes);
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


        [Test]
        public void VarWithoutWrongType()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test; 
VAR
    id: DUMMY;

 END Test.", errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Type not known", errors[0].Message);
        }
    }
}
