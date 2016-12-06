using System;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    internal abstract class BinaryOperation: IArithmeticOperation
    {
        public Expression Operate(Expression e, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            var bin = e as BinaryExpression;
            if (bin == null) throw new InvalidCastException("Cannot cast expression to binary expression");
            return BinaryOperate(bin, block, operationParameters);
        }

        protected abstract Expression BinaryOperate(BinaryExpression e, Block block,
            IArithmeticOpMetadata operationParameters);
    }
}