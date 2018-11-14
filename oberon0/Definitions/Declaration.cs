#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Declaration.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/Declaration.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Definitions
{
    using Oberon0.Compiler.Generator;
    using Oberon0.Compiler.Types;

    public class Declaration
    {
        public Declaration(string name, TypeDefinition type)
            : this(name, type, null)
        {
        }

        public Declaration(string name, TypeDefinition type, Block block)
        {
            this.Name = name;
            this.Type = type;
            this.Block = block;
        }

        /// <summary>
        /// Gets the block.
        /// </summary>
        public Block Block { get; }

        /// <summary>
        /// Gets or sets additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        public IGeneratorInfo GeneratorInfo { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the type.
        /// </summary>
        public TypeDefinition Type { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{this.Name}:{this.Type}";
        }
    }
}