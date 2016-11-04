using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions
{
    public class ConstantBoolExpression : ConstantExpression
    {
        public ConstantBoolExpression(bool value)
            : base(BaseType.BoolType, value)
        {
        }
    }
}