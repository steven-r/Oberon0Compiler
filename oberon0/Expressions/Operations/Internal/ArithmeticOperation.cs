#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    internal class ArithmeticOperation
    {
        public ArithmeticOperation(IArithmeticOperation operation, IArithmeticOpMetadata metadata)
        {
            Operation = operation;
            Metadata = metadata;
        }

        public IArithmeticOperation Operation { get; }
        public IArithmeticOpMetadata Metadata { get; }
    }
}
