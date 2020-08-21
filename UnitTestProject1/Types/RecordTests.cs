#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using System.Collections.Generic;
using System.Linq;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;
using Oberon0.Test.Support;
using Xunit;

namespace Oberon0.Compiler.Tests.Types
{
    public class RecordTests
    {
        [Fact]
        public void RecordAssign()
        {
            var errors = new List<CompilerError>();
            var m = TestHelper.CompileString(
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
            Assert.Empty(errors);
        }

        [Fact]
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

            Assert.Single(errors);
            Assert.Equal("Left & right side do not match types", errors[0].Message);
        }

        [Fact]
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

        [Fact]
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
                "Element not found in underlying type", "Left & right side do not match types");
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void RecordMany()
        {
            var m = TestHelper.CompileString(
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
            Assert.IsType<RecordTypeDefinition>(t);
            Assert.IsType<RecordTypeDefinition>(embType);
            var rtd = (RecordTypeDefinition) t;
            Assert.Equal(4, rtd.Elements.Count);
            var d = rtd.Elements.SingleOrDefault(x => x.Name == "d");
            Assert.NotNull(d);
            Assert.Equal(embType, d.Type);
        }

        [Fact]
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

        [Fact]
        public void RecordSelector()
        {
            var m = TestHelper.CompileString(
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
            Assert.IsType<ProcedureCallStatement>(s);
            var pcs = (ProcedureCallStatement) s;
            Assert.Single(pcs.Parameters);
            Assert.IsType<VariableReferenceExpression>(pcs.Parameters[0]);
            var vre = (VariableReferenceExpression) pcs.Parameters[0];
            Assert.NotNull(vre.Selector);
            Assert.Equal(3, vre.Selector.Count);
            Assert.Equal(intType, vre.Selector.SelectorResultType);
        }

        [Fact]
        public void RecordSimple()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
TYPE
  Demo = RECORD 
    a: INTEGER
  END;

END Test.");

            var t = m.Block.LookupType("Demo");
            var intType = m.Block.LookupType("INTEGER");
            Assert.NotNull(t);
            Assert.IsType<RecordTypeDefinition>(t);
            var rtd = (RecordTypeDefinition) t;
            Assert.Single(rtd.Elements);
            var a = rtd.Elements.First();
            Assert.Equal("a", a.Name);
            Assert.Equal(intType, a.Type);
        }
    }
}
