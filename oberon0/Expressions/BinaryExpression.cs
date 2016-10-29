using System;
using System.Linq;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Operations;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions
{
    public class BinaryExpression: Expression
    {
        public Expression LeftHandSide { get; set; }
        public Expression RightHandSide { get; set; }

        /// <summary>
        /// Creates the specified binary expression.
        /// </summary>
        /// <param name="tokenType">Type of the token.</param>
        /// <param name="left">The left hand side.</param>
        /// <param name="right">The right hand side.</param>
        /// <returns>BinaryExpression.</returns>
        /// <exception cref="InvalidOperationException">Operator {tokenType}</exception>
        public static BinaryExpression Create(TokenType tokenType, Expression left, Expression right)
        {
            ArithmeticOperation op;
            BinaryExpression result;
            if (right == null)
            {
                // unary
                op = ExpressionRepository.Instance.Get(tokenType, left.TargetType, BaseType.AnyType);
                result = new UnaryExpression
                {
                    LeftHandSide = left,
                    Operator = tokenType,
                    TargetType = op.Metadata.TargetType
                };
                result.LeftHandSide = left;
                result.Operation = op;
                return result;
            }
            op = ExpressionRepository.Instance.Get(tokenType, left.TargetType, right.TargetType);
            result = new BinaryExpression { LeftHandSide = left, RightHandSide = right, Operator = tokenType, TargetType = op.Metadata.TargetType };
            result.LeftHandSide = left;
            result.RightHandSide = right;
            result.Operation = op;
            return result;
        }

        internal ArithmeticOperation Operation { get; private set; }

        public override string ToString()
        {
            if (RightHandSide == null)
            {
                return $"{Operator:G} ({LeftHandSide.TargetType:G}) -> {TargetType}";
            }
            return $"{Operator:G} ({LeftHandSide.TargetType:G}, {RightHandSide.TargetType:G}) -> {TargetType}";
        }
    }

}