#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests.Types
{
    [TestFixture]
    public class RecordTests
    {
        [Test]
        public void RecordAssign()
        {
            var errors = new List<CompilerError>();
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
TYPE
  Demo = RECORD 
    a: INTEGER
  END;
VAR
  r1: Demo;
  r2: Demo;

BEGIN
  r1.a := 42;
  r2 := r1;
END Test.",
                errors);

            Assert.NotNull(m);
            Assert.AreEqual(0, errors.Count);
        }

        [Test]
        public void RecordAssignFail()
        {
            var errors = new List<CompilerError>();
            TestHelper.CompileString(
                @"MODULE Test; 
TYPE
  Demo = RECORD 
    a: INTEGER
  END;
VAR
  r1: Demo;
  r2: RECORD 
    a: INTEGER
  END;

BEGIN
  r1.a := 42;
  r2 := r1;
END Test.",
                errors);

            Assert.AreEqual(1, errors.Count);
            Assert.AreEqual("Left & right side do not match types", errors[0].Message);
        }

        [Test]
        public void RecordConstRecordSelectorError()
        {
            TestHelper.CompileString(
                @"MODULE test; 
CONST 
  ci = 12;

VAR 
  a : INTEGER;
                
BEGIN
    a := ci.s;
END test.",
                "Simple variables or constants do not allow any selector");
        }

        [Test]
        public void RecordElementNotFoundError()
        {
            TestHelper.CompileString(
                @"MODULE test; 
TYPE 
    rType = RECORD a: INTEGER END;

VAR 
  r : rType;
                
BEGIN
    r.s := 1;
END test.",
                "Element not found in underlying type");
        }

        [Test]
        public void RecordFail1()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
TYPE
  Demo = RECORD 
    a: INTEGER;
    a: REAL
  END;

END Test.",
                "Element a defined more than once");
        }

        [Test]
        public void RecordFailElement()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
TYPE 
t = RECORD
   a: INTEGER
END;
t2 = RECORD
   a: INTEGER
END;

VAR a: t;
    b: t2;
BEGIN
  a := b;
END Test.",
                "Left & right side do not match types");
        }

        [Test]
        public void RecordFailSimple()
        {
            TestHelper.CompileString(
                @"MODULE Test; 

VAR a: RECORD
      b: INTEGER
    END;
    b: INTEGER;
BEGIN
  a := b;
END Test.", 
                "Left & right side do not match types");
        }

        [Test]
        public void RecordMany()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
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
        public void RecordNoRecordTypeSelectorError()
        {
            TestHelper.CompileString(
                @"MODULE test; 
TYPE 
    aType= ARRAY 5 OF INTEGER;

VAR 
  a : aType;
                
BEGIN
    a.s := 1;
END test.",
                "Record reference expected");
        }

        [Test]
        public void RecordSelector()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
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
            var pcs = (ProcedureCallStatement)s;
            Assert.AreEqual(1, pcs.Parameters.Count);
            Assert.IsInstanceOf<VariableReferenceExpression>(pcs.Parameters[0]);
            VariableReferenceExpression vre = (VariableReferenceExpression)pcs.Parameters[0];
            Assert.NotNull(vre.Selector);
            Assert.AreEqual(3, vre.Selector.Count);
            Assert.AreEqual(intType, vre.Selector.SelectorResultType);
        }

        [Test]
        public void RecordSimple()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
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
    }
}