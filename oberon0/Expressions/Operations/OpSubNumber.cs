using System.ComponentModel.Composition;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Operations.Internal;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations
{
    [Export(typeof(IArithmeticOperation))]
    [ArithmeticOperation(OberonGrammarLexer.MINUS, BaseType.IntType, BaseType.IntType, BaseType.IntType)]
    [ArithmeticOperation(OberonGrammarLexer.MINUS, BaseType.IntType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(OberonGrammarLexer.MINUS, BaseType.DecimalType, BaseType.DecimalType, BaseType.DecimalType)]
    [ArithmeticOperation(OberonGrammarLexer.MINUS, BaseType.DecimalType, BaseType.IntType, BaseType.DecimalType)]
    internal class OpSubNumber : BinaryOperation
    {
        protected override Expression BinaryOperate(BinaryExpression bin, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (bin.LeftHandSide.IsConst && bin.RightHandSide.IsConst)
            {
                var left = (ConstantExpression)bin.LeftHandSide;
                var right = (ConstantExpression)bin.RightHandSide;
                if (bin.LeftHandSide.TargetType.BaseType == BaseType.IntType && bin.RightHandSide.TargetType.BaseType == BaseType.IntType)
                    return new ConstantIntExpression(left.ToInt32() - right.ToInt32());
                return new ConstantDoubleExpression(left.ToDouble() - right.ToDouble());
            }
            return bin; // expression remains the same
        }
    }
}