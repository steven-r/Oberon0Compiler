using System;
using System.ComponentModel.Composition;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Operations.Internal;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations
{
    [Export(typeof(IArithmeticOperation))]
    [ArithmeticOperation(OberonGrammarLexer.DIV, BaseType.IntType, BaseType.IntType, BaseType.IntType)]
    [ArithmeticOperation(OberonGrammarLexer.DIV, BaseType.IntType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(OberonGrammarLexer.DIV, BaseType.DecimalType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(OberonGrammarLexer.DIV, BaseType.DecimalType, BaseType.IntType, BaseType.DecimalType)]
    internal class OpDivNumber : BinaryOperation
    {
        protected override Expression BinaryOperate(BinaryExpression bin, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (bin.LeftHandSide.IsConst && bin.RightHandSide.IsConst)
            {
                var left = (ConstantExpression)bin.LeftHandSide;
                var right = (ConstantExpression)bin.RightHandSide;
                if (bin.LeftHandSide.TargetType.BaseType == BaseType.IntType && bin.RightHandSide.TargetType.BaseType == BaseType.IntType)
                    return new ConstantIntExpression(left.ToInt32() / right.ToInt32());
                double res = left.ToDouble()/right.ToDouble();
                if (double.IsInfinity(res))
                    throw new ArithmeticException("Division by 0");
                return new ConstantDoubleExpression(res);
            }
            return bin; // expression remains the same
        }
    }
}
