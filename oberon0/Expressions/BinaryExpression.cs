using System;
using System.Linq;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions
{
    class BinaryExpression : OperatorExpression
    {
        public Expression LeftHandSide { get; set; }
        public Expression RightHandSide { get; set; }

        public static BinaryExpression Create(TokenType tokenType, Expression left, Expression right)
        {
            var result = Create(tokenType) as BinaryExpression;
            if (result == null) result = new BinaryExpression {Operator = tokenType};
            result.LeftHandSide = left;
            result.RightHandSide = right;
            return result;
        }

        /// <summary>
        /// Cannot calculate.
        /// </summary>
        /// <returns>Expression.</returns>
        public override Expression Calc(Block block)
        {
            TargetType = BaseType.ErrorType;
            return null;
        }
    }

    public abstract class OperatorExpression: Expression, ICalculatable
    {
        public TokenType Operator { get; set; }
        public abstract Expression Calc(Block block);

        private static OperatorExpression CreateInstance(ICalculatable instance)
        {
            return (OperatorExpression) Activator.CreateInstance(instance.GetType());
        }

        public static Expression Create(TokenType oper)
        {
            var op = CompilerParser.CalculationRepository.ClassRepository.FirstOrDefault(x => x.Operator == oper);
            if (op == null) throw new ArgumentOutOfRangeException(nameof(oper), oper, "Value not found");

            var result = CreateInstance(op);
            result.Operator = oper;
            return result;
        }
    }
}