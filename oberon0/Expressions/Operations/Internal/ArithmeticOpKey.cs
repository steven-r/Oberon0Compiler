#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArithmeticOpKey.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/ArithmeticOpKey.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    using System;

    using Oberon0.Compiler.Types;

    /// <summary>
    /// Helper class to create a dictionary of operations and it's left and right parameters
    /// </summary>
    internal class ArithmeticOpKey : IArithmeticOpMetadata, IEquatable<ArithmeticOpKey>
    {
        public ArithmeticOpKey(
            int operation,
            BaseTypes leftHandType,
            BaseTypes rightHandType,
            BaseTypes targetTypes = BaseTypes.AnyType)
        {
            Operation = operation;
            LeftHandType = leftHandType;
            RightHandType = rightHandType;
            ResultType = targetTypes;
        }

        public BaseTypes LeftHandType { get; }

        public int Operation { get; }

        public BaseTypes ResultType { get; }

        public BaseTypes RightHandType { get; }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return Equals((ArithmeticOpKey)obj);
        }

        public bool Equals(ArithmeticOpKey other)
        {
            if (other == null) return false;
            return Operation == other.Operation && LeftHandType == other.LeftHandType
                                                && RightHandType == other.RightHandType;
        }

        public override int GetHashCode()
        {
            // ignore Target type
            return 17 ^ Operation.GetHashCode() ^ LeftHandType.GetHashCode() ^ RightHandType.GetHashCode();
        }
    }
}