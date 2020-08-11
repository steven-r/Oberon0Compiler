#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    /// <summary>
    /// Helper class to create a dictionary of operations and it's left and right parameters
    /// </summary>
    internal class ArithmeticOpKey : IArithmeticOpMetadata, IEquatable<ArithmeticOpKey>
    {
        public ArithmeticOpKey(
            int operation,
            BaseTypes leftHandType,
            BaseTypes rightHandType,
            BaseTypes targetTypes = BaseTypes.Any)
        {
            Operation = operation;
            LeftHandType = leftHandType;
            RightHandType = rightHandType;
            ResultType = targetTypes;
        }

        public BaseTypes LeftHandType { get; }

        public BaseTypes RightHandType { get; }

        public int Operation { get; }

        public BaseTypes ResultType { get; }

        public bool Equals(ArithmeticOpKey other)
        {
            if (other == null) return false;
            return Operation == other.Operation && LeftHandType == other.LeftHandType
                                                && RightHandType == other.RightHandType;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            return Equals((ArithmeticOpKey)obj);
        }

        public override int GetHashCode()
        {
            // ignore Target type
            return 17 ^ Operation.GetHashCode() ^ LeftHandType.GetHashCode() ^ RightHandType.GetHashCode();
        }
    }
}