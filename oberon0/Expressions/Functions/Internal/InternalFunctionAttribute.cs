#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Composition;
using System.Diagnostics.CodeAnalysis;

namespace Oberon0.Compiler.Expressions.Functions.Internal;

[MetadataAttribute]
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class InternalFunctionAttribute(
    string prototype)
    : ExportAttribute(typeof(IInternalFunction))
{

    // ReSharper disable once UnusedMember.Global
    public string Prototype { get;
        [ExcludeFromCodeCoverage]
        set; } = prototype;
}