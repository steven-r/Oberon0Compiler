#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Xunit;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Generator;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests
{
    public class DummyTests
    {
        private class DummyGeneratorInfo : IGeneratorInfo
        {}

        [Fact]
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
            Assert.Null(d.GeneratorInfo);
            d.GeneratorInfo = new DummyGeneratorInfo();
            Assert.NotNull(d.GeneratorInfo);
        }
    }
}
