#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/RecordTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests.Types
{
    using System.Collections.Generic;
    using System.Linq;

    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Statements;
    using Oberon0.Compiler.Types;
    using Oberon0.TestSupport;

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
        public void RecordFail1()
        {
            List<CompilerError> errors = new List<CompilerError>();
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
    }
}