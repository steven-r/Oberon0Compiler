#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BinaryExpression.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/BinaryExpression.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions
{
    using System;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions.Operations.Internal;
    using Oberon0.Compiler.Types;

    public class BinaryExpression : Expression
    {
        public Expression LeftHandSide { get; set; }

        public Expression RightHandSide { get; set; }

        internal ArithmeticOperation Operation { get; private set; }

        /// <summary>
        /// Creates the specified binary expression.
        /// </summary>
        /// <param name="tokenType">Type of the token.</param>
        /// <param name="left">The left hand side.</param>
        /// <param name="right">The right hand side.</param>
        /// <param name="block">The block to handle.</param>
        /// <returns>A binary expression.</returns>
        public static BinaryExpression Create(int tokenType, Expression left, Expression right, Block block)
        {
            ArithmeticOperation op;
            BinaryExpression result;
            if (right == null)
            {
                // unary
                op = ExpressionRepository.Instance.Get(tokenType, left.TargetType.Type, BaseTypes.Any);
                result = new UnaryExpression
                             {
                                 LeftHandSide = left,
                                 Operator = tokenType,
                                 TargetType = block.LookupTypeByBaseType(op.Metadata.ResultType),
                                 Operation = op
                             };
                return result;
            }

            op = ExpressionRepository.Instance.Get(tokenType, left.TargetType.Type, right.TargetType.Type);
            result = new BinaryExpression
                         {
                             LeftHandSide = left,
                             RightHandSide = right,
                             Operator = tokenType,
                             TargetType = block.LookupTypeByBaseType(op.Metadata.ResultType)
                         };
            result.LeftHandSide = left;
            result.RightHandSide = right;
            result.Operation = op;
            return result;
        }

        public override string ToString()
        {
            if (RightHandSide == null)
            {
                // unary
                return $"{Operator:G} ({LeftHandSide.TargetType:G}) -> {TargetType}";
            }

            return $"{Operator:G} ({LeftHandSide.TargetType:G}, {RightHandSide.TargetType:G}) -> {TargetType}";
        }
    }
}