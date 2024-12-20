﻿#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;
using Oberon0.Compiler.Expressions.Constant;

namespace Oberon0.Compiler.Solver
{
    /// <summary>
    ///     Solve constant expressions
    /// </summary>
    public static class ConstantSolver
    {
        /// <summary>
        ///     Initialize a constant solver.
        /// </summary>
        /// <param name="expression">
        ///     The expression.
        /// </param>
        /// <param name="block">
        ///     The block.
        /// </param>
        /// <returns>
        ///     The <see cref="Expression" />.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     The expression is missing.
        /// </exception>
        public static Expression Solve(Expression expression, Block block)
        {
            ArgumentNullException.ThrowIfNull(expression);
            ArgumentNullException.ThrowIfNull(block);

            return Calculate(expression, block);
        }

        private static Expression Calculate(Expression expression, Block block)
        {
            switch (expression)
            {
                case VariableReferenceExpression:
                    return expression;

                case BinaryExpression bin: // includes unary operations
                {
                    if (!bin.LeftHandSide.IsConst)
                    {
                        bin.LeftHandSide = Calculate(bin.LeftHandSide, block);
                    }

                    if (bin is { RightHandSide: not null, IsConst: false })
                    {
                        bin.RightHandSide = Calculate(bin.RightHandSide, block);
                    }

                    return bin.Operation.Operation.Operate(bin, block, bin.Operation.Metadata);
                }
                case ConstantExpression c:
                    return c;
                // ignore string expressions
                case StringExpression:
                    return expression;
                case FunctionCallExpression fce:
                    string prototype = FunctionDeclaration.GeneratePrototype(fce.FunctionDeclaration);
                    var internalFunc = ExpressionRepository.Instance.GetInternalFunction(prototype);
                    return internalFunc != null ? internalFunc.Item1.Operate(fce, block, internalFunc.Item2) : expression;
                default:
                    throw new InvalidOperationException(
                        $"Calculate does not support operation on {expression.GetType().Name}");
            }
        }
    }
}
