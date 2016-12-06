using System;
using System.ComponentModel.Composition;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Operations.Internal;

namespace Oberon0.Compiler.Expressions.Operations
{
    [Export(typeof(IArithmeticOperation))]
    [ArithmeticOperation(TokenType.Mul, BaseType.IntType, BaseType.IntType, BaseType.IntType)]
    [ArithmeticOperation(TokenType.Mul, BaseType.IntType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(TokenType.Mul, BaseType.DecimalType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(TokenType.Mul, BaseType.DecimalType, BaseType.IntType, BaseType.DecimalType)]
    internal class OpMulNumber : BinaryOperation
    {
        protected override Expression BinaryOperate(BinaryExpression bin, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (bin.LeftHandSide.IsConst && bin.RightHandSide.IsConst)
            {
                var left = (ConstantExpression)bin.LeftHandSide;
                var right = (ConstantExpression)bin.RightHandSide;
                if (bin.LeftHandSide.TargetType == BaseType.IntType && bin.RightHandSide.TargetType == BaseType.IntType)
                    return new ConstantIntExpression(left.ToInt32() * right.ToInt32());
                return new ConstantDoubleExpression(left.ToDouble() * right.ToDouble());
            }
            return bin; // expression remains the same
        }
    }
}
