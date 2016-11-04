using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions
{
    public class ConstantDoubleExpression : ConstantExpression
    {
        public ConstantDoubleExpression(double value)
            : base(BaseType.DecimalType, value)
        {
        }

        public override string ToString()
        {
            return ((double)Value).ToString("G");
        }
    }
}