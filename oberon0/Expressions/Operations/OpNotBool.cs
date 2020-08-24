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
    /// <summary>
    ///     Handle "~".
    /// </summary>
    /// <seealso cref="IArithmeticOperation" />
    /// <remarks>This function is some kind of exception as ~ usually takes one parameter. The second is handled as a dummy</remarks>
    [ArithmeticOperation(OberonGrammarLexer.NOT, BaseTypes.Bool, BaseTypes.Any, BaseTypes.Bool)]
    [UsedImplicitly]
    internal class OpNotBool : BinaryOperation
    {
        protected override Expression BinaryOperate(
            BinaryExpression bin,
            Block block,
            IArithmeticOpMetadata operationParameters)
        {
            if (bin.LeftHandSide.IsConst)
            {
                var left = (ConstantBoolExpression) bin.LeftHandSide;
                left.Value = !(bool) left.Value;
                return left;
            }

            return bin; // expression remains the same
        }
    }
}
