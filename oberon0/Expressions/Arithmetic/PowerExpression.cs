using System;
using System.ComponentModel.Composition;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions.Arithmetic
{
    [Export(typeof(ICalculatable))]
    class PowerExpression: BinaryExpression, ICalculatable
    {
        public PowerExpression()
        {
            Operator = TokenType.Exp;
        }

        public override Expression Calc(Block block)
        {
            if (this.BinaryConstChecker(block) == null)
            {
                return null;
            }
            // 1. Easy as int
            var lhi = (ConstantExpression)LeftHandSide;
            var rhi = (ConstantExpression)RightHandSide;
            return new ConstantDoubleExpression(Math.Pow(lhi.ToDouble(), rhi.ToDouble()));
        }
    }
}
