#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ConstantSolver.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/ConstantSolver.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Solver
{
    using System;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions;
    using Oberon0.Compiler.Expressions.Constant;

    /// <summary>
    /// Solve constant expressions
    /// </summary>
    internal static class ConstantSolver
    {
        /// <summary>
        /// Initialize a constant solver.
        /// </summary>
        /// <param name="expression">
        /// The expression.
        /// </param>
        /// <param name="block">
        /// The block.
        /// </param>
        /// <returns>
        /// The <see cref="Expression"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">The expression is missing.
        /// </exception>
        public static Expression Solve(Expression expression, Block block)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            return Calculate(expression, block);
        }

        private static Expression Calculate(Expression expression, Block block)
        {
            if (expression is VariableReferenceExpression) return expression;

            if (expression is BinaryExpression bin)
            {
                if (!bin.LeftHandSide.IsConst)
                    bin.LeftHandSide = Calculate(bin.LeftHandSide, block);
                if (bin.RightHandSide != null && !bin.IsConst)
                    bin.RightHandSide = Calculate(bin.RightHandSide, block);
                return bin.Operation.Operation.Operate(bin, block, bin.Operation.Metadata);
            }

            if (expression is ConstantExpression c) return c;

            // ignore string expressions
            if (expression is StringExpression) return expression;
            if (expression is FunctionCallExpression) return expression;
            throw new InvalidOperationException($"Calculate does not support operation on {expression.GetType().Name}");
        }
    }
}