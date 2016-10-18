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
    public class RecordTests
    {
        [Test]
        public void SimpleRecord()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
TYPE
  Demo = INTEGER;

BEGIN END.");

            Assert.AreEqual(true, m.Block.Types.Any(x => x.Name == "Demo"));
            Assert.NotNull(m.Block.LookupType("Demo"));
        }

        [Test]
        public void LookupType()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
TYPE
  Demo = INTEGER;

BEGIN END.");

            Assert.NotNull(m.Block.LookupType("Demo"));
        }

        [Test]
        public void LookupTypeFail()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
TYPE
  Demo = INTEGER;

BEGIN END.");

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

BEGIN END.");

            Assert.IsAssignableFrom(typeof(RecordTypeDefinition), m.Block.LookupType("Demo"));
            RecordTypeDefinition rtd = (RecordTypeDefinition) m.Block.LookupType("Demo");
            Assert.AreEqual(1, rtd.Elements.Count);
            Assert.AreEqual("a", rtd.Elements[0].Name);
            Assert.AreEqual("INTEGER", rtd.Elements[0].Type.Name);
        }

    }
}
