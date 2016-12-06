using System;
using System.ComponentModel.Composition;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Operations.Internal;

namespace Oberon0.Compiler.Expressions.Operations
{
    [Export(typeof(IArithmeticOperation))]
    [ArithmeticOperation(TokenType.Mod, BaseType.IntType, BaseType.IntType, BaseType.IntType)]
    [ArithmeticOperation(TokenType.Mod, BaseType.IntType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(TokenType.Mod, BaseType.DecimalType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(TokenType.Mod, BaseType.DecimalType, BaseType.IntType, BaseType.DecimalType)]
    internal class OpModNumber : BinaryOperation
    {
        protected override Expression BinaryOperate(BinaryExpression bin, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (bin.LeftHandSide.IsConst && bin.RightHandSide.IsConst)
            {
                var left = (ConstantExpression)bin.LeftHandSide;
                var right = (ConstantExpression)bin.RightHandSide;
                if (bin.LeftHandSide.TargetType == BaseType.IntType && bin.RightHandSide.TargetType == BaseType.IntType)
                    return new ConstantIntExpression(left.ToInt32() % right.ToInt32());
                double res = left.ToDouble()%right.ToDouble();
                if (double.IsInfinity(res))
                    throw new ArithmeticException("Division by 0");
                return new ConstantDoubleExpression(res);
            }
            return bin; // expression remains the same
        }
    }
}
