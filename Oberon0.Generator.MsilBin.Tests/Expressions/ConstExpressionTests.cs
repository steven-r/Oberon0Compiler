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

namespace Oberon0.Generator.MsilBin.Tests.Expressions;

public class ConstExpressionTests(ITestOutputHelper output)
{
    [Fact]
    public void TestTrueAndFalseAreNotInGeneratedCode()
    {
        const string source = """
                              MODULE TestTrueAndFalseAreNotInGeneratedCode;

                              BEGIN
                                  WriteLn
                              END TestTrueAndFalseAreNotInGeneratedCode.
                              """;
        CompileHelper.CompileOberon0Code(source, out string code, output);

        Assert.NotEmpty(code);
        Assert.DoesNotContain("private bool TRUE = true;", code);
        Assert.DoesNotContain("private bool FALSE = false;", code);
    }

    [Fact]
    public void TestCorrectEpsilon()
    {
        const string source = """
                              MODULE TestCorrectEpsilon;
                              VAR
                                res: BOOLEAN;
                              BEGIN
                                IF (EPSILON = 4.94065645841247E-324) THEN res := TRUE ELSE res := FALSE END;
                                WriteBool(res);
                                WriteLn
                              END TestCorrectEpsilon.
                              """;
        var cg = CompileHelper.CompileOberon0Code(source, out string code, output);
        Assert.NotEmpty(code);

        var syntaxTree = CSharpSyntaxTree.ParseText(code);

        // Assert.Contains("private double EPSILON = double.Epsilon;", code);

        var assembly = syntaxTree.CompileAndLoadAssembly(cg, true);
        Assert.NotNull(assembly);

        using var output1 = new StringWriter();
        Runner.Execute(assembly, output1);
        Assert.Equal($"{true}\n", output1.ToString().NlFix());
    }
}