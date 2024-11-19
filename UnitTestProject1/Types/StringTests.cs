#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Statements;
using Oberon0.Compiler.Types;
using Oberon0.Test.Support;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Compiler.Tests.Types;

public class StringTests(ITestOutputHelper testOutput)
{
    [Fact]
    public void StringDefinition()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              s : STRING;
                            
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = m.Block.LookupVar("s");
        Assert.NotNull(s);
        Assert.Equal(SimpleTypeDefinition.StringType.Type, s.Type.Type);
    }

    [Fact]
    public void StringLiteralAssignment()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              s : STRING;
            BEGIN
              s := 'Hello Test'
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[0]);
        var se = Assert.IsType<StringExpression>(s.Expression);
        Assert.Equal("Hello Test", se.Value);
    }

    [Fact]
    public void StringLiteralAssignmentVar()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              s, s1 : STRING;
            BEGIN
              s := 'Hello Test';
              s1 := s
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[0]);
        var s1 = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[1]);
        var se = Assert.IsType<StringExpression>(s.Expression);
        Assert.Equal("Hello Test", se.Value);
        Assert.IsType<VariableReferenceExpression>(s1.Expression);
    }

    [Fact]
    public void StringLiteralAssignmentFail()
    {
        TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              s: INTEGER;
            BEGIN
              s := 'Hello Test'
            END test.
            """,
            testOutput,
            "Left & right side do not match types");
    }

    [Fact]
    public void StringLiteralAssignmentFail2()
    {
        TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              s: STRING;
            BEGIN
              s := 1
            END test.
            """,
            testOutput,
            "Left & right side do not match types");
    }

    [Fact]
    public void StringLiteralAssignmentToStringIntLiteral()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              s: STRING;
            BEGIN
              s := ToString(1)
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[0]);
        var se = Assert.IsType<StringExpression>(s.Expression);
        Assert.Equal("1", se.Value);
    }

    [Fact]
    public void StringLiteralAssignmentToStringRealLiteral()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              s: STRING;
            BEGIN
              s := ToString(1.1)
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[0]);
        var se = Assert.IsType<StringExpression>(s.Expression);
        Assert.Equal("1.1", se.Value);
    }

    [Fact]
    public void StringLiteralAssignmentToStringBoolLiteral()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              s: STRING;
            BEGIN
              s := ToString(TRUE)
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[0]);
        var se = Assert.IsType<StringExpression>(s.Expression);
        Assert.Equal("True", se.Value);
    }

    [Fact]
    public void StringLengthLiteral()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              l: INTEGER;
            BEGIN
              l := Length('Hello Test');
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[0]);
        var se = Assert.IsType<ConstantIntExpression>(s.Expression);
        Assert.Equal(10, se.ToInt32());
    }

    [Fact]
    public void StringLengthVar()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              l: INTEGER;
              s: STRING;
            BEGIN
              s := 'Hello String';
              l := Length(s);
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[1]);
        var se = Assert.IsType<FunctionCallExpression>(s.Expression);
        Assert.Equal("INTEGER Length(STRING)", se.FunctionDeclaration.ToString());
    }

    [Fact]
    public void StringAddVarVar()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              a: STRING;
              b: STRING;
              r: STRING;
            BEGIN
              a := 'Hello String';
              b := 'Hello';
              r := a + b;
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[2]);
        var se = Assert.IsType<BinaryExpression>(s.Expression);
        Assert.Equal(BaseTypes.String, se.TargetType.Type);
        Assert.IsType<VariableReferenceExpression>(se.LeftHandSide);
        Assert.IsType<VariableReferenceExpression>(se.RightHandSide);

    }

    [Fact]
    public void StringAddVarString()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              a: STRING;
              r: STRING;
            BEGIN
              a := 'Hello ';
              r := a + 'String';
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[1]);
        var se = Assert.IsType<BinaryExpression>(s.Expression);
        Assert.Equal(BaseTypes.String, se.TargetType.Type);
        Assert.IsType<VariableReferenceExpression>(se.LeftHandSide);
        Assert.IsType<StringExpression>(se.RightHandSide);
    }


    [Fact]
    public void StringAddStringString()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              r: STRING;
            BEGIN
              r := 'Hello ' + 'String';
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[0]);
        var se = Assert.IsType<StringExpression>(s.Expression);
        Assert.Equal("Hello String", se.Value);
    }

    [Fact]
    public void TestStringMultVarVar()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              a: STRING;
              b: INTEGER;
              r: STRING;
            BEGIN
              a := 'Hello String';
              b := 5;
              r := a * b;
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[2]);
        var se = Assert.IsType<BinaryExpression>(s.Expression);
        Assert.Equal(BaseTypes.String, se.TargetType.Type);
        Assert.Equal(OberonGrammarLexer.STAR, se.Operator);
        var vr = Assert.IsType<VariableReferenceExpression>(se.LeftHandSide);
        Assert.Equal("a", vr.Name);
        Assert.IsType<VariableReferenceExpression>(se.RightHandSide);
    }

    [Fact]
    public void TestStringMultVarInt()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              a: STRING;
              r: STRING;
            BEGIN
              a := 'Hello String';
              r := a * 5;
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[1]);
        var expression = Assert.IsType<BinaryExpression>(s.Expression);
        Assert.Equal(BaseTypes.String, expression.TargetType.Type);
        Assert.Equal(OberonGrammarLexer.STAR, expression.Operator);
        var vr = Assert.IsType<VariableReferenceExpression>(expression.LeftHandSide);
        Assert.Equal("a", vr.Name);
        Assert.IsType<ConstantIntExpression>(expression.RightHandSide);
    }

    [Fact]
    public void TestStringMultStringInt()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              r: STRING;
            BEGIN
              r := 'Hello' * 5;
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[0]);
        var stringExpression = Assert.IsType<StringExpression>(s.Expression);
        Assert.Equal("HelloHelloHelloHelloHello", stringExpression.Value);
    }

    [Fact]
    public void TestStringMultStringVar()
    {
        var m = TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              b: INTEGER;
              r: STRING;
            BEGIN
              b := 5;
              r := 'Hello String' * b;
            END test.
            """,
            testOutput);
        Assert.NotNull(m);
        var s = Assert.IsAssignableFrom<AssignmentStatement>(m.Block.Statements[1]);
        var expression = Assert.IsType<BinaryExpression>(s.Expression);
        Assert.Equal(BaseTypes.String, expression.TargetType.Type);
        Assert.Equal(OberonGrammarLexer.STAR, expression.Operator);
        var se = Assert.IsType<StringExpression>(expression.LeftHandSide);
        Assert.Equal("Hello String", se.Value);
        var vr = Assert.IsType<VariableReferenceExpression>(expression.RightHandSide);
        Assert.Equal("b", vr.Name);
    }

}