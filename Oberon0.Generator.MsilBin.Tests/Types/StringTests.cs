#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

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
}