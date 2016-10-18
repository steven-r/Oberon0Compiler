using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions;

namespace Oberon0.Compiler.Solver
{
    /// <summary>
    /// Solve constant expressions
    /// </summary>
    static class ConstantSolver
    {
        public static Expression Solve(Expression expression, Block block)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));

            if (!CheckConst(expression, block)) return null;

            var ex = expression as ICalculatable;
            Debug.Assert(ex != null, "Shouldn't be here");
            var result = ex.Calc(block);
            if (!(result is ConstantExpression)) return null;
            return result;
        }

        private static bool CheckConst(Expression expression, Block block)
        {
            while (true)
            {
                var constExpression = expression as ConstantExpression;
                if (constExpression != null) return true;
                var binaryExpression = expression as BinaryExpression;
                if (binaryExpression != null)
                    return CheckConst(binaryExpression.LeftHandSide, block) &&
                           CheckConst(binaryExpression.RightHandSide, block);
                var unaryExpression = expression as UnaryExpression;
                if (unaryExpression != null)
                {
                    expression = unaryExpression.Operand;
                    continue;
                }
                var varExpression = expression as VariableReferenceExpression;
                if (varExpression != null)
                {
                    expression = ((ConstDeclaration) varExpression.Declaration).Value;
                    continue;
                }

                return false; // assume that all other are wrong
            }
        }
    }
}
