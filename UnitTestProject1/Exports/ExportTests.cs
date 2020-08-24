#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;
using Oberon0.Test.Support;
using Xunit;

namespace Oberon0.Compiler.Tests.Exports
{
    public class ExportTests
    {
        [Fact]
        public void ModuleExportConst()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test;
CONST
  TestConst* = 1;
  LocalConst = 2;

BEGIN
    WriteInt(TestConst)
END Test.");

            var c = m.Block.LookupVar("TestConst");
            Assert.NotNull(c);
            Assert.IsType<ConstDeclaration>(c);
            var cp = (ConstDeclaration) c;
            Assert.True(cp.Exportable);
            c = m.Block.LookupVar("LocalConst");
            Assert.IsType<ConstDeclaration>(c);
            cp = (ConstDeclaration) c;
            Assert.False(cp.Exportable);
            Assert.True(m.HasExports);
        }

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
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

        [Fact]
        public void ModuleExportTest()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test;

END Test.");
            Assert.False(m.HasExports);
        }

        [Fact]
        public void ModuleExportType()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test;
TYPE
  ExportType* = RECORD a: INTEGER END;
  ExportSimple* = INTEGER; (* check if integer is changed as well *)
  LocalSimple = INTEGER;

END Test.");

            var t = m.Block.LookupType("ExportType");
            Assert.NotNull(t);
            Assert.IsType<RecordTypeDefinition>(t);
            var cp = (RecordTypeDefinition) t;
            Assert.True(cp.Exportable);
            t = m.Block.LookupType("ExportSimple");
            Assert.IsAssignableFrom<TypeDefinition>(t);
            Assert.True(t.Exportable);
            t = m.Block.LookupType("INTEGER");
            Assert.IsAssignableFrom<TypeDefinition>(t);
            Assert.False(t.Exportable);
            t = m.Block.LookupType("LocalSimple");
            Assert.IsAssignableFrom<TypeDefinition>(t);
            Assert.False(t.Exportable);
            Assert.True(m.HasExports);
        }

        [Fact]
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

        [Fact]
        public void ModuleExportVar()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test;
VAR
  TestVar* : INTEGER;

END Test.");

            var c = m.Block.LookupVar("TestVar");
            Assert.NotNull(c);
            Assert.True(c.Exportable);
            Assert.True(m.HasExports);
        }

        [Fact]
        public void ModuleExportVarMult()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test;
VAR
  a, TestVar*, c : INTEGER;

END Test.");

            var c = m.Block.LookupVar("TestVar");
            Assert.NotNull(c);
            Assert.True(c.Exportable);
            c = m.Block.LookupVar("a");
            Assert.NotNull(c);
            Assert.False(c.Exportable);
            c = m.Block.LookupVar("c");
            Assert.NotNull(c);
            Assert.False(c.Exportable);
            Assert.True(m.HasExports);
        }

        [Fact]
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

        [Fact]
        public void ProcExportTest()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test;
PROCEDURE TestProc*;
BEGIN
END TestProc;

END Test.");

            var p = m.Block.LookupFunction("TestProc", null);
            Assert.NotNull(p);
            Assert.True(p.Exportable);
            Assert.True(m.HasExports);
        }
    }
}
