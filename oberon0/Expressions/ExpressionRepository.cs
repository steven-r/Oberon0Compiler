#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Collections.Generic;
using Oberon0.Compiler.Expressions.Functions;
using Oberon0.Compiler.Expressions.Operations.Internal;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions;
internal partial class ExpressionRepository
{
    private static ExpressionRepository? _instance;

    private ExpressionRepository()
    {
        LoadOperations();
        LoadFunctions();
    }

    internal static partial List<ArithmeticOpKey> GetSupportedOperations();

    internal static partial List<IInternalFunction> GetSupportedFunctions();

    private void LoadOperations()
    {
        var operations = GetSupportedOperations();

        // translate all arithmetic operations to a dictionary
        ArithmeticOperations = [];
        foreach (var op in operations)
        {
            ArithmeticOperations.Add(
                op,
                new ArithmeticOperation(op.Instance!, op));
        }
    }

    private void LoadFunctions()
    {
        var functions = GetSupportedFunctions();

        // translate all arithmetic operations to a dictionary
        InternalFunctions = [];
        foreach (var func in functions)
        {
            foreach (var proto in func.Prototypes)
            {
                InternalFunctions.Add(proto, func);
            }
        }
    }

    /// <summary>
    ///     Gets a singleton instance.
    /// </summary>
    /// <value>The instance.</value>
    public static ExpressionRepository Instance { get; } = _instance ??= new ExpressionRepository();

    private Dictionary<string, IInternalFunction> InternalFunctions { get; set; } = null!;
    
    private Dictionary<ArithmeticOpKey, ArithmeticOperation> ArithmeticOperations { get; set; } = null!;

    /// <summary>
    ///     Gets the specified operation.
    /// </summary>
    /// <param name="operation">The operation.</param>
    /// <param name="left">The left.</param>
    /// <param name="right">The right.</param>
    /// <returns>ArithmeticOperation</returns>
    public ArithmeticOperation? Get(int operation, BaseTypes left, BaseTypes right)
    {
        var key = new ArithmeticOpKey(operation, left, right);
        return ArithmeticOperations.GetValueOrDefault(key);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prototype"></param>
    /// <returns></returns>
    public IInternalFunction? GetInternalFunction(string prototype)
    {
        return InternalFunctions.GetValueOrDefault(prototype);
    }
}
