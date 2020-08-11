#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Composition;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ArithmeticOperationAttribute : ExportAttribute, IArithmeticOpMetadata
    {
        public ArithmeticOperationAttribute(
            int operation,
            BaseTypes leftHandType,
            BaseTypes rightHandType,
            BaseTypes resultType)
            : base(typeof(IArithmeticOperation))
        {
            Operation = operation;
            LeftHandType = leftHandType;
            RightHandType = rightHandType;
            ResultType = resultType;
        }

        public BaseTypes LeftHandType { get; set; }

        public BaseTypes RightHandType { get; set; }

        public int Operation { get; set; }

        public BaseTypes ResultType { get; set; }
    }
}