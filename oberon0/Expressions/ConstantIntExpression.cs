using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions
{
    public class ConstantIntExpression : ConstantExpression
    {
        public ConstantIntExpression(int value)
            : base(BaseType.IntType, value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return ((int)Value).ToString("G");
        }
    }
}