#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Expressions.Operations;
using Oberon0.Compiler.Solver;
using Xunit;

namespace Oberon0.Compiler.Tests.Expressions
{
    public class AdditionalTests
    {
        [Fact]
        public void ConstantSolverNullInputExceptionForExpression()
        {
            Assert.Throws<ArgumentNullException>(
                () => ConstantSolver.Solve(null!, new Block(null, new Module(null))));
        }

        [Fact]
        public void BinaryOperatorCheckParametersNull()
        {
            var b = new Module(null).Block;
            var e = Assert.Throws<ArgumentNullException>(
                () => new OpRelOp().Operate(null!, b, null!));
            Assert.Equal("e", e.ParamName);
            var e1 = Assert.Throws<ArgumentNullException>(
                () => new OpRelOp().Operate(
                    BinaryExpression.Create(OberonGrammarLexer.PLUS, ConstantExpression.Create(0),
                        ConstantExpression.Create(1), b), null!, null!));
            Assert.Equal("block", e1.ParamName);
        }
    }
}
