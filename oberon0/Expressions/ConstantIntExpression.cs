using System.ComponentModel.Composition;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions
{
    [Export(typeof(ICalculatable))]
    class ConstantIntExpression : ConstantExpression, ICalculatable
    {
        public int Value { get; set; }

        public ConstantIntExpression()
            : base(BaseType.IntType)
        {
        }

        public ConstantIntExpression(int value)
            : base(BaseType.IntType)
        {
            Value = value;
        }

        public override Expression Calc(Block block)
        {
            return this;
        }
    }
}