#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Diagnostics.CodeAnalysis;

namespace Oberon0.Compiler.Types
{
    public class SimpleTypeDefinition : TypeDefinition
    {
        private SimpleTypeDefinition(BaseTypes baseTypes, string name)
            : base(baseTypes)
        {
            Name = name;
        }

        public SimpleTypeDefinition(BaseTypes baseTypes, string name, bool isInternal)
            : base(baseTypes, isInternal)
        {
            Name = name;
        }

        /// <summary>
        ///     Gets or sets the global reference to "BOOLEAN".
        /// </summary>
        public static TypeDefinition BoolType { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the global reference to "INTEGER".
        /// </summary>
        public static TypeDefinition IntType { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the global reference to "REAL".
        /// </summary>
        public static TypeDefinition RealType { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the global reference to "STRING".
        /// </summary>
        public static TypeDefinition StringType { get; set; } = null!;

        /// <summary>
        ///     Gets or sets the global reference to "VOID".
        /// </summary>
        public static TypeDefinition VoidType { get; set; } = null!;

        public override TypeDefinition Clone(string name)
        {
            return new SimpleTypeDefinition(Type, name);
        }

        public override bool IsAssignable(TypeDefinition sourceType)
        {
            return sourceType.Type == Type // same simple type
             || sourceType.Type.HasFlag(BaseTypes.Int) && Type.HasFlag(BaseTypes.Real)
             || sourceType.Type.HasFlag(BaseTypes.Int) && Type.HasFlag(BaseTypes.Bool);
        }

        [ExcludeFromCodeCoverage]
        public override string ToString()
        {
            return Name ?? "<unset>";
        }
    }
}
