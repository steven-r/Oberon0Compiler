﻿#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Operations.Internal;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions
{
    public class BinaryExpression : Expression
    {
        public required Expression LeftHandSide { get; set; }

        public Expression? RightHandSide { get; set; }

        public required ArithmeticOperation Operation { get; set; }

        /// <summary>
        ///     Creates the specified binary expression.
        /// </summary>
        /// <param name="tokenType">Type of the token.</param>
        /// <param name="left">The left hand side.</param>
        /// <param name="right">The right hand side.</param>
        /// <param name="block">The block to handle.</param>
        /// <returns>A binary expression. If no expression is found, <code>null</code> is returned.</returns>
        public static BinaryExpression? Create(int tokenType, Expression left, Expression? right, Block block)
        {
            ArithmeticOperation? op;
            BinaryExpression result;
            if (right == null)
            {
                // unary
                op = ExpressionRepository.Instance.Get(tokenType, left.TargetType.Type, BaseTypes.Any);
                if (op == null)
                {
                    return null;
                }
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
            if (op == null)
            {
                return null;
            }
            result = new BinaryExpression
            {
                LeftHandSide = left,
                RightHandSide = right,
                Operator = tokenType,
                TargetType = block.LookupTypeByBaseType(op.Metadata.ResultType),
                Operation = op
            };
            return result;
        }

        public override string ToString()
        {
            return RightHandSide == null
                // unary
                ? $"{OberonGrammarLexer.DefaultVocabulary.GetSymbolicName(Operator)} ({LeftHandSide.TargetType:G}) -> {TargetType}" 
                : $"{OberonGrammarLexer.DefaultVocabulary.GetSymbolicName(Operator)} ({LeftHandSide.TargetType:G}, {RightHandSide.TargetType:G}) -> {TargetType}";
        }
    }
}
