using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Solver;

namespace Oberon0.Compiler.Expressions
{
    static class ExpressionHelper
    {
        internal static OperatorExpression BinaryConstChecker(this BinaryExpression helper, Block block)
        {
            if (helper == null) return null;
            var l = helper.LeftHandSide as ICalculatable;
            var r = helper.RightHandSide as ICalculatable;
            if (l == null || r == null) return null;
            Expression lhs = l.Calc(block);
            Expression rhs = r.Calc(block);
            if (lhs != null)
            {
                helper.LeftHandSide = lhs; // optimized
            }
            if (rhs != null)
            {
                helper.RightHandSide = rhs;
            }

            // cannot create constant calculation
            if (lhs == null || rhs == null) return null;
            return helper;
        }

        internal static OperatorExpression UnaryConstChecker(this UnaryExpression helper, Block block)
        {
            var l = helper?.Operand as ICalculatable;
            if (l == null) return null;
            Expression lhs = l.Calc(block);
            if (lhs != null)
            {
                helper.Operand = lhs; // optimized
            }
            // cannot create constant calculation
            if (lhs == null) return null;
            return helper;
        }

    }
}
