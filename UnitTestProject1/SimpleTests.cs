#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Test.Support;
using Xunit;

namespace Oberon0.Compiler.Tests
{
    public class SimpleTests
    {
        [Fact]
        public void EmptyApplication()
        {
            var m = Oberon0Compiler.CompileString("MODULE Test; END Test.");
            Assert.Equal("Test", m.Name);
            Assert.Equal(3, m.Block.Declarations.Count);
        }

        [Fact]
        public void EmptyApplication2()
        {
            var m = TestHelper.CompileString(
                "MODULE Test; BEGIN END Test.");
            Assert.Empty(m.Block.Statements);
        }

        [Fact]
        public void ModuleMissingDot()
        {
            TestHelper.CompileString(
                "MODULE Test; END Test",
                "missing '.' at '<EOF>'");
        }

        [Fact]
        public void ModuleMissingId()
        {
            TestHelper.CompileString(
                "MODULE ; BEGIN END Test.",
                "missing ID at ';'",
                "The name of the module does not match the end node");
        }
    }
}
