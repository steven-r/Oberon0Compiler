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
            var op = ExpressionRepository.Instance.Get(tokenType, left.TargetType, right.TargetType);
            if (op == null)
                throw new InvalidOperationException(
                    $"Cannot find operation {tokenType:G} ({left.TargetType:G}, {right.TargetType:G})");
            var result = new BinaryExpression { LeftHandSide = left, RightHandSide = right, Operator = tokenType, TargetType = op.Metadata.TargetType};
            result.LeftHandSide = left;
            result.RightHandSide = right;
            result.Operation = op;
            return result;
        }

        internal ArithmeticOperation Operation { get; private set; }

        public override string ToString()
        {
            return $"{Operator:G} ({LeftHandSide.TargetType:G}, {RightHandSide.TargetType:G}) -> {TargetType}";
        }
    }

}