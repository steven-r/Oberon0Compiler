#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using JetBrains.Annotations;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Expressions.Functions;
using Oberon0.Compiler.Expressions.Operations.Internal;

namespace Oberon0.Compiler.Expressions.Operations;

/// <summary>
///     Handle "Length(STRING)".
/// </summary>
/// <seealso cref="IArithmeticOperation" />
/// <remarks>This function is some kind of exception as - usually takes one parameter. The second is handled as a dummy</remarks>
[InternalFunction("INTEGER Length(STRING)")]
[UsedImplicitly]
internal class FunctionStringLength: IInternalFunction {

    public Expression Operate(FunctionCallExpression e, Block block, InternalFunctionMetadata functionMetadata)
    {
        if (e.Parameters[0] is StringExpression se)
        {
            return ConstantExpression.Create(se.Value.Length);
        }

        return e;
    }
}