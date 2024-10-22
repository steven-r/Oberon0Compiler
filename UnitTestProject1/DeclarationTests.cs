#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Types;
using Oberon0.Test.Support;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Compiler.Tests;

public class DeclarationTests(ITestOutputHelper output)
{
    [Fact]
    public void ArrayFailBooleanIndex()
    {
        TestHelper.CompileString(
            """
            MODULE Test; 

            VAR
              id: ARRAY TRUE OF INTEGER;
             END Test.
            """,
            output,
            "The array size must return a constant integer expression");
    }

    [Fact]
    public void ArrayOfArray()
    {
        var m = TestHelper.CompileString(
            """
            MODULE Test; 
            VAR
              id: ARRAY 5 OF ARRAY 10 OF INTEGER;
             END Test.
            """,
            output);

        var id = m.Block.LookupVar("id");
        var intType = m.Block.LookupType("INTEGER");
        Assert.NotNull(id);

        var atd = Assert.IsType<ArrayTypeDefinition>(id.Type);

        Assert.Equal(5, atd.Size);

        var atd1 = Assert.IsType<ArrayTypeDefinition>(atd.ArrayType);
        Assert.Equal(10, atd1.Size);
        Assert.Equal(intType, atd1.ArrayType);
    }

    [Fact]
    public void OneVar()
    {
        var m = TestHelper.CompileString(
            """
            MODULE Test; 
            VAR
              id: INTEGER;
             END Test.
            """, output);

        var v = m.Block.LookupVar("id");
        Assert.NotNull(v);
        Assert.Equal(BaseTypes.Int, v.Type.Type);
    }

    [Fact]
    public void SimpleArray()
    {
        var m = TestHelper.CompileString(
            """
            MODULE Test; 
            VAR
              id: ARRAY 5 OF INTEGER;
             END Test.
            """, output);

        var id = m.Block.LookupVar("id");
        var intType = m.Block.LookupType("INTEGER");
        Assert.NotNull(id);
        Assert.NotNull(intType);

        var atd = Assert.IsType<ArrayTypeDefinition>(id.Type);

        Assert.Equal(5, atd.Size);
        Assert.Equal(intType, atd.ArrayType);
    }

    [Fact]
    public void TwoVars1()
    {
        var m = TestHelper.CompileString(
            """
            MODULE Test; 
            VAR
              id: INTEGER;
              id1: INTEGER;
             END Test.
            """);

        var id = m.Block.LookupVar("id");
        Assert.NotNull(id);
        Assert.Equal(BaseTypes.Int, id.Type.Type);
        var id1 = m.Block.LookupVar("id1");
        Assert.NotNull(id1);
        Assert.Equal(BaseTypes.Int, id1.Type.Type);
    }

    [Fact]
    public void TwoVars2()
    {
        var m = TestHelper.CompileString(
            """
            MODULE Test; 
            VAR
              id, id1: INTEGER;
             END Test.
            """);

        var id = m.Block.LookupVar("id");
        var id1 = m.Block.LookupVar("id1");
        Assert.NotNull(id);
        Assert.Equal(BaseTypes.Int, id.Type.Type);
        Assert.NotNull(id1);
        Assert.Equal(BaseTypes.Int, id1.Type.Type);
    }

    [Fact]
    public void TwoVarsFail1()
    {
        TestHelper.CompileString(
            """
            MODULE Test; 
            VAR
              id: INTEGER;
              id: INTEGER;
             END Test.
            """,
            "Variable declared twice");
    }

    [Fact]
    public void TwoVarsFail2()
    {
        TestHelper.CompileString(
            """
            MODULE Test; 
            VAR
              id, id: INTEGER;
             END Test.
            """,
            "Variable declared twice");
    }

    [Fact]
    public void VarArrayNotConst()
    {
        TestHelper.CompileString(
            """
            MODULE Test; 
            VAR
                id: INTEGER;
                arr: ARRAY id OF INTEGER;
            
             END Test.
            """,
            "The array size must return a constant integer expression");
    }

    [Fact]
    public void VarWithoutId()
    {
        TestHelper.CompileString(
            """
            MODULE Test; 
            VAR
             END Test.
            """, output, 
            "extraneous input 'END' expecting ID",
            "no viable alternative at input 'Test.'",
            "The name of the module does not match the end node");
    }

    [Fact]
    public void VarWithoutWrongType()
    {
        TestHelper.CompileString(
            """
            MODULE Test; 
            VAR
                id: DUMMY;
            
             END Test.
            """,
            "Type not known");
    }

    [Fact]
    public void ConstVarNameClash()
    {
        TestHelper.CompileString(
            """
            MODULE Test;
            CONST
                id = 1;
            VAR
                id: INTEGER;
            
             END Test.
            """,
            output,
            "Variable declared twice");
    }

}