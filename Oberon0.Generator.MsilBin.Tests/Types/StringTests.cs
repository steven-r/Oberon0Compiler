#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Globalization;
using System.IO;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Generator.MsilBin.Tests.Types;

    public class StringTests(ITestOutputHelper output)
    {
        [Theory]
        [InlineData("", 0)]
        [InlineData("hello\nworld", 11)]
        [InlineData("hello", 5)]
    public void StringLengthTest(string str, int length)
        {
            string source = $"""
                                  MODULE Test; 
                                  VAR 
                                      s: STRING;

                                  BEGIN
                                      s := '{str}';
                                      WriteInt(Length(s))
                                  END Test.
                                  """;
            var cg = CompileHelper.CompileOberon0Code(source, out string code, output);
            Assert.NotEmpty(code);

            var syntaxTree = CSharpSyntaxTree.ParseText(code);

            byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
            Assert.NotNull(assembly);

            using var output1 = new StringWriter();
            Runner.Execute(assembly, output1);
            Assert.Equal($"{length}", output1.ToString());
        }

    [Fact]
    public void StringAddTest()
    {
        const string source = $"""
                                   MODULE Test; 
                                   VAR 
                                       s: STRING;

                                   BEGIN
                                       s := 'Hello ';
                                       WriteString(s + 'String')
                                   END Test.
                                   """;
        var cg = CompileHelper.CompileOberon0Code(source, out string code, output);
        Assert.NotEmpty(code);

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
        Assert.NotNull(assembly);

        using var output1 = new StringWriter();
        Runner.Execute(assembly, output1);
        Assert.Equal($"Hello String", output1.ToString());
    }

    [Fact]
    public void StringMultVarVar()
    {
        const string source = $"""
                               MODULE Test; 
                               VAR
                                 a, s: STRING;
                                 b: INTEGER;

                               BEGIN
                                   a := 'Hello';
                                   b := 5;
                                   WriteString(a * b)
                               END Test.
                               """;
        var cg = CompileHelper.CompileOberon0Code(source, out string code, output);
        Assert.NotEmpty(code);

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
        Assert.NotNull(assembly);

        using var output1 = new StringWriter();
        Runner.Execute(assembly, output1);
        Assert.Equal("HelloHelloHelloHelloHello", output1.ToString());
    }

    [Fact]
    public void StringMultVarInt()
    {
        const string source = $"""
                               MODULE Test; 
                               VAR
                                 a, s: STRING;
                                 b: INTEGER;

                               BEGIN
                                   a := 'Hello';
                                   b := 5;
                                   WriteString(a * 3)
                               END Test.
                               """;
        var cg = CompileHelper.CompileOberon0Code(source, out string code, output);
        Assert.NotEmpty(code);

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
        Assert.NotNull(assembly);

        using var output1 = new StringWriter();
        Runner.Execute(assembly, output1);
        Assert.Equal("HelloHelloHello", output1.ToString());
    }

    [Fact]
    public void StringMultStringVar()
    {
        const string source = $"""
                               MODULE Test; 
                               VAR
                                 a, s: STRING;
                                 b: INTEGER;

                               BEGIN
                                   a := 'Hello';
                                   b := 4;
                                   WriteString('HI' * b)
                               END Test.
                               """;
        var cg = CompileHelper.CompileOberon0Code(source, out string code, output);
        Assert.NotEmpty(code);

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
        Assert.NotNull(assembly);

        using var output1 = new StringWriter();
        Runner.Execute(assembly, output1);
        // ReSharper disable once StringLiteralTypo
        Assert.Equal("HIHIHIHI", output1.ToString());
    }

    [Fact]
    public void StringMultVarZero()
    {
        const string source = $"""
                               MODULE Test; 
                               VAR
                                 a, s: STRING;
                                 b: INTEGER;

                               BEGIN
                                   a := 'Hello';
                                   b := 5;
                                   WriteString(a * 0)
                               END Test.
                               """;
        var cg = CompileHelper.CompileOberon0Code(source, out string code, output);
        Assert.NotEmpty(code);

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
        Assert.NotNull(assembly);

        using var output1 = new StringWriter();
        Runner.Execute(assembly, output1);
        Assert.Equal("", output1.ToString());
    }

    [Theory]
    [InlineData(0.0, "0", "G")]
    [InlineData(1.0, "1", "G")]
    [InlineData(10000000000000.00001, "10000000000000", "G")]
    [InlineData(0.1, "0.1", "G")]
    [InlineData(2.3E-06, "2.3E-06", "G")]
    [InlineData(2.3E06, "2300000", "G")]
    [InlineData(double.MinValue, "-1.7976931348623157E+308", "G")]
    public void TestToStringRealFormat(double value, string expected, string format)
    {
        string source = $"""
                         MODULE Test; 
                         VAR
                           r: REAL;
                           s: STRING;
                           b: INTEGER;

                         BEGIN
                             r := {value.ToString(CultureInfo.InvariantCulture)};
                             s := ToString(r, '{format}');
                             WriteString(s)
                         END Test.
                         """;
        var cg = CompileHelper.CompileOberon0Code(source, out string code, output);
        Assert.NotEmpty(code);

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
        Assert.NotNull(assembly);

        using var output1 = new StringWriter();
        Runner.Execute(assembly, output1);
        Assert.Equal(expected, output1.ToString());
    }

    [Theory]
    [InlineData(false, "false", "true", "false")]
    [InlineData(true, "true", "true", "false")]
    [InlineData(false, "0", "1", "0")]
    [InlineData(true, "1", "1", "0")]
    public void TestToStringBooleanFormat(bool value, string expected, string trueVal, string falseVal)
    {
        string source = $"""
                         MODULE Test; 
                         VAR
                           r: BOOLEAN;
                           s: STRING;
                           b: INTEGER;

                         BEGIN
                             r := {value.ToString().ToUpper()};
                             s := ToString(r, '{trueVal}', '{falseVal}');
                             WriteString(s)
                         END Test.
                         """;
        var cg = CompileHelper.CompileOberon0Code(source, out string code, output);
        Assert.NotEmpty(code);

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        byte[] assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
        Assert.NotNull(assembly);

        using var output1 = new StringWriter();
        Runner.Execute(assembly, output1);
        Assert.Equal(expected, output1.ToString());
    }

}