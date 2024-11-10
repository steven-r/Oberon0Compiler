#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Linq;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Exceptions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Expressions.Operations.Internal;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations
{
    [ArithmeticOperation(OberonGrammarLexer.STAR, BaseTypes.String, BaseTypes.Int, BaseTypes.String)]
    // ReSharper disable once UnusedMember.Global
    internal class OpMulString : BinaryOperation
    {
        protected override Expression BinaryOperate(
            BinaryExpression bin,
            Block block,
            IArithmeticOpMetadata operationParameters)
        {
            if (!bin.LeftHandSide.IsConst || !bin.RightHandSide!.IsConst)
            {
                return bin; // expression remains the same
            }

            var strVal = bin.LeftHandSide as StringExpression ??
                throw new InternalCompilerException("OpMulString with wrong parameters (string)");
            var multiplier = bin.RightHandSide as ConstantIntExpression ??
                throw new InternalCompilerException("OpMulString with wrong parameters (multiplier)");

            return new StringExpression(string.Concat(Enumerable.Repeat(strVal.Value, multiplier.ToInt32())));

        }
    }
}
