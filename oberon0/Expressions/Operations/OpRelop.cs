using System;
using System.ComponentModel.Composition;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Operations.Internal;

namespace Oberon0.Compiler.Expressions.Operations
{
    [Export(typeof(IArithmeticOperation))]
    [ArithmeticOperation(TokenType.GT, BaseType.IntType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.GT, BaseType.IntType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.GT, BaseType.DecimalType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.GT, BaseType.DecimalType, BaseType.IntType, BaseType.BoolType)]

    [ArithmeticOperation(TokenType.GE, BaseType.IntType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.GE, BaseType.IntType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.GE, BaseType.DecimalType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.GE, BaseType.DecimalType, BaseType.IntType, BaseType.BoolType)]

    [ArithmeticOperation(TokenType.LT, BaseType.IntType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.LT, BaseType.IntType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.LT, BaseType.DecimalType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.LT, BaseType.DecimalType, BaseType.IntType, BaseType.BoolType)]

    [ArithmeticOperation(TokenType.LE, BaseType.IntType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.LE, BaseType.IntType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.LE, BaseType.DecimalType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.LE, BaseType.DecimalType, BaseType.IntType, BaseType.BoolType)]

    [ArithmeticOperation(TokenType.NotEquals, BaseType.IntType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.NotEquals, BaseType.IntType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.NotEquals, BaseType.DecimalType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.NotEquals, BaseType.DecimalType, BaseType.IntType, BaseType.BoolType)]

    [ArithmeticOperation(TokenType.Equals, BaseType.IntType, BaseType.IntType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.Equals, BaseType.IntType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.Equals, BaseType.DecimalType, BaseType.DecimalType, BaseType.BoolType)]
    [ArithmeticOperation(TokenType.Equals, BaseType.DecimalType, BaseType.IntType, BaseType.BoolType)]
    internal class OpRelOp : BinaryOperation
    {
        protected override Expression BinaryOperate(BinaryExpression bin, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (bin.LeftHandSide.IsConst && bin.RightHandSide.IsConst)
            {
                var left = (ConstantExpression)bin.LeftHandSide;
                var right = (ConstantExpression)bin.RightHandSide;
                bool res;
                switch (operationParameters.Operation)
                {
                    case TokenType.GT:
                        res = left.ToDouble() > right.ToDouble();
                        break;
                    case TokenType.GE:
                        res = left.ToDouble() >= right.ToDouble();
                        break;
                    case TokenType.LT:
                        res = left.ToDouble() < right.ToDouble();
                        break;
                    case TokenType.LE:
                        res = left.ToDouble() <= right.ToDouble();
                        break;
                    case TokenType.NotEquals:
                        res = Math.Abs(left.ToDouble() - right.ToDouble()) > double.Epsilon;
                        break;
                    case TokenType.Equals:
                        res = Math.Abs(left.ToDouble() - right.ToDouble()) <= double.Epsilon;
                        break;
                    default:
                        throw new InvalidOperationException("Unknown comparison");
                }
                return new ConstantBoolExpression(res);
            }
            return bin; // expression remains the same
        }
    }
}
