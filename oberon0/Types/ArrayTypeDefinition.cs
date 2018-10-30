#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayTypeDefinition.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/ArrayTypeDefinition.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Types
{
    public class ArrayTypeDefinition : TypeDefinition
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayTypeDefinition"/> class.
        /// </summary>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <param name="baseType">
        /// The base type.
        /// </param>
        public ArrayTypeDefinition(int size, TypeDefinition baseType)
            : base(BaseTypes.Array)
        {
            Size = size;
            ArrayType = baseType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayTypeDefinition"/> class.
        /// </summary>
        /// <param name="size">
        /// The size.
        /// </param>
        /// <param name="baseType">
        /// The base type.
        /// </param>
        /// <param name="name">
        /// The name.
        /// </param>
        public ArrayTypeDefinition(int size, TypeDefinition baseType, string name)
            : base(BaseTypes.Array)
        {
            Size = size;
            ArrayType = baseType;
            Name = name;
        }

        /// <summary>
        /// Gets the array type.
        /// </summary>
        public TypeDefinition ArrayType { get; }

        /// <summary>
        /// Gets the array size
        /// </summary>
        public int Size { get; }

        public override TypeDefinition Clone(string name)
        {
            return new ArrayTypeDefinition(this.Size, this.ArrayType, name);
        }

        public override bool IsAssignable(TypeDefinition sourceType)
        {
            if (sourceType is ArrayTypeDefinition array)
            {
                return array.Size == Size && ArrayType.IsAssignable(array);
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