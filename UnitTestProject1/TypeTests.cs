using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;
using Oberon0.CompilerSupport;

namespace Oberon0.Compiler.Tests
{
    [TestFixture]
    public class TypeTests
    {
        [Test]
        public void LookupType()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            Assert.NotNull(m.Block.LookupType("Demo"));
        }

        [Test]
        public void LookupTypeFail()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            Assert.IsNull(m.Block.LookupType("?Unknown"));
        }

        [Test]
        public void RecordSimple()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
TYPE
  Demo = RECORD 
    a: INTEGER
  END;

END Test.");

            var t = m.Block.LookupType("Demo");
            var intType = m.Block.LookupType("INTEGER");
            Assert.NotNull(t);
            Assert.IsInstanceOf<RecordTypeDefinition>(t);
            RecordTypeDefinition rtd = (RecordTypeDefinition)t;
            Assert.AreEqual(1, rtd.Elements.Count);
            Declaration a = rtd.Elements.First();
            Assert.AreEqual("a", a.Name);
            Assert.AreEqual(intType, a.Type);
        }

        [Test]
        public void SimpleRecord()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            var t = m.Block.LookupType("Demo");
            var intType = m.Block.LookupType("INTEGER");
            Assert.NotNull(t);
            Assert.IsInstanceOf<SimpleTypeDefinition>(t);
            SimpleTypeDefinition std = (SimpleTypeDefinition)t;
            Assert.AreEqual(intType.BaseType, std.BaseType);
        }

        [Test]
        public void TypeDefinedTwiceError()
        {
            List<CompilerError> errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(@"MODULE Test; 
TYPE
  Demo = INTEGER;
  Demo = INTEGER;

END Test.", errors);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Type Demo declared twice", errors[0].Message);
        }
    }
}
