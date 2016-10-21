﻿using System;
using NUnit.Framework;
using Oberon0.Compiler;
using Oberon0.Compiler.Definitions;

namespace UnitTestProject1
{
    [TestFixture]
    public class SimpleTests
    {
        [Test]
        public void EmptyApplication()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate("MODULE Test; END Test.");

            Assert.AreEqual("Test", m.Name);
            Assert.AreEqual(2, m.Block.Declarations.Count);
        }

        [Test]
        public void EmptyApplication2()
        {
            var compiler = new CompilerParser();
            Exception e = Assert.Throws<Loyc.LogException>(() => { Module m = compiler.Calculate(@"MODULE Test; 
BEGIN END Test."); });
            Assert.AreEqual(e.Message, "Statement expected");
        }

        [Test]
        public void ModuleMissingId()
        {
            var compiler = new CompilerParser();


            Exception e = Assert.Throws<Loyc.LogException>(() => { Module m = compiler.Calculate(@"MODULE ; 
BEGIN END Test."); });
            Assert.AreEqual(e.Message, "\'Semicolon\': expected Id");
        }

        [Test]
        public void ModuleMissingDot()
        {
            var compiler = new CompilerParser();


            Exception e = Assert.Throws<Loyc.LogException>(() => { Module m = compiler.Calculate(@"MODULE Test; 
END Test"); });
            Assert.AreEqual(e.Message, "\'EOF\': expected Dot");
        }

    }
}
