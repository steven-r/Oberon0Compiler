#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Composition;
using System.Diagnostics.CodeAnalysis;
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
        // ReSharper disable once UnusedMember.Global
        public BaseTypes LeftHandType { get; set; } = leftHandType;

        // ReSharper disable once UnusedMember.Global
        public BaseTypes RightHandType { get; set; } = rightHandType;

        public int Operation { 
            get;
            [ExcludeFromCodeCoverage(Justification = "Called by MEF")]
            set;
        } = operation;

        public BaseTypes ResultType { 
            get; 
            [ExcludeFromCodeCoverage(Justification = "Called by MEF")]
            set;
        } = resultType;
    }
}
