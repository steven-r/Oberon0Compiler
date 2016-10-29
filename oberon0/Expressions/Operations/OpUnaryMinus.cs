using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loyc.Geometry;
using Oberon0.Compiler.Definitions;

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
    public class OpUnaryMinus : IArithmeticOperation
    {
        public Expression Operate(Expression e, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            var bin = e as BinaryExpression;
            if (bin == null) throw new InvalidCastException("Cannot cast expression to binary expression");
            if (bin.LeftHandSide.IsConst)
            {
                if (bin.LeftHandSide.TargetType == BaseType.IntType)
                {
                    ConstantIntExpression left = (ConstantIntExpression)bin.LeftHandSide;
                    left.Value = -(int)left.Value;
                    return left;
                }
                else if (bin.LeftHandSide.TargetType == BaseType.DecimalType)
                {
                    ConstantDoubleExpression left = (ConstantDoubleExpression)bin.LeftHandSide;
                    left.Value = -(int)left.Value;
                    return left;
                }
            }
            return e; // expression remains the same
        }
    }
}
