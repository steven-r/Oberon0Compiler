#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcedureTests.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler.Tests/ProcedureTests.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Tests
{
    using System.Linq;

    using NUnit.Framework;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Statements;
    using Oberon0.Compiler.Types;
    using Oberon0.TestSupport;

    [TestFixture]
    public class ProcedureTests
    {
        [Test]
        public void Proc1()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
PROCEDURE Test1;

END Test1;

END Test.");

            Assert.NotNull(m.Block.LookupFunction("Test1"));
        }

        [Test]
        public void Proc2()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
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
            Module m = TestHelper.CompileString(
                @"MODULE Test; 
PROCEDURE Test1 (VAR a: INTEGER);

END Test1;

END Test.");

            FunctionDeclaration p = m.Block.LookupFunction("Test1");
            Assert.NotNull(p);
            Assert.AreEqual(1, p.Block.Declarations.Count);
            Assert.AreEqual("a", p.Block.Declarations[0].Name);
            Assert.AreEqual(true, ((ProcedureParameter)p.Block.Declarations[0]).IsVar);
        }

        [Test]
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

        [Test]
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
            var proc = m.Block.LookupFunction("TestProc");
            Assert.NotNull(proc);
            Assert.AreEqual("TestProc", proc.Name);
            Assert.AreEqual(0, proc.Block.Declarations.OfType<ProcedureParameter>().Count());
            Assert.AreEqual(1, proc.Block.Declarations.Count);
            var x = m.Block.LookupVar("x");
            Assert.IsNull(x);
            x = proc.Block.LookupVar("x");
            Assert.NotNull(x);
            Assert.AreEqual(BaseTypes.Int, x.Type.Type);
            Assert.AreEqual("x:INTEGER", x.ToString());
        }

        [Test]
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
            var proc = m.Block.LookupFunction("TestProc");
            Assert.NotNull(proc);
            Assert.AreEqual("TestProc", proc.Name);
            Assert.AreEqual(0, proc.Block.Declarations.OfType<ProcedureParameter>().Count());
            Assert.AreEqual(0, proc.Block.Declarations.Count);
            var x = proc.Block.LookupVar("x", false);
            Assert.IsNull(x);
            x = proc.Block.LookupVar("x");
            Assert.NotNull(x);
            x = m.Block.LookupVar("x");
            Assert.NotNull(x);
            Assert.AreEqual(BaseTypes.Int, x.Type.Type);
            Assert.AreEqual("x:INTEGER", x.ToString());
        }

        [Test]
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
            var proc = m.Block.LookupFunction("TestProc");
            Assert.NotNull(proc);
            Assert.AreEqual("TestProc", proc.Name);
            Assert.AreEqual(1, m.Block.Statements.Count);
            var statement = m.Block.Statements[0] as ProcedureCallStatement;
            Assert.NotNull(statement);
            Assert.NotNull(statement.FunctionDeclaration);
        }

        [Test]
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

        [Test]
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

        [Test]
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
                "Parameter x requires a variable reference, not an expression");
        }
    }
}