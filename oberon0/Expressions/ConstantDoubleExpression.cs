using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions
{
    public class ConstantDoubleExpression : ConstantExpression
    {
        public double Value { get; }

        public ConstantDoubleExpression(double value)
            : base(BaseType.DecimalType)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString("G");
        }
    }
}