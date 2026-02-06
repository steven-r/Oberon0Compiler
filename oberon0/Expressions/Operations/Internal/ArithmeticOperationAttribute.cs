#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations.Internal;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ArithmeticOperationAttribute(
    int operation,
    BaseTypes leftHandType,
    BaseTypes rightHandType,
    BaseTypes resultType)
    : Attribute
{
    public BaseTypes LeftHandType => leftHandType;

    public BaseTypes RightHandType => rightHandType;

    public int Operation => operation;

    public BaseTypes ResultType => resultType;
}
