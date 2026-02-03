#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Constant;

namespace Oberon0.Compiler.Expressions.Functions;

/// <summary>
///     Handle "ToString(INTEGER), ToString(REAL), ToString(BOOLEAN)".
/// </summary>
[InternalFunction]
internal class FunctionStringToString : IInternalFunction
{
    public string[] Prototypes => [ "STRING ToString(INTEGER)", "STRING ToString(REAL)", "STRING ToString(BOOLEAN)" ];

    public Expression Operate(FunctionCallExpression e, Block block)
    {
        if (e.Parameters[0] is ConstantExpression ce)
        {
            return new StringExpression(ce.ToString()!);
        }

        return e;
    }
}