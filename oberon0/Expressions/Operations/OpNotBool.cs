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
    [ArithmeticOperation(TokenType.Not, BaseType.BoolType, BaseType.AnyType, BaseType.BoolType)]
    public class OpNotBool : IArithmeticOperation
    {
        public Expression Operate(Expression e, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            var bin = e as BinaryExpression;
            if (bin == null) throw new InvalidCastException("Cannot cast expression to binary expression");
            if (bin.LeftHandSide.IsConst)
            {
                var left = (ConstantBoolExpression)bin.LeftHandSide;
                left.Value = !(bool)left.Value;
                return left;
            }
            return e; // expression remains the same
        }
    }
}
