#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Composition;
using JetBrains.Annotations;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Functions;

[MetadataAttribute]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class InternalFunctionAttribute(
    string prototype)
    : ExportAttribute(typeof(IInternalFunction))
{

    public string Prototype { get; set; } = prototype;
}