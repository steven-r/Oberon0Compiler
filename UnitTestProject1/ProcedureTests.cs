using NUnit.Framework;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Tests
{
    [TestFixture]
    public class ProcedureTests
    {
        [Test]
        public void Proc1()
        {
            Module m = Oberon0Compiler.CompileString(@"MODULE Test; 
PROCEDURE Test1;

END Test1;

END Test.");

            Assert.NotNull(m.Block.LookupFunction("Test1"));
        }

        [Test]
        public void Proc2()
        {
            Module m = Oberon0Compiler.CompileString(@"MODULE Test; 
PROCEDURE Test1 (a: INTEGER);

END Test1;

END Test.");

            FunctionDeclaration p = m.Block.LookupFunction("Test1");
            Assert.NotNull(p);
            Assert.AreEqual(1, p.Block.Declarations.Count);
            Assert.AreEqual("a", p.Block.Declarations[0].Name);
            Assert.AreEqual(false, ((ProcedureParameter)p.Block.Declarations[0]).IsVar);
        }

        [Test]
        public void Proc3()
        {
            Module m = Oberon0Compiler.CompileString(@"MODULE Test; 
PROCEDURE Test1 (VAR a: INTEGER);

END Test1;

END Test.");

            FunctionDeclaration p = m.Block.LookupFunction("Test1");
            Assert.NotNull(p);
            Assert.AreEqual(1, p.Block.Declarations.Count);
            Assert.AreEqual("a", p.Block.Declarations[0].Name);
            Assert.AreEqual(true, ((ProcedureParameter)p.Block.Declarations[0]).IsVar);
        }
    }
}
