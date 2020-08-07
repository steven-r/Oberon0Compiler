#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using NUnit.Framework;
using Oberon0.Compiler.Definitions;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests
{
    [TestFixture]
    public class SimpleTests
    {
        [Test]
        public void EmptyApplication()
        {
            Module m = Oberon0Compiler.CompileString("MODULE Test; END Test.");
            Assert.AreEqual("Test", m.Name);
            Assert.AreEqual(3, m.Block.Declarations.Count);
        }

        [Test]
        public void EmptyApplication2()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; BEGIN END Test.");
            Assert.AreEqual(0, m.Block.Statements.Count);
        }

        [Test]
        public void ModuleMissingDot()
        {
            TestHelper.CompileString(
                @"MODULE Test; END Test",
                "missing '.' at '<EOF>'");
        }

        [Test]
        public void ModuleMissingId()
        {
            TestHelper.CompileString(
                @"MODULE ; BEGIN END Test.",
                "missing ID at ';'",
                "The name of the module does not match the end node");
        }
    }
}