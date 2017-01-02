using System;
using System.ComponentModel.Composition;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    [MetadataAttribute, AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ArithmeticOperationAttribute: ExportAttribute, IArithmeticOpMetadata
    {
        public ArithmeticOperationAttribute(int operation, BaseType leftHandType, BaseType rightHandType, BaseType resultType)
        {
            Operation = operation;
            LeftHandType = leftHandType;
            RightHandType = rightHandType;
            ResultType = resultType;
        }

        public int Operation { get; }

        public BaseType LeftHandType { get; }

        public BaseType RightHandType { get; }

        public BaseType ResultType { get; }
    }
}