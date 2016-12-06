using System.ComponentModel.Composition;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Operations.Internal;

namespace Oberon0.Compiler.Expressions.Operations
{
    /// <summary>
    /// Handle "~".
    /// </summary>
    /// <seealso cref="IArithmeticOperation" />
    /// <remarks>This function is some kind of exception as ~ usually takes one parameter. The second is handled as a dummy</remarks>
    [Export(typeof(IArithmeticOperation))]
    [ArithmeticOperation(TokenType.Not, BaseType.BoolType, BaseType.AnyType, BaseType.BoolType)]
    internal class OpNotBool : BinaryOperation
    {
        protected override Expression BinaryOperate(BinaryExpression bin, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (bin.LeftHandSide.IsConst)
            {
                var left = (ConstantBoolExpression)bin.LeftHandSide;
                left.Value = !(bool)left.Value;
                return left;
            }
            return bin; // expression remains the same
        }
    }
}
