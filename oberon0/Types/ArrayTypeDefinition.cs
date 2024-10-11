#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Types
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ArrayTypeDefinition" /> class.
    /// </summary>
    /// <param name="size">
    ///     The size.
    /// </param>
    /// <param name="baseType">
    ///     The base type.
    /// </param>
    public class ArrayTypeDefinition(int size, TypeDefinition baseType) : TypeDefinition(BaseTypes.Array)
    {
        /// <summary>
        ///     Gets the array type.
        /// </summary>
        public TypeDefinition ArrayType { get; } = baseType;

        /// <summary>
        ///     Gets the array size
        /// </summary>
        public int Size { get; } = size;

        public override TypeDefinition Clone(string name)
        {
            return new ArrayTypeDefinition(Size, ArrayType) {Name = name};
        }

        public override bool IsAssignable(TypeDefinition sourceType)
        {
            if (sourceType is ArrayTypeDefinition array)
            {
                return Name != null && array.Name != null && array.Size == Size
                 && ArrayType.IsAssignable(array.ArrayType);
            }

            return false;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"ARRAY {Size} OF {ArrayType}";
        }
    }
}
