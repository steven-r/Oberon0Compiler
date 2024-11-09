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
    [ArithmeticOperation(OberonGrammarLexer.PLUS, BaseTypes.String, BaseTypes.String, BaseTypes.String)]
    [UsedImplicitly]
    internal class OpAddString : BinaryOperation
    {
        protected override Expression BinaryOperate(
            BinaryExpression bin,
            Block block,
            IArithmeticOpMetadata operationParameters)
        {
            if (bin.LeftHandSide.IsConst && bin.RightHandSide!.IsConst)
            {
                var left = (StringExpression) bin.LeftHandSide;
                var right = (StringExpression) bin.RightHandSide;

                return new StringExpression(left.Value + right.Value);
            }

            return bin; // expression remains the same
        }
    }
}
