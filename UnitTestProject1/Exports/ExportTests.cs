#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using NUnit.Framework;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;
using Oberon0.TestSupport;

namespace Oberon0.Compiler.Tests.Exports
{
    [TestFixture]
    public class ExportTests
    {
        [Test]
        public void ModuleExportConst()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test;
CONST
  TestConst* = 1;
  LocalConst = 2;

BEGIN
    WriteInt(TestConst)
END Test.");

            var c = m.Block.LookupVar("TestConst");
            Assert.IsNotNull(c);
            Assert.IsInstanceOf<ConstDeclaration>(c);
            var cp = (ConstDeclaration)c;
            Assert.IsTrue(cp.Exportable);
            c = m.Block.LookupVar("LocalConst");
            Assert.IsInstanceOf<ConstDeclaration>(c);
            cp = (ConstDeclaration)c;
            Assert.IsFalse(cp.Exportable);
            Assert.IsTrue(m.HasExports);
        }

        [Test]
        public void ModuleExportGlobalFail()
        {
            TestHelper.CompileString(
                @"MODULE Test;
PROCEDURE TestProc(VAR x*: INTEGER);
BEGIN
END TestProc;

END Test.",
                "no viable alternative at input 'VARx*'",
                "extraneous input ')' expecting ';'",
                "Exportable elements can only be defined as global");
        }

        [Test]
        public void ModuleExportGlobalFailConst()
        {
            TestHelper.CompileString(
                @"MODULE Test;
PROCEDURE TestProc;
CONST
    x* = 21;
BEGIN
END TestProc;

END Test.",
                "Exportable elements can only be defined as global");
        }

        [Test]
        public void ModuleExportGlobalFailType()
        {
            TestHelper.CompileString(
                @"MODULE Test;
PROCEDURE TestProc;
TYPE
    t* = INTEGER;
BEGIN
END TestProc;

END Test.",
                "Exportable elements can only be defined as global");
        }

        [Test]
        public void ModuleExportGlobalFailVar()
        {
            TestHelper.CompileString(
                @"MODULE Test;
PROCEDURE TestProc;
VAR
    x*: INTEGER;
BEGIN
END TestProc;

END Test.",
                "Exportable elements can only be defined as global");
        }

        [Test]
        public void ModuleExportTest()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test;

END Test.");
            Assert.IsFalse(m.HasExports);
        }

        [Test]
        public void ModuleExportType()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test;
TYPE
  ExportType* = RECORD a: INTEGER END;
  ExportSimple* = INTEGER; (* check if integer is changed as well *)
  LocalSimple = INTEGER;

END Test.");

            var t = m.Block.LookupType("ExportType");
            Assert.IsNotNull(t);
            Assert.IsInstanceOf<RecordTypeDefinition>(t);
            var cp = (RecordTypeDefinition)t;
            Assert.IsTrue(cp.Exportable);
            t = m.Block.LookupType("ExportSimple");
            Assert.IsInstanceOf<TypeDefinition>(t);
            Assert.IsTrue(t.Exportable);
            t = m.Block.LookupType("INTEGER");
            Assert.IsInstanceOf<TypeDefinition>(t);
            Assert.IsFalse(t.Exportable);
            t = m.Block.LookupType("LocalSimple");
            Assert.IsInstanceOf<TypeDefinition>(t);
            Assert.IsFalse(t.Exportable);
            Assert.IsTrue(m.HasExports);
        }

        [Test]
        public void ModuleExportTypeExportCheck()
        {
            TestHelper.CompileString(
                @"MODULE Test;
TYPE
  TestType = INTEGER;
  ExportType* = INTEGER;

VAR
  TestVar* : TestType;
  TestVar2* : ExportType;
  ExportVar2* : INTEGER;

END Test.",
                "Non-basic type (TestType) need to be exportable if used on exportable elements.");
        }

        [Test]
        public void ModuleExportVar()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test;
VAR
  TestVar* : INTEGER;

END Test.");

            var c = m.Block.LookupVar("TestVar");
            Assert.IsNotNull(c);
            Assert.IsTrue(c.Exportable);
            Assert.IsTrue(m.HasExports);
        }

        [Test]
        public void ModuleExportVarMult()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test;
VAR
  a, TestVar*, c : INTEGER;

END Test.");

            var c = m.Block.LookupVar("TestVar");
            Assert.IsNotNull(c);
            Assert.IsTrue(c.Exportable);
            c = m.Block.LookupVar("a");
            Assert.IsNotNull(c);
            Assert.IsFalse(c.Exportable);
            c = m.Block.LookupVar("c");
            Assert.IsNotNull(c);
            Assert.IsFalse(c.Exportable);
            Assert.IsTrue(m.HasExports);
        }

        [Test]
        public void NestedProcFailTest()
        {
            TestHelper.CompileString(
                @"MODULE Test;
PROCEDURE TestProc;
    PROCEDURE Fail*;
    BEGIN  
    END Fail;

BEGIN
END TestProc;

END Test.",
                "Exportable elements can only be defined as global");
        }

        [Test]
        public void ProcExportTest()
        {
            Module m = TestHelper.CompileString(
                @"MODULE Test;
PROCEDURE TestProc*;
BEGIN
END TestProc;

END Test.");

            var p = m.Block.LookupFunction("TestProc", null);
            Assert.IsNotNull(p);
            Assert.IsTrue(p.Exportable);
            Assert.IsTrue(m.HasExports);
        }
    }
}