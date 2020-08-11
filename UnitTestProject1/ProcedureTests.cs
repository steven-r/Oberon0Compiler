#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Linq;
using Xunit;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests
{
    public class ProcedureTests
    {
        [Fact]
        public void FuncNotFoundError()
        {
            TestHelper.CompileString(
                // ReSharper disable once StringLiteralTypo
                @"MODULE Test; 
VAR
  a: INTEGER;
BEGIN
    a := NOTFOUND() 
END Test.",
                // ReSharper disable once StringLiteralTypo
                "No procedure/function with prototype 'NOTFOUND()' found");
        }

        [Fact]
        public void Proc1()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
PROCEDURE Test1;

END Test1;

END Test.");

            Assert.NotNull(m.Block.LookupFunction("Test1", null));
        }

        [Fact]
        public void Proc2()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
PROCEDURE Test1 (a: INTEGER);

END Test1;

END Test.");

            FunctionDeclaration p = m.Block.LookupFunction("Test1", null, "INTEGER");
            Assert.NotNull(p);
            Assert.Single(p.Block.Declarations);
            Assert.Equal("a", p.Block.Declarations[0].Name);
            Assert.False(((ProcedureParameterDeclaration)p.Block.Declarations[0]).IsVar);
        }

        [Fact]
        public void Proc3()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
PROCEDURE Test1 (VAR a: INTEGER);

END Test1;

END Test.");

            FunctionDeclaration p = m.Block.LookupFunction("Test1", null, "&INTEGER");
            Assert.NotNull(p);
            Assert.Single(p.Block.Declarations);
            Assert.Equal("a", p.Block.Declarations[0].Name);
            Assert.True(((ProcedureParameterDeclaration)p.Block.Declarations[0]).IsVar);
        }

        [Fact]
        public void ProcArrayCallByValue()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR 
    arr: ARRAY 5 OF INTEGER; 

    PROCEDURE TestArray(a: ARRAY 5 OF INTEGER);
    BEGIN
        IF (a[1] # 1) THEN WriteString('a is 0') END;
        a[1] := 2
    END TestArray;

BEGIN
    arr[1] := 1;
    TestArray(arr);
    WriteBool(arr[1] = 1);
    WriteLn 
END Test.",
                "No procedure/function with prototype 'TestArray(INTEGER[5])' found");
        }

        [Fact]
        public void ProcCall()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
PROCEDURE TestProc;
BEGIN
END TestProc;

BEGIN
  TestProc
END Test.");
            Assert.NotNull(m);
            var proc = m.Block.LookupFunction("TestProc", null);
            Assert.NotNull(proc);
            Assert.Equal("TestProc", proc.Name);
            Assert.Single(m.Block.Statements);
            var statement = m.Block.Statements[0] as ProcedureCallStatement;
            Assert.NotNull(statement);
            Assert.NotNull(statement.FunctionDeclaration);
        }

        [Fact]
        public void ProcDifferentName()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR 
  x: INTEGER;
PROCEDURE TestProc;
BEGIN
  x := 0;
END FailProc;

END Test.",
                "The name of the procedure does not match the name after END");
        }

        [Fact]
        public void ProcDuplicateParameterFail()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
PROCEDURE Test1 (VAR a: INTEGER; a: INTEGER);

END Test1;

END Test.",
                "Duplicate parameter",
                "Duplicate parameter");
        }

        [Fact]
        public void ProcGlobalVar()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
VAR 
  x: INTEGER;
PROCEDURE TestProc;
BEGIN
  x := 0;
END TestProc;

END Test.");
            Assert.NotNull(m);
            var proc = m.Block.LookupFunction("TestProc", null);
            Assert.NotNull(proc);
            Assert.Equal("TestProc", proc.Name);
            Assert.Empty(proc.Block.Declarations.OfType<ProcedureParameterDeclaration>());
            Assert.Empty(proc.Block.Declarations);
            var x = proc.Block.LookupVar("x", false);
            Assert.Null(x);
            x = proc.Block.LookupVar("x");
            Assert.NotNull(x);
            x = m.Block.LookupVar("x");
            Assert.NotNull(x);
            Assert.Equal(BaseTypes.Int, x.Type.Type);
            Assert.Equal("x:INTEGER", x.ToString());
        }

        [Fact]
        public void ProcLocalVar()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
PROCEDURE TestProc;
VAR 
  x: INTEGER;
BEGIN
  x := 0;
END TestProc;

END Test.");
            Assert.NotNull(m);
            var proc = m.Block.LookupFunction("TestProc", null);
            Assert.NotNull(proc);
            Assert.Equal("TestProc", proc.Name);
            Assert.Empty(proc.Block.Declarations.OfType<ProcedureParameterDeclaration>());
            Assert.Single(proc.Block.Declarations);
            var x = m.Block.LookupVar("x");
            Assert.Null(x);
            x = proc.Block.LookupVar("x");
            Assert.NotNull(x);
            Assert.Equal(BaseTypes.Int, x.Type.Type);
            Assert.Equal("x:INTEGER", x.ToString());
        }

        [Fact]
        public void ProcMissingByRefExpression()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR 
  x: INTEGER;

PROCEDURE TestProc(VAR x: INTEGER);
BEGIN
  x := 0
END TestProc;

BEGIN
  TestProc(x +1);
END Test.",
                "No procedure/function with prototype 'TestProc(INTEGER)' found");
        }

        [Fact]
        public void ProcMissingEndName()
        {
            TestHelper.CompileString(
                @"MODULE Test; 
VAR 
  x: INTEGER;
PROCEDURE TestProc;
BEGIN
  x := 0;
END ;

END Test.",
                "missing ID at ';'",
                "The name of the procedure does not match the name after END");
        }
    }
}