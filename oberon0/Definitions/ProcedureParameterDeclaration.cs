#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using System.Diagnostics;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    ///     Define a procedure/function parameter
    /// </summary>
    /// <seealso cref="Declaration" />
    [DebuggerDisplay("{Name}: {TypeName}")]
    public class ProcedureParameterDeclaration : Declaration
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ProcedureParameterDeclaration" /> class.
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
        ///     Gets or sets a value indicating whether this parameter is referenced, not by value.
        /// </summary>
        /// <value><c>true</c> if this instance is variable; otherwise, <c>false</c>.</value>
        public bool IsVar { get; }

        /// <summary>
        ///     Return a string representation of the parameter type
        /// </summary>
        public string TypeName => $"{(IsVar ? "&" : string.Empty)}{Type.Name}";

        public override string ToString()
        {
            return $"{(IsVar ? "VAR " : string.Empty)}{Name}: {Type.Name}";
        }
    }
}
