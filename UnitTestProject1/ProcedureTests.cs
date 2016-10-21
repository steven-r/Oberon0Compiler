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

END Test.");

            Assert.NotNull(m.Block.LookupFunction("Test1"));
        }

        [Test]
        public void Proc2()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
PROCEDURE Test1 (a: INTEGER);

END Test1;

END Test.");

            FunctionDeclaration p = m.Block.LookupFunction("Test1");
            Assert.NotNull(p);
            Assert.AreEqual(1, p.Parameters.Count);
            Assert.AreEqual("a", p.Parameters[0].Name);
            Assert.AreEqual(false, p.Parameters[0].IsVar);
        }

        [Test]
        public void Proc3()
        {
            var compiler = new CompilerParser();
            Module m = compiler.Calculate(@"MODULE Test; 
PROCEDURE Test1 (VAR a: INTEGER);

END Test1;

END Test.");

            FunctionDeclaration p = m.Block.LookupFunction("Test1");
            Assert.NotNull(p);
            Assert.AreEqual(1, p.Parameters.Count);
            Assert.AreEqual(BaseType.IntType, p.Parameters[0].Type.BaseType);
            Assert.AreEqual(true, p.Parameters[0].IsVar);
        }

    }
}
