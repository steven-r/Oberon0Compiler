using System;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Operations.Internal;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions
{
    public class BinaryExpression: Expression
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
        /// <returns>BinaryExpression.</returns>
        /// <exception cref="InvalidOperationException">Operator {tokenType}</exception>
        public static BinaryExpression Create(int tokenType, Expression left, Expression right, Block block)
        {
            ArithmeticOperation op;
            BinaryExpression result;
            if (right == null)
            {
                // unary
                op = ExpressionRepository.Instance.Get(tokenType, left.TargetType.BaseType, BaseType.AnyType);
                result = new UnaryExpression
                {
                    LeftHandSide = left,
                    Operator = tokenType,
                    TargetType = block.LookupTypeByBaseType(op.Metadata.ResultType),
                    Operation = op
                };
                return result;
            }
            op = ExpressionRepository.Instance.Get(tokenType, left.TargetType.BaseType, right.TargetType.BaseType);
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
                return $"{Operator:G} ({LeftHandSide.TargetType:G}) -> {TargetType}";
            return $"{Operator:G} ({LeftHandSide.TargetType:G}, {RightHandSide.TargetType:G}) -> {TargetType}";
        }
    }

}