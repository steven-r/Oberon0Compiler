#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using System.Diagnostics.CodeAnalysis;
using Oberon0.Compiler.Generator;

namespace Oberon0.Compiler.Types
{
    public abstract class TypeDefinition
    {
        /// <summary>
        ///     The name for the type "VOID"
        /// </summary>
        public const string VoidTypeName = "$$VOID";

        protected TypeDefinition(BaseTypes baseTypes)
        {
            Type = baseTypes;
        }

        protected TypeDefinition(BaseTypes baseTypes, bool isInternal)
        {
            Type = baseTypes;
            IsInternal = isInternal;
        }

        public BaseTypes Type { get; }

        /// <summary>
        ///     Gets or sets additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        [ExcludeFromCodeCoverage]
        public IGeneratorInfo GeneratorInfo { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the given type declaration is exportable.
        /// </summary>
        public bool Exportable { get; set; }

        public bool IsInternal { get; }

        public string Name { get; set; }

        /// <summary>
        ///     Clones the current type and name it <see cref="name" />.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The <see cref="TypeDefinition" />.</returns>
        public abstract TypeDefinition Clone(string name);

        public abstract bool IsAssignable(TypeDefinition sourceType);
    }
}
