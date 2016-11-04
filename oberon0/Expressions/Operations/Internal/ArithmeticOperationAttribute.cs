using System;
using System.ComponentModel.Composition;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Compiler.Expressions.Operations
{
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ArithmeticOperationAttribute: ExportAttribute, IArithmeticOpMetadata
    {
        public ArithmeticOperationAttribute(TokenType operation, BaseType leftHandType, BaseType rightHandType, BaseType targetType)
        {
            Operation = operation;
            LeftHandType = leftHandType;
            RightHandType = rightHandType;
            TargetType = targetType;
        }

        public TokenType Operation { get; }

        public BaseType LeftHandType { get; }

        public BaseType RightHandType { get; }

        public BaseType TargetType { get; }
    }
}