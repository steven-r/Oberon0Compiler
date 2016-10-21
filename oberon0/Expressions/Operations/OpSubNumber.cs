using System;
using System.ComponentModel.Composition;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions.Operations
{
    [Export(typeof(IArithmeticOperation))]
    [ArithmeticOperation(TokenType.Sub, BaseType.IntType, BaseType.IntType, BaseType.IntType)]
    [ArithmeticOperation(TokenType.Sub, BaseType.IntType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(TokenType.Sub, BaseType.DecimalType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(TokenType.Sub, BaseType.DecimalType, BaseType.IntType, BaseType.DecimalType)]
    public class OpSubNumber : IArithmeticOperation
    {
        public Expression Operate(Expression e, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            var bin = e as BinaryExpression;
            if (bin == null) throw new InvalidCastException("Cannot cast expression to binary expression");
            if (bin.LeftHandSide.IsConst && bin.RightHandSide.IsConst)
            {
                var left = (ConstantExpression)bin.LeftHandSide;
                var right = (ConstantExpression)bin.RightHandSide;
                if (bin.LeftHandSide.TargetType == BaseType.IntType && bin.RightHandSide.TargetType == BaseType.IntType)
                {
                    return new ConstantIntExpression(left.ToInt32() - right.ToInt32());
                }
                return new ConstantDoubleExpression(left.ToDouble() - right.ToDouble());
            }
            return e; // expression remains the same
        }
    }
}