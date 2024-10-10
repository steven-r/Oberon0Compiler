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

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ArithmeticOperationAttribute(
        int operation,
        BaseTypes leftHandType,
        BaseTypes rightHandType,
        BaseTypes resultType)
        : ExportAttribute(typeof(IArithmeticOperation)), IArithmeticOpMetadata
    {
        [UsedImplicitly] public BaseTypes LeftHandType { get; set; } = leftHandType;

        [UsedImplicitly] public BaseTypes RightHandType { get; set; } = rightHandType;

        public int Operation { get; set; } = operation;

        public BaseTypes ResultType { get; set; } = resultType;
    }
}
