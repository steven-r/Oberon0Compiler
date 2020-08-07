#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Expressions
{
    public class UnaryExpression: BinaryExpression
    {
        /// <summary>
        /// For unary expressions this value is true.
        /// </summary>
        /// <value><c>true</c> if this instance is unary; otherwise, <c>false</c>.</value>
        public override bool IsUnary => true;
    }
}
