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
    [ArithmeticOperation(OberonGrammarLexer.OR, BaseTypes.Bool, BaseTypes.Bool, BaseTypes.Bool)]
    [ArithmeticOperation(OberonGrammarLexer.AND, BaseTypes.Bool, BaseTypes.Bool, BaseTypes.Bool)]
    [UsedImplicitly]
    internal class OpRelOp2 : BinaryOperation
    {
        protected override Expression BinaryOperate(BinaryExpression bin, Block block, IArithmeticOpMetadata operationParameters)
        {
            if (bin.LeftHandSide.IsConst && bin.RightHandSide.IsConst)
            {
                var left = (ConstantExpression)bin.LeftHandSide;
                var right = (ConstantExpression)bin.RightHandSide;
                bool res = false;
                switch (operationParameters.Operation)
                {
                    case OberonGrammarLexer.AND:
                        res = left.ToBool() && right.ToBool();
                        break;

                    case OberonGrammarLexer.OR:
                        res = left.ToBool() || right.ToBool();
                        break;
                }

                return new ConstantBoolExpression(res);
            }

            return bin; // expression remains the same
        }
    }
}