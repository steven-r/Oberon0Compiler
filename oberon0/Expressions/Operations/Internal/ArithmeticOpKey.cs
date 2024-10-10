#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    /// <summary>
    ///     Helper class to create a dictionary of operations, and it's left and right parameters
    /// </summary>
    internal class ArithmeticOpKey(
        int operation,
        BaseTypes leftHandType,
        BaseTypes rightHandType,
        BaseTypes targetTypes = BaseTypes.Any)
        : IArithmeticOpMetadata, IEquatable<ArithmeticOpKey>
    {
        public BaseTypes LeftHandType { get; } = leftHandType;

        public BaseTypes RightHandType { get; } = rightHandType;

        public int Operation { get; } = operation;

        public BaseTypes ResultType { get; } = targetTypes;

        [ExcludeFromCodeCoverage]
        public bool Equals(ArithmeticOpKey? other)
        {
            if (other == null)
            {
                return false;
            }

            return Operation == other.Operation && LeftHandType == other.LeftHandType
             && RightHandType == other.RightHandType;
        }

        [ExcludeFromCodeCoverage]
        public override bool Equals(object? obj)
        {
            return obj != null && Equals((ArithmeticOpKey) obj);
        }

        public override int GetHashCode()
        {
            // ignore Target type
            return 17 ^ Operation.GetHashCode() ^ LeftHandType.GetHashCode() ^ RightHandType.GetHashCode();
        }
    }
}
