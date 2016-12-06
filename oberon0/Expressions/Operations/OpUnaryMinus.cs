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
    [ArithmeticOperation(TokenType.Unary, BaseType.IntType, BaseType.AnyType, BaseType.IntType)]
    [ArithmeticOperation(TokenType.Unary, BaseType.DecimalType, BaseType.AnyType, BaseType.DecimalType)]
    internal class OpUnaryMinus : BinaryOperation
    {
        protected override Expression BinaryOperate(BinaryExpression e, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (e.LeftHandSide.IsConst)
                if (e.LeftHandSide.TargetType == BaseType.IntType)
                {
                    ConstantIntExpression left = (ConstantIntExpression)e.LeftHandSide;
                    left.Value = -(int)left.Value;
                    return left;
                }
                else if (e.LeftHandSide.TargetType == BaseType.DecimalType)
                {
                    ConstantDoubleExpression left = (ConstantDoubleExpression)e.LeftHandSide;
                    left.Value = -(int)left.Value;
                    return left;
                }
            return e; // expression remains the same
        }
    }
}
