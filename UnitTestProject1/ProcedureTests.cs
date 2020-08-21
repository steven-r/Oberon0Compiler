#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;
using Oberon0.Test.Support;
using Oberon0System.Attributes;
using Xunit;

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
            var m = TestHelper.CompileString(
                @"MODULE Test; 
PROCEDURE Test1;

END Test1;

END Test.");

            Assert.NotNull(m.Block.LookupFunction("Test1", null));
        }

        [Fact]
        public void Proc2()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
PROCEDURE Test1 (a: INTEGER);

END Test1;

END Test.");

            var p = m.Block.LookupFunction("Test1", null, "INTEGER");
            Assert.NotNull(p);
            Assert.Single(p.Block.Declarations);
            Assert.Equal("a", p.Block.Declarations[0].Name);
            Assert.False(((ProcedureParameterDeclaration) p.Block.Declarations[0]).IsVar);
        }

        [Fact]
        public void Proc3()
        {
            var m = TestHelper.CompileString(
                @"MODULE Test; 
PROCEDURE Test1 (VAR a: INTEGER);

END Test1;

END Test.");

            var p = m.Block.LookupFunction("Test1", null, "&INTEGER");
            Assert.NotNull(p);
            Assert.Single(p.Block.Declarations);
            Assert.Equal("a", p.Block.Declarations[0].Name);
            Assert.True(((ProcedureParameterDeclaration) p.Block.Declarations[0]).IsVar);
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
            var m = TestHelper.CompileString(
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
            var m = TestHelper.CompileString(
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
            var m = TestHelper.CompileString(
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

        [Fact]
        public void AddExternalFunctionCallByValue()
        {
            var m = new Module(null);
            var function = m.AddExternalFunctionDeclaration(new Oberon0ExportAttribute("__Test1", "INTEGER", "INTEGER"),
                "Test",
                "__TEST");
            m.Block.Procedures.Add(function);
            var f = m.Block.LookupFunction("__Test1", null, "INTEGER");
            Assert.NotNull(f);
            Assert.IsType<ExternalFunctionDeclaration>(f);
        }

        [Fact]
        public void AddExternalFunctionCallByValueTwoParameters()
        {
            var m = new Module(null);
            var function = m.AddExternalFunctionDeclaration(
                new Oberon0ExportAttribute("__Test1", "INTEGER", "INTEGER", "REAL"), "Test",
                "__TEST");
            m.Block.Procedures.Add(function);
            Assert.Throws<InvalidOperationException>(() => m.Block.LookupFunction("__Test1", null, "INTEGER"));
            var f = m.Block.LookupFunction("__Test1", null, "INTEGER,REAL");
            Assert.NotNull(f);
            var externalFunction = Assert.IsAssignableFrom<ExternalFunctionDeclaration>(f);
            var paramList = externalFunction.Block.Declarations.OfType<ProcedureParameterDeclaration>();
            var parameters = paramList as ProcedureParameterDeclaration[] ?? paramList.ToArray();
            Assert.Equal(2, parameters.Count());
            Assert.Equal(SimpleTypeDefinition.IntType.Name, parameters[0].Type.Name);
            Assert.False(parameters[0].IsVar);
            Assert.Equal(SimpleTypeDefinition.RealType.Name, parameters[1].Type.Name);
            Assert.False(parameters[1].IsVar);
        }

        [Fact]
        public void AddExternalFunctionCallByValueNoParameters()
        {
            var m = new Module(null);
            var function = m.AddExternalFunctionDeclaration(new Oberon0ExportAttribute("__Test1", "INTEGER"), "Test",
                "__TEST");
            m.Block.Procedures.Add(function);
            var f = m.Block.LookupFunction("__Test1", null);
            Assert.NotNull(f);
            var externalFunction = Assert.IsAssignableFrom<ExternalFunctionDeclaration>(f);
            var paramList = externalFunction.Block.Declarations.OfType<ProcedureParameterDeclaration>();
            var parameters = paramList as ProcedureParameterDeclaration[] ?? paramList.ToArray();
            Assert.Empty(parameters);
        }

        [Fact]
        public void AddExternalFunctionThrowMissingAttr()
        {
            var m = new Module(null);
            var t = Assert.Throws<ArgumentNullException>(() => m.AddExternalFunctionDeclaration(null!, "Test", "Test"));
            Assert.Equal("attr", t.ParamName);
        }

        [Fact]
        public void AddExternalFunctionThrowMissingClassName()
        {
            var m = new Module(null);
            var t = Assert.Throws<ArgumentNullException>(() =>
                m.AddExternalFunctionDeclaration(new Oberon0ExportAttribute("name", "INTEGER"), null!, "Test"));
            Assert.Equal("className", t.ParamName);
        }

        [Fact]
        public void AddExternalFunctionThrowMissingMethodName()
        {
            var m = new Module(null);
            var t = Assert.Throws<ArgumentNullException>(() =>
                m.AddExternalFunctionDeclaration(new Oberon0ExportAttribute("name", "INTEGER"), "Test", null!));
            Assert.Equal("methodName", t.ParamName);
        }

        [Fact]
        public void AddExternalFunctionThrowWrongReturnType()
        {
            var m = new Module(null);
            var t = Assert.Throws<InvalidOperationException>(() =>
                m.AddExternalFunctionDeclaration(new Oberon0ExportAttribute("name", "UNKNOWN"), "Test", "Demo"));
            Assert.Equal("Return type UNKNOWN not defined", t.Message);
        }

        [Fact]
        public void GetProcedureParameterByNameThrowMissingName()
        {
            var m = new Module(null);
            var t = Assert.Throws<ArgumentNullException>(() =>
                Module.GetProcedureParameterByName(null!, "INTEGER", m.Block));
            Assert.Equal("parameterName", t.ParamName);
        }

        [Fact]
        public void GetProcedureParameterByNameThrowMissingTypeName()
        {
            var m = new Module(null);
            var t = Assert.Throws<ArgumentNullException>(() =>
                Module.GetProcedureParameterByName("isThere", null!, m.Block));
            Assert.Equal("parameterType", t.ParamName);
        }

        [Fact]
        public void GetProcedureParameterByNameUseVar()
        {
            var m = new Module(null);
            var f = Module.GetProcedureParameterByName("isThere", "VAR INTEGER", m.Block);
            Assert.NotNull(f);
            var p = Assert.IsType<ProcedureParameterDeclaration>(f);
            Assert.True(p.IsVar);
            Assert.IsType(SimpleTypeDefinition.IntType.GetType(), p.Type);
        }

        [Fact]
        public void GetProcedureParameterByNameThrowMissingBlock()
        {
            var t = Assert.Throws<ArgumentNullException>(() =>
                Module.GetProcedureParameterByName("isThere", "INTEGER", null!));
            Assert.Equal("block", t.ParamName);
        }

        [Fact]
        public void GetProcedureParameterByNameThrowUnknownType()
        {
            var m = new Module(null);
            var t = Assert.Throws<ArgumentException>(() =>
                Module.GetProcedureParameterByName("isThere", "UNKNOWN", m.Block));
            Assert.Equal("UNKNOWN is not a valid type reference (Parameter 'typeString')", t.Message);
        }

        [Fact]
        public void AddExternalFunctionCheckTypes()
        {
            var m = new Module(null);
            var function = m.AddExternalFunctionDeclaration(new Oberon0ExportAttribute("__Test1", "INTEGER", "INTEGER"),
                "Test",
                "__TEST");
            m.Block.Procedures.Add(function);
            var f = m.Block.LookupFunction("__Test1", null, "INTEGER");
            Assert.NotNull(f);
            var ext = Assert.IsType<ExternalFunctionDeclaration>(f);
            Assert.Equal("Test", ext.ClassName);
            Assert.Equal("__TEST", ext.MethodName);
            Assert.False(ext.Exportable);
        }

        [Fact]
        public void TestBuildPrototypeWithVoidResultNoParams()
        {
            Assert.Equal("TestFunction()", BuildPrototypeTester("TestFunction", TypeDefinition.VoidTypeName));
        }

        [Fact]
        public void TestBuildPrototypeWithIntResultNoParams()
        {
            Assert.Equal("INTEGER TestFunction()", BuildPrototypeTester("TestFunction", "INTEGER"));
        }

        [Fact]
        public void TestBuildPrototypeWithIntResultOneParam()
        {
            Assert.Equal("INTEGER TestFunction(INTEGER)",
                BuildPrototypeTester("TestFunction", "INTEGER", ("a", "INTEGER")));
        }

        [Fact]
        public void TestBuildPrototypeWithIntResultTwoParamIncludingReference()
        {
            Assert.Equal("INTEGER TestFunction(INTEGER,&REAL)",
                BuildPrototypeTester("TestFunction", "INTEGER", ("a", "INTEGER"), ("b", "&REAL")));
        }

        [Fact]
        public void TestBuildPrototypeWithIntResultArray()
        {
            Assert.Equal("INTEGER TestFunction(INTEGER[5])",
                BuildPrototypeTester("TestFunction", "INTEGER", ("a", "INTEGER[5]")));
        }

        [Fact]
        public void TestBuildPrototypeWithIntResultArrayRef()
        {
            Assert.Equal("INTEGER TestFunction(&INTEGER[5])",
                BuildPrototypeTester("TestFunction", "INTEGER", ("a", "VAR INTEGER[5]")));
        }

        [Fact]
        public void TestBuildPrototypeWithIntResultRecordFail()
        {
            var e = Assert.Throws<ArgumentException>(() =>
                BuildPrototypeTester("TestFunction", "INTEGER", ("a", "RECORD a: INTEGER END")));
            Assert.Equal("RECORD a: INTEGER END is not a valid type reference (Parameter 'typeString')", e.Message);
            Assert.Equal("typeString", e.ParamName);
        }

        private static string BuildPrototypeTester(string name, string returnTypeName,
                                                   params (string, string)[] parameters)
        {
            var m = new Module(null);
            var paramList = new List<ProcedureParameterDeclaration>();
            if (parameters != null)
            {
                paramList.AddRange(parameters.Select(parameter =>
                    Module.GetProcedureParameterByName(parameter.Item1, parameter.Item2, m.Block)));
            }

            var f = new FunctionDeclaration(name, m.Block, m.Block.LookupType(returnTypeName), paramList.ToArray());
            string prototype = FunctionDeclaration.BuildPrototype(f.Name, f.ReturnType,
                f.Block.Declarations.OfType<ProcedureParameterDeclaration>().ToArray());
            return prototype;
        }
    }
}
