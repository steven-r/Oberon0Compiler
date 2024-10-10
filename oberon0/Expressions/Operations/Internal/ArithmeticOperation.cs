#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions.Operations.Internal
{
    public class ArithmeticOperation(IArithmeticOperation operation, IArithmeticOpMetadata metadata)
    {
        public IArithmeticOperation Operation { get; } = operation;
        public IArithmeticOpMetadata Metadata { get; } = metadata;
    }
}
