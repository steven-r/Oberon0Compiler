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

public class IntegerTests(ITestOutputHelper output)
{
    [Fact]
    public void IntIssue_INTEGER_MinValue_is_REAL()
    {
        var m = TestHelper.CompileString(
            $"""
                     MODULE Test; 
                     VAR
                       val: INTEGER;
                     BEGIN
                       val := {int.MinValue};
                     END Test.
             """, output);

        var s = m.Block.Statements[0];
        var a = Assert.IsAssignableFrom<AssignmentStatement>(s);
        Assert.IsAssignableFrom<ConstantIntExpression>(a.Expression);
    }
}