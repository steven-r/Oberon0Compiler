#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TypeDefinition.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/TypeDefinition.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Types
{
    using Oberon0.Compiler.Generator;

    public abstract class TypeDefinition
    {
        /// <summary>
        /// The name for the type "VOID"
        /// </summary>
        public const string VoidTypeName = "$$VOID";

        protected TypeDefinition(BaseType baseType)
        {
            this.BaseType = baseType;
        }

        protected TypeDefinition(BaseType baseType, bool isInternal)
        {
            this.BaseType = baseType;
            this.IsInternal = isInternal;
        }

        public BaseType BaseType { get; }

        /// <summary>
        /// Gets or sets additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        public IGeneratorInfo GeneratorInfo { get; set; }

        public bool IsInternal { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Clones the current type and name it <see cref="name"/>.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The <see cref="TypeDefinition"/>.</returns>
        public abstract TypeDefinition Clone(string name);
    }
}