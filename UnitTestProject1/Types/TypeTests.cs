#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Exceptions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;
using Oberon0.Test.Support;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Compiler.Tests.Types;

public class TypeTests(ITestOutputHelper output)
{
    [Fact]
    public void LookupType()
    {
        var m = TestHelper.CompileString(
            """
            MODULE Test; 
            TYPE
              Demo = INTEGER;

            END Test.
            """);

        Assert.NotNull(m.Block.LookupType("Demo"));
    }

    [Fact]
    public void LookupTypeFail()
    {
        var m = TestHelper.CompileString(
            """
            MODULE Test; 
            TYPE
              Demo = INTEGER;

            END Test.
            """);

        Assert.Null(m.Block.LookupType("?Unknown"));
    }

    [Fact]
    public void SimpleType()
    {
        var m = TestHelper.CompileString(
            """
            MODULE Test; 
            TYPE
              Demo = INTEGER;

            END Test.
            """);

        var intType = m.Block.LookupType("INTEGER");
        Assert.NotNull(intType);
        
        var t = m.Block.LookupType("Demo");
        Assert.NotNull(t);
        var std = Assert.IsType<SimpleTypeDefinition>(t);

        Assert.Equal(intType.Type, std.Type);
    }

    [Fact]
    public void TypeDefinedTwiceError()
    {
        TestHelper.CompileString(
            """
            MODULE Test; 
            TYPE
              Demo = INTEGER;
              Demo = INTEGER;

            END Test.
            """,
            output,
            "Type Demo declared twice");
    }

    [Fact]
    public void NameDefinedTwiceError()
    {
        var m = TestHelper.CompileString(
            """
            MODULE Test; 
            TYPE
              Demo = INTEGER;
            VAR
              Demo: INTEGER;

            END Test.
            """,
            output);
        Assert.NotNull(m);
        var t = m.Block.LookupType("Demo");
        Assert.NotNull(t);
        var tt = Assert.IsAssignableFrom<TypeDefinition>(t);
        Assert.Equal(BaseTypes.Int, tt.Type);

        var v = m.Block.LookupVar("Demo");
        Assert.NotNull(v);
        Assert.Equal(BaseTypes.Int, v.Type.Type);
    }


    [Fact]
    public void TypeEquality()
    {
        var m = TestHelper.CompileString(
            """
            MODULE Test; 
            TYPE
              Demo = INTEGER;
            VAR
              i: INTEGER;
              j: Demo;

            BEGIN
              i := 0;
              j := i+1;
              WriteInt(i); WriteString(', '); WriteInt(j); WriteLn
            END Test.
            """);

        var i = m.Block.LookupVar("i");
        Assert.NotNull(i);
        var j = m.Block.LookupVar("j");
        Assert.NotNull(j);
        Assert.Equal(6, m.Block.Statements.Count);
        var s1 = m.Block.Statements[0];
        var s2 = m.Block.Statements[1];
        
        var as1 = Assert.IsType<AssignmentStatement>(s1);
        var as2 = Assert.IsType<AssignmentStatement>(s2);
        Assert.IsType<ConstantIntExpression>(as1.Expression);
        Assert.IsType<BinaryExpression>(as2.Expression);
        Assert.Equal(as1.Expression.TargetType, as2.Expression.TargetType);
        Assert.NotEqual(as1.Expression.TargetType, j.Type);
        Assert.Equal(as1.Expression.TargetType.Type, j.Type.Type);
    }

    [Fact]
    public void LookupBaseTypeFail()
    {
        var b = new Block(null, new Module(null));
        Assert.Throws<InternalCompilerException>(() => b.LookupTypeByBaseType(BaseTypes.Any));
    }

    [Fact]
    public void TestInternalConstants()
    {
        var m = new Module(null);

        var var = m.Block.LookupVar("TRUE");
        Assert.NotNull(var);
        var constant = Assert.IsAssignableFrom<ConstDeclaration>(var);
        Assert.NotNull(constant);
        Assert.True(constant.Value.Internal);

        var = m.Block.LookupVar("FALSE");
        Assert.NotNull(var);
        constant = Assert.IsAssignableFrom<ConstDeclaration>(var);
        Assert.NotNull(constant);
        Assert.True(constant.Value.Internal);
    }


    [Fact]
    public void TestInternalFlagNotSetForCustomConstant()
    {
        var m = TestHelper.CompileString(
            """
            MODULE Test; 
            CONST
              test = 1;

            BEGIN
            END Test.
            """);

        var i = m.Block.LookupVar("test");
        Assert.NotNull(i);

        var constant = Assert.IsType<ConstDeclaration>(i);
        Assert.NotNull(constant);
        Assert.False(constant.Value.Internal);
    }
}