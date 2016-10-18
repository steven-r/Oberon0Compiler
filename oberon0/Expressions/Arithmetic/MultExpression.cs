using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions.Arithmetic
{
    // ReSharper disable once UnusedMember.Global
    [Export(typeof(ICalculatable))]
    class MultExpression : BinaryExpression, ICalculatable
    {

        public MultExpression()
        {
            Operator = TokenType.Mul;
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
            if (rhi.BaseType == lhi.BaseType && lhi.BaseType == BaseType.IntType)
            {
                return new ConstantIntExpression(lhi.ToInt32() * rhi.ToInt32());
            }
            // at least one of them is double
            return new ConstantDoubleExpression(lhi.ToDouble() * rhi.ToDouble());
        }
    }
}
