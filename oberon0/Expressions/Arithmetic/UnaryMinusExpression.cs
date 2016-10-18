using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions.Arithmetic
{
    class UnaryMinusExpression: UnaryExpression
    {
        public UnaryMinusExpression()
        {
            Operator = TokenType.Unary;
        }

        public override Expression Calc(Block block)
        {
            if (this.UnaryConstChecker(block) == null)
            {
                return null;
            }
            // 1. Easy as int
            var val = (ConstantExpression)Operand;
            if (val.BaseType == BaseType.IntType)
            {
                return new ConstantIntExpression(-val.ToInt32());
            }
            // at least one of them is double
            return new ConstantDoubleExpression(-val.ToDouble());
        }
    }
}
