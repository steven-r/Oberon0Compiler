using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oberon0.Compiler.Tests
{
    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Generator;
    using Oberon0.TestSupport;

    [TestFixture]
    public class DummyTests
    {
        [Test]
        public void FixupGeneratorInfo()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x : INTEGER;

END Test.");
            Assert.NotNull(m);
            Declaration d = m.Block.LookupVar("x");
            Assert.NotNull(d);
            Assert.IsNull(d.GeneratorInfo);
            d.GeneratorInfo = new DummyGeneratorInfo();
            Assert.NotNull(d.GeneratorInfo);
        }

        private class DummyGeneratorInfo : IGeneratorInfo
        {
        }
    }
}
