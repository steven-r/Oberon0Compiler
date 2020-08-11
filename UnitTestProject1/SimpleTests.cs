#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Xunit;
using Oberon0.Compiler.Definitions;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests
{
    public class SimpleTests
    {
        [Fact]
        public void EmptyApplication()
        {
            Module m = Oberon0Compiler.CompileString("MODULE Test; END Test.");
            Assert.Equal("Test", m.Name);
            Assert.Equal(3, m.Block.Declarations.Count);
        }

        [Fact]
        public void EmptyApplication2()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; BEGIN END Test.");
            Assert.Empty(m.Block.Statements);
        }

        [Fact]
        public void ModuleMissingDot()
        {
            TestHelper.CompileString(
                @"MODULE Test; END Test",
                "missing '.' at '<EOF>'");
        }

        [Fact]
        public void ModuleMissingId()
        {
            TestHelper.CompileString(
                @"MODULE ; BEGIN END Test.",
                "missing ID at ';'",
                "The name of the module does not match the end node");
        }
    }
}