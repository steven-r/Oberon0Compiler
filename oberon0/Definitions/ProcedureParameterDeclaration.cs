#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProcedureParameterDeclaration.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/ProcedureParameterDeclaration.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Definitions
{
    using Oberon0.Compiler.Types;

    /// <summary>
    /// Define a procedure/function parameter
    /// </summary>
    /// <seealso cref="Declaration" />
    public class ProcedureParameterDeclaration : Declaration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProcedureParameterDeclaration"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="block">The corresponding block</param>
        /// <param name="type">The type.</param>
        /// <param name="isVar">if set to <c>true</c> [is variable].</param>
        public ProcedureParameterDeclaration(string name, Block block, TypeDefinition type, bool isVar)
            : base(name, type, block)
        {
            IsVar = isVar;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this parameter is referenced, not by value.
        /// </summary>
        /// <value><c>true</c> if this instance is variable; otherwise, <c>false</c>.</value>
        public bool IsVar { get; set; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Name}:{Type}{(IsVar ? "&" : string.Empty)}";
        }
    }
}