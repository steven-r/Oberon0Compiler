#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Antlr4.Runtime;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Operations.Internal;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions
{
    public class BinaryExpression : Expression
    {
        public Expression LeftHandSide { get; set; }

        public Expression RightHandSide { get; set; }

        internal ArithmeticOperation Operation { get; private set; }

#pragma warning disable CS3001 // Argument type is not CLS-compliant
        /// <summary>
        /// Creates the specified binary expression.
        /// </summary>
        /// <param name="tokenType">Type of the token.</param>
        /// <param name="left">The left hand side.</param>
        /// <param name="right">The right hand side.</param>
        /// <param name="block">The block to handle.</param>
        /// <param name="token">The source code token</param>
        /// <returns>A binary expression.</returns>
        public static BinaryExpression Create(int tokenType, Expression left, Expression right, Block block, IToken token)
#pragma warning restore CS3001 // Argument type is not CLS-compliant
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
                        Operation = op,
                        Token = token
                    };
                return result;
            }

            op = ExpressionRepository.Instance.Get(tokenType, left.TargetType.Type, right.TargetType.Type);
            result = new BinaryExpression
                {
                    LeftHandSide = left,
                    RightHandSide = right,
                    Operator = tokenType,
                    TargetType = block.LookupTypeByBaseType(op.Metadata.ResultType),
                    Operation = op,
                    Token = token
            };
            return result;
        }

        public override string ToString()
        {
            if (this.RightHandSide == null)
            // unary
                return
                    $"{OberonGrammarLexer.DefaultVocabulary.GetSymbolicName(this.Operator)} ({this.LeftHandSide.TargetType:G}) -> {this.TargetType}";

            return
                $"{OberonGrammarLexer.DefaultVocabulary.GetSymbolicName(this.Operator)} ({this.LeftHandSide.TargetType:G}, {this.RightHandSide.TargetType:G}) -> {this.TargetType}";
        }
    }
}