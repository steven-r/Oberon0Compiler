using System;
using JetBrains.Annotations;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    internal abstract class BinaryOperation : IArithmeticOperation
    {
        public Expression Operate(Expression e, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (!(e is BinaryExpression bin)) throw new InvalidCastException("Cannot cast expression to binary expression");
            return BinaryOperate(bin, block, operationParameters);
        }

        protected abstract Expression BinaryOperate(
            BinaryExpression e,
            [UsedImplicitly] Block block,
            IArithmeticOpMetadata operationParameters);
    }
}