using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;
using Oberon0.CompilerSupport;

namespace Oberon0.Compiler.Tests
{
    using Oberon0.Compiler.Expressions.Constant;

    [TestFixture]
    public class TypeTests
    {
        [Test]
        public void LookupType()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            Assert.NotNull(m.Block.LookupType("Demo"));
        }

        [Test]
        public void LookupTypeFail()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            Assert.IsNull(m.Block.LookupType("?Unknown"));
        }

        [Test]
        public void RecordSimple()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
TYPE
  Demo = RECORD 
    a: INTEGER
  END;

END Test.");

            var t = m.Block.LookupType("Demo");
            var intType = m.Block.LookupType("INTEGER");
            Assert.NotNull(t);
            Assert.IsInstanceOf<RecordTypeDefinition>(t);
            RecordTypeDefinition rtd = (RecordTypeDefinition)t;
            Assert.AreEqual(1, rtd.Elements.Count);
            Declaration a = rtd.Elements.First();
            Assert.AreEqual("a", a.Name);
            Assert.AreEqual(intType, a.Type);
        }

        [Test]
        public void RecordFail1()
        {
            List<CompilerError> errors = new List<CompilerError>();
            TestHelper.CompileString(@"MODULE Test; 
TYPE
  Demo = RECORD 
    a: INTEGER;
    a: REAL
  END;

END Test.", errors);
            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Element a defined more than once", errors[0].Message);
        }

        [Test]
        public void RecordMany()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
TYPE
  Embedded = RECORD
     emb: INTEGER;
     arr: ARRAY 6 OF INTEGER
  END;
  Demo = RECORD 
    a: INTEGER;
    b: STRING;
    c: REAL;
    d: Embedded
  END;

END Test.");

            var t = m.Block.LookupType("Demo");
            var embType = m.Block.LookupType("Embedded");
            Assert.NotNull(t);
            Assert.NotNull(embType);
            Assert.IsInstanceOf<RecordTypeDefinition>(t);
            Assert.IsInstanceOf<RecordTypeDefinition>(embType);
            RecordTypeDefinition rtd = (RecordTypeDefinition)t;
            Assert.AreEqual(4, rtd.Elements.Count);
            Declaration d = rtd.Elements.SingleOrDefault(x => x.Name == "d");
            Assert.NotNull(d);
            Assert.AreEqual(embType, d.Type);
        }

        [Test]
        public void RecordSelector()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
TYPE
  Embedded = RECORD
     emb: INTEGER;
     arr: ARRAY 6 OF INTEGER
  END;
  Demo = RECORD 
    a: INTEGER;
    b: STRING;
    c: REAL;
    d: Embedded
  END;
VAR 
  test: Demo;

BEGIN
  WriteInt(test.d.arr[3])
END Test.");

            var intType = m.Block.LookupType("INTEGER");
            var s = m.Block.Statements[0];
            Assert.NotNull(s);
            Assert.IsInstanceOf<ProcedureCallStatement>(s);
            var pcs = (ProcedureCallStatement) s;
            Assert.AreEqual(1, pcs.Parameters.Count);
            Assert.IsInstanceOf<VariableReferenceExpression>(pcs.Parameters[0]);
            VariableReferenceExpression vre = (VariableReferenceExpression) pcs.Parameters[0];
            Assert.NotNull(vre.Selector);
            Assert.AreEqual(3, vre.Selector.Count);
            Assert.AreEqual(intType, vre.Selector.SelectorResultType);
        }


        [Test]
        public void SimpleType()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
TYPE
  Demo = INTEGER;

END Test.");

            var t = m.Block.LookupType("Demo");
            var intType = m.Block.LookupType("INTEGER");
            Assert.NotNull(t);
            Assert.IsInstanceOf<SimpleTypeDefinition>(t);
            SimpleTypeDefinition std = (SimpleTypeDefinition)t;
            Assert.AreEqual(intType.BaseTypes, std.BaseTypes);
        }

        [Test]
        public void TypeDefinedTwiceError()
        {
            List<CompilerError> errors = new List<CompilerError>();
            TestHelper.CompileString(@"MODULE Test; 
TYPE
  Demo = INTEGER;
  Demo = INTEGER;

END Test.", errors);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Type Demo declared twice", errors[0].Message);
        }
        [Test]

        public void TypeEquality()
        {
            Module m = TestHelper.CompileString(@"MODULE Test; 
TYPE
  Demo = INTEGER;
VAR
  i: INTEGER;
  j: Demo;

BEGIN
  i := 0;
  j := i+1;
  WriteInt(i); WriteString(', '); WriteInt(j); WriteLn
END Test.");

            var i = m.Block.LookupVar("i");
            Assert.NotNull(i);
            var j = m.Block.LookupVar("j");
            Assert.NotNull(j);
            Assert.AreEqual(6, m.Block.Statements.Count);
            var s1 = m.Block.Statements[0];
            var s2 = m.Block.Statements[1];
            Assert.IsInstanceOf<AssignmentStatement>(s1);
            Assert.IsInstanceOf<AssignmentStatement>(s2);
            var as1 = (AssignmentStatement) s1;
            var as2 = (AssignmentStatement) s2;
            Assert.IsInstanceOf<ConstantIntExpression>(as1.Expression);
            Assert.IsInstanceOf<BinaryExpression>(as2.Expression);
            Assert.AreEqual(as1.Expression.TargetType, as2.Expression.TargetType);
            Assert.AreNotEqual(as1.Expression.TargetType, j.Type);
            Assert.AreEqual(as1.Expression.TargetType.BaseTypes, j.Type.BaseTypes);
        }


    }
}
