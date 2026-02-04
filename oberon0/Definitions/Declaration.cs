#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Generator;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    /// Represents a declaration in an Oberon0 program, such as variables, constants, types, or procedures.
    /// </summary>
    /// <param name="name">The name of the declared entity.</param>
    /// <param name="type">The type definition of the declared entity.</param>
    /// <param name="block">The block in which the declaration is defined, or <c>null</c> if not associated with a specific block.</param>
    public abstract class Declaration(string name, TypeDefinition type, Block? block)
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Declaration"/> class without an associated block.
        /// </summary>
        /// <param name="name">The name of the declared entity.</param>
        /// <param name="type">The type definition of the declared entity.</param>
        protected Declaration(string name, TypeDefinition type)
            : this(name, type, null)
        {
        }

        /// <summary>
        ///     Gets the block.
        /// </summary>
        public Block? Block { get; } = block;

        /// <summary>
        ///     Gets or sets additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        public IGeneratorInfo? GeneratorInfo { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the given declaration is exportable.
        /// </summary>
        public bool Exportable { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; } = name;

        /// <summary>
        ///     Gets the type.
        /// </summary>
        public TypeDefinition Type { get; } = type;

        /// <summary>
        /// Returns a string representation of this declaration.
        /// </summary>
        /// <returns>A string in the format "{Name}:{Type}".</returns>
        public override string ToString()
        {
            return $"{Name}:{Type}";
        }
    }
}
