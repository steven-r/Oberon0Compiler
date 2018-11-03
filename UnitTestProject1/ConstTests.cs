#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/ConstTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests
{
    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.TestSupport;

    [TestFixture]
    public class ConstTests
    {
        [Test]
        public void ConstConstExpr()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test;
CONST
  Test1 = 2;
  Test = 1+Test1;

 END Test.");

            var t = m.Block.LookupVar("Test");
            var t1 = m.Block.LookupVar("Test1");
            Assert.IsNotNull(t);
            Assert.IsNotNull(t1);
            Assert.IsInstanceOf<ConstDeclaration>(t);
            Assert.IsInstanceOf<ConstDeclaration>(t1);
            var tp = (ConstDeclaration)t;
            var tp1 = (ConstDeclaration)t1;
            Assert.AreEqual("Test", tp.Name);
            Assert.AreEqual(m.Block.LookupType("INTEGER"), tp.Type);
            Assert.AreEqual(3, tp.Value.ToInt32());

            Assert.AreEqual("Test1", tp1.Name);
            Assert.AreEqual(m.Block.LookupType("INTEGER"), tp1.Type);
            Assert.AreEqual(2, tp1.Value.ToInt32());
            Assert.AreEqual("Const Test1 = 2", t1.ToString());
        }

        [Test]
        public void ConstSimple()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test;
CONST
  Test = 1;

 END Test.");

            var c = m.Block.LookupVar("Test");
            Assert.IsNotNull(c);
            Assert.IsInstanceOf<ConstDeclaration>(c);
            var cp = (ConstDeclaration)c;
            Assert.AreEqual("Test", cp.Name);
            Assert.AreEqual(m.Block.LookupType("INTEGER"), cp.Type);
            Assert.AreEqual(1, cp.Value.ToInt32());
        }

        [Test]
        public void ConstSimpleExpr()
        {
            Module m = Oberon0Compiler.CompileString(
                @"MODULE Test;
CONST
  Test = 1+1;

 END Test.");

            var c = m.Block.LookupVar("Test");
            Assert.IsNotNull(c);
            Assert.IsInstanceOf<ConstDeclaration>(c);
            var cp = (ConstDeclaration)c;
            Assert.AreEqual("Test", cp.Name);
            Assert.AreEqual(m.Block.LookupType("INTEGER"), cp.Type);
            Assert.AreEqual(2, cp.Value.ToInt32());
        }

        [Test]
        public void ConstSimpleFailDuplicate()
        {
            TestHelper.CompileString(
                @"MODULE Test;
CONST
  Test = 1;
  Test = 2;

 END Test.",
                "A variable/constant with this name has been defined already");
        }

        [Test]
        public void ConstSimpleFailVarReference()
        {
            TestHelper.CompileString(
                @"MODULE Test;
VAR
  Test : INTEGER;
CONST
  Test1 = 2+Test;

 END Test.",
                "A constant must resolve during compile time");
        }
    }
}