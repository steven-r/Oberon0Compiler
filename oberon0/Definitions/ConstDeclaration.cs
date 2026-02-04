#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Expressions.Constant;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    /// Represents a constant declaration in an Oberon0 program.
    /// </summary>
    public class ConstDeclaration : Declaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstDeclaration"/> class.
        /// </summary>
        /// <param name="name">The name of the constant.</param>
        /// <param name="type">The type definition of the constant.</param>
        /// <param name="value">The constant expression representing the value of the constant.</param>
        public ConstDeclaration(string name, TypeDefinition type, ConstantExpression value)
            : base(name, type)
        {
            Value = value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstDeclaration"/> class with an associated block.
        /// </summary>
        /// <param name="name">The name of the constant.</param>
        /// <param name="type">The type definition of the constant.</param>
        /// <param name="value">The constant expression representing the value of the constant.</param>
        /// <param name="block">The block in which the constant is declared.</param>
        public ConstDeclaration(string name, TypeDefinition type, ConstantExpression value, Block block)
            : base(name, type, block)
        {
            Value = value;
        }

        /// <summary>
        /// Gets the constant expression representing the value of this constant declaration.
        /// </summary>
        public ConstantExpression Value { get; }

        /// <summary>
        /// Returns a string representation of this constant declaration.
        /// </summary>
        /// <returns>A string in the format "Const {Name} = {Value}".</returns>
        public override string ToString()
        {
            return $"Const {Name} = {Value}";
        }
    }
}
