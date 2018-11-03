using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oberon0.Compiler.Tests.Types
{
    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Types;
    using Oberon0.TestSupport;

    [TestFixture]
    public class ArrayTests
    {
        [Test]
        public void ArrayType()
        {
            Module m = TestHelper.CompileString(
                @"
MODULE test; 
TYPE 
    aType= ARRAY 5 OF INTEGER;
VAR 
  a : aType;
                
END test.");
            var type = m.Block.LookupType("aType");
            Assert.NotNull(type);
            var arrayType = type as ArrayTypeDefinition;
            Assert.NotNull(arrayType);
            Assert.AreEqual("ARRAY 5 OF INTEGER", arrayType.ToString());
        }
    }
}
