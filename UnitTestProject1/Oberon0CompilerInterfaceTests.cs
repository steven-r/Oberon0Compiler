﻿using Oberon0.Compiler.Generator;
using Oberon0.TestSupport;
using Xunit;

namespace Oberon0.Compiler.Tests
{
    public class Oberon0CompilerInterfaceTests
    {
        [Fact]
        public void TestHasError()
        {
            var m = TestHelper.CompileString(
                // ReSharper disable once StringLiteralTypo
                @"MODULE Test; 
VAR
  x : INTEGER; This is some error

END Test.", "no viable alternative at input 'Thisis'");
            Assert.True(m.CompilerInstance.HasError);
        }

        private class DummyGeneratorInfo : IGeneratorInfo
        { }

        [Fact]
        public void FixupGeneratorInfo()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
VAR
  x : INTEGER;

END Test.");
            Assert.NotNull(m);
            var d = m.Block.LookupVar("x");
            Assert.NotNull(d);
            Assert.Null(d.GeneratorInfo);
            d.GeneratorInfo = new DummyGeneratorInfo();
            Assert.NotNull(d.GeneratorInfo);
        }
    }
}