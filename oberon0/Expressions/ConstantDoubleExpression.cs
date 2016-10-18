using System.ComponentModel.Composition;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions
{
    [Export(typeof(ICalculatable))]
    class ConstantDoubleExpression : ConstantExpression, ICalculatable
    {
        public double Value { get; set; }

        public ConstantDoubleExpression()
            : base(BaseType.DecimalType)
        {
        }

        public ConstantDoubleExpression(double value)
            : base(BaseType.DecimalType)
        {
            Value = value;
        }

        public override Expression Calc(Block block)
        {
            return this;
        }

    }
}