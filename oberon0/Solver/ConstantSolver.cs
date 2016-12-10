using System;
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

            return Calculate(expression, block);
        }

        private static Expression Calculate(Expression expression, Block block)
        {
            if (expression is VariableReferenceExpression) return expression;
            var bin = expression as BinaryExpression;
            if (bin != null)
            {
                if (!bin.LeftHandSide.IsConst)
                    bin.LeftHandSide = Calculate(bin.LeftHandSide, block);
                if (!bin.RightHandSide?.IsConst ?? false)
                    bin.RightHandSide = Calculate(bin.RightHandSide, block);
                return bin.Operation.Operation.Operate(bin, block, bin.Operation.Metadata);
            }
            var c = expression as ConstantExpression;
            if (c != null) return c;
            // ignore string expressions
            if (expression is StringExpression) return expression;
            throw new InvalidOperationException($"Calculate does not support operation on {expression.GetType()}");
        }
    }
}
