#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Test.Support;
using Xunit;
using Xunit.Abstractions;

namespace Oberon0.Compiler.Tests;

public class CompilerErrorTests(ITestOutputHelper outputHelper)
{

    [Fact]
    public void InvalidOperationTestRelation()
    {
        TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              b: INTEGER;
              r: STRING;
            BEGIN
              b := 5;
              r := b >= FALSE
            END test.
            """,
            outputHelper,
            "Left and right expression are not compatible with >=",
            "Cannot parse right side of assignment");
    }

    [Fact]
    public void InvalidOperationTestAdd()
    {
        TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              b: INTEGER;
              r: STRING;
            BEGIN
              b := 5;
              r := b + FALSE
            END test.
            """,
            outputHelper,
            "Left and right expression are not compatible with +",
            "Cannot parse right side of assignment");
    }

    [Fact]
    public void TestStringMultStringOnRightHandSideFail()
    {
        TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              b: INTEGER;
              r: STRING;
            BEGIN
              b := 5;
              r := b * 'Hello String';
            END test.
            """,
            outputHelper,
            "Left and right expression are not compatible with *",
            "Cannot parse right side of assignment");
    }

    [Fact]
    public void InvalidOperationTestNot()
    {
        TestHelper.CompileString(
            """

            MODULE test; 
            VAR 
              b: INTEGER;
              r: BOOLEAN;
            BEGIN
              b := 1;
              r := ~b
            END test.
            """,
            outputHelper,
            "Expression is not compatible with ~",
            "Cannot parse right side of assignment");
    }

}
