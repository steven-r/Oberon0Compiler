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
    /// <summary>
    ///     Base class for all type definitions used by the compiler.
    /// </summary>
    public abstract class TypeDefinition(BaseTypes baseTypes, bool isInternal)
    {
        /// <summary>
        ///     The name for the type "VOID"
        /// </summary>
        public const string VoidTypeName = "$$VOID";

        /// <summary>
        ///     The built-in integer type name.
        /// </summary>
        public const string IntegerTypeName = "INTEGER";

        /// <summary>
        ///     The built-in real type name.
        /// </summary>
        public const string RealTypeName = "REAL";

        /// <summary>
        ///     The built-in string type name.
        /// </summary>
        public const string StringTypeName = "STRING";

        /// <summary>
        ///     The built-in boolean type name.
        /// </summary>
        public const string BooleanTypeName = "BOOLEAN";

        protected TypeDefinition(BaseTypes baseTypes)
            : this(baseTypes, false)
        {
        }

        /// <summary>
        ///     Gets the base type flags for this definition.
        /// </summary>
        public BaseTypes Type => baseTypes;

        /// <summary>
        ///     Gets or sets additional information used by the generator engine
        /// </summary>
        /// <value>Generator information.</value>
        [ExcludeFromCodeCoverage]
        // ReSharper disable once UnusedMember.Global
        public IGeneratorInfo? GeneratorInfo { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether the given type declaration is exportable.
        /// </summary>
        public bool Exportable { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this type is internal to the compiler.
        /// </summary>
        public bool IsInternal => isInternal;

        /// <summary>
        ///     Gets or sets the declared name of the type.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        ///     Clones the current type and name it <see cref="name" />.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>The <see cref="TypeDefinition" />.</returns>
        public abstract TypeDefinition Clone(string name);

        /// <summary>
        ///     Determines whether values of <paramref name="sourceType" /> can be assigned to this type.
        /// </summary>
        /// <param name="sourceType">The type of the source expression.</param>
        /// <returns><c>true</c> when the assignment is valid; otherwise <c>false</c>.</returns>
        public abstract bool IsAssignable(TypeDefinition sourceType);

        /// <summary>
        ///     Gets a value indicating whether this is a simple type.
        /// </summary>
        public bool IsSimpleType => Type.HasFlag(BaseTypes.Simple);

        /// <summary>
        ///     Gets a value indicating whether this is a complex type.
        /// </summary>
        public bool IsComplexType => Type.HasFlag(BaseTypes.Complex);

        /// <summary>
        ///     Gets a value indicating whether this is the special Any type.
        /// </summary>
        public bool IsAnyType => Type == BaseTypes.Any;
    }
}
