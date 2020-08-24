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
    public class Declaration
    {
        public Declaration(string name, TypeDefinition type)
            : this(name, type, null)
        {
        }

        public Declaration(string name, TypeDefinition type, Block block)
        {
            Name = name;
            Type = type;
            Block = block;
        }

        /// <summary>
        ///     Gets the block.
        /// </summary>
        public Block Block { get; }

        /// <summary>
        ///     Gets or sets additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        public IGeneratorInfo GeneratorInfo { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the given declaration is exportable.
        /// </summary>
        public bool Exportable { get; set; }

        /// <summary>
        ///     Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        public TypeDefinition Type { get; }

        public override string ToString()
        {
            return $"{Name}:{Type}";
        }
    }
}
