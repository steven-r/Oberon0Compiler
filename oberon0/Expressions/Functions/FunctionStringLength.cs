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
///     Handle "Length(STRING)".
/// </summary>
[InternalFunction]
internal class FunctionStringLength : IInternalFunction
{
    public string[] Prototypes => [ "INTEGER Length(STRING)" ];

    public Expression Operate(FunctionCallExpression e, Block block)
    {
        if (e.Parameters[0] is StringExpression se)
        {
            return ConstantExpression.Create(se.Value.Length);
        }

        return e;
    }
}
