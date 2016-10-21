using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions
{
    public class ConstantBoolExpression : ConstantExpression
    {
        public bool Value { get; set; }

        public ConstantBoolExpression(bool value)
            : base(BaseType.BoolType)
        {
            Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}