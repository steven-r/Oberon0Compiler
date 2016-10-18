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
    public class ProcedureTests
    {
        [Test]
        public void Proc1()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
PROCEDURE Test1;

END Test1;

BEGIN END.");

            Assert.AreEqual(1, m.Block.Procedures.Count);
            Assert.AreEqual("Test1", m.Block.Procedures[0].Name);
        }

        [Test]
        public void Proc2()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
PROCEDURE Test1 (a: INTEGER);

END Test1;

BEGIN END.");

            Assert.AreEqual(1, m.Block.Procedures[0].Parameters.Count);
            Assert.AreEqual("a", m.Block.Procedures[0].Parameters[0].Name);
            Assert.AreEqual(false, m.Block.Procedures[0].Parameters[0].IsVar);
        }

        [Test]
        public void Proc3()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
PROCEDURE Test1 (VAR a: INTEGER);

END Test1;

BEGIN END.");

            Assert.AreEqual(1, m.Block.Procedures[0].Parameters.Count);
            Assert.AreEqual(true, m.Block.Procedures[0].Parameters[0].IsVar);
        }

    }
}
