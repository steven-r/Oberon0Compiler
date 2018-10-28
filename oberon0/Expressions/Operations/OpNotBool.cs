#region copyright

// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OpNotBool.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/OpNotBool.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------

#endregion

namespace Oberon0.Compiler.Expressions.Operations
{
    using JetBrains.Annotations;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Expressions.Constant;
    using Oberon0.Compiler.Expressions.Operations.Internal;
    using Oberon0.Compiler.Types;

    /// <summary>
    /// Handle "~".
    /// </summary>
    /// <seealso cref="IArithmeticOperation" />
    /// <remarks>This function is some kind of exception as ~ usually takes one parameter. The second is handled as a dummy</remarks>
    [ArithmeticOperation(OberonGrammarLexer.NOT, BaseTypes.BoolType, BaseTypes.AnyType, BaseTypes.BoolType)]
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
                var left = (ConstantBoolExpression)bin.LeftHandSide;
                left.Value = !(bool)left.Value;
                return left;
            }

            return bin; // expression remains the same
        }
    }
}