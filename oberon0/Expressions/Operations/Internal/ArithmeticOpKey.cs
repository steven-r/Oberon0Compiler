using System;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Expressions.Operations;

namespace Oberon0.Compiler.Expressions
{
    /// <summary>
    /// Helper class to create a dictionary of operations and it's left and right parameters
    /// </summary>
    internal class ArithmeticOpKey: IArithmeticOpMetadata, IEquatable<ArithmeticOpKey>
    {
        #region IEquatable

        public bool Equals(ArithmeticOpKey other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            return Operation == other.Operation &&
                   LeftHandType == other.LeftHandType &&
                   RightHandType == other.RightHandType;
        }
        #endregion

        #region Constructors

        public ArithmeticOpKey(TokenType operation, BaseType leftHandType, BaseType rightHandType, BaseType targetType = BaseType.AnyType)
        {
            Operation = operation;
            LeftHandType = leftHandType;
            RightHandType = rightHandType;
            TargetType = targetType;
        }

        #endregion

        public TokenType Operation { get; }
        public BaseType LeftHandType { get; }
        public BaseType RightHandType { get; }
        public BaseType TargetType { get; }

        public override int GetHashCode()
        {
            // ignore Target type
            return 17 ^ Operation.GetHashCode() ^ LeftHandType.GetHashCode() ^ RightHandType.GetHashCode();
        }
    }
}