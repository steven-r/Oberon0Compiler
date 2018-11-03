#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SimpleTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/SimpleTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests
{
    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.TestSupport;

    [TestFixture]
    public class SimpleTests
    {
        [Test]
        public void EmptyApplication()
        {
            Module m = Oberon0Compiler.CompileString("MODULE Test; END Test.");
            Assert.AreEqual("Test", m.Name);
            Assert.AreEqual(2, m.Block.Declarations.Count);
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