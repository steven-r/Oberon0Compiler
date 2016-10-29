﻿namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    /// Define a procedure/function parameter
    /// </summary>
    /// <seealso cref="Oberon0.Compiler.Definitions.Declaration" />
    public class ProcedureParameter: Declaration
    {
        /// <summary>
        /// If set, this parameter is referenced, not by value
        /// </summary>
        /// <value><c>true</c> if this instance is variable; otherwise, <c>false</c>.</value>
        public bool IsVar { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcedureParameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="type">The type.</param>
        /// <param name="isVar">if set to <c>true</c> [is variable].</param>
        public ProcedureParameter(string name, TypeDefinition type, bool isVar)
            :base (name, type)
        {
            IsVar = isVar;
        }
    }
}