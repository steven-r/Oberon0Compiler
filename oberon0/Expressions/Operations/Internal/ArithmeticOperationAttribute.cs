#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArithmeticOperationAttribute.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/ArithmeticOperationAttribute.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    using System;
    using System.Composition;

    using Oberon0.Compiler.Types;

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

        public int Operation { get; set; }

        public BaseTypes ResultType { get; set; }

        public BaseTypes RightHandType { get; set; }
    }
}