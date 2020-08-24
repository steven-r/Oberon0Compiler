#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using JetBrains.Annotations;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Expressions.Operations.Internal;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations
{
    [ArithmeticOperation(OberonGrammarLexer.STAR, BaseTypes.Int, BaseTypes.Int, BaseTypes.Int)]
    [ArithmeticOperation(OberonGrammarLexer.STAR, BaseTypes.Int, BaseTypes.Real, BaseTypes.Real)]
    [ArithmeticOperation(OberonGrammarLexer.STAR, BaseTypes.Real, BaseTypes.Real, BaseTypes.Real)]
    [ArithmeticOperation(OberonGrammarLexer.STAR, BaseTypes.Real, BaseTypes.Int, BaseTypes.Real)]
    [UsedImplicitly]
    internal class OpMulNumber : BinaryOperation
    {
        protected override Expression BinaryOperate(
            BinaryExpression bin,
            Block block,
            IArithmeticOpMetadata operationParameters)
        {
            if (bin.LeftHandSide.IsConst && bin.RightHandSide.IsConst)
            {
                var left = (ConstantExpression) bin.LeftHandSide;
                var right = (ConstantExpression) bin.RightHandSide;
                if (bin.LeftHandSide.TargetType.Type == BaseTypes.Int
                 && bin.RightHandSide.TargetType.Type == BaseTypes.Int)
                {
                    return new ConstantIntExpression(left.ToInt32() * right.ToInt32());
                }

                return new ConstantDoubleExpression(left.ToDouble() * right.ToDouble());
            }

            return bin; // expression remains the same
        }
    }
}
