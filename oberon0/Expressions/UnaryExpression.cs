using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions
{
    class UnaryExpression : OperatorExpression 
    {
        public override Expression Calc(Block block)
        {
            TargetType = BaseType.ErrorType;
            return null;
        }

        public Expression Operand { get; set; }
    }
}