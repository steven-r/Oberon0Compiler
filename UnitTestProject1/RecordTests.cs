using System.Linq;
using NUnit.Framework;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;

namespace UnitTestProject1
{
    [TestFixture]
    public class RecordTests
    {
        [Test]
        public void LookupType()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            Assert.NotNull(m.Block.LookupType("Demo"));
        }

        [Test]
        public void LookupTypeFail()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            Assert.IsNull(m.Block.LookupType("?Unknown"));
        }

        [Test]
        public void RecordSimple()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
TYPE
  Demo = RECORD 
    a: INTEGER;
  END;

END Test.");

            Assert.IsAssignableFrom(typeof(RecordTypeDefinition), m.Block.LookupType("Demo"));
            RecordTypeDefinition rtd = (RecordTypeDefinition) m.Block.LookupType("Demo");
            Assert.AreEqual(1, rtd.Elements.Count);
            Assert.AreEqual("a", rtd.Elements[0].Name);
            Assert.AreEqual("INTEGER", rtd.Elements[0].Type.Name);
        }

        [Test]
        public void SimpleRecord()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            Assert.AreEqual(true, m.Block.Types.Any(x => x.Name == "Demo"));
            Assert.NotNull(m.Block.LookupType("Demo"));
        }
    }
}
