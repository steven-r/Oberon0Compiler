#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;

namespace Oberon0.Compiler.Types
{
    /// <summary>
    ///     Standard types
    /// </summary>
    public enum BaseTypes
    {
        /// <summary>
        ///     Standard integer
        /// </summary>
        Int = Simple + 1,

        /// <summary>
        ///     The string type - Not in use
        /// </summary>
        String = Simple + 2,

        /// <summary>
        ///     The REAL type
        /// </summary>
        Real = Simple + 4,

        /// <summary>
        /// Generic number type
        /// </summary>
        Number = Real | Int,

        /// <summary>
        ///     The bool type
        /// </summary>
        Bool = Simple + 8,

        /// <summary>
        ///     a "non" type. This means no value (like an empty return value for a function)
        /// </summary>
        Void = Simple + 16,

        /// <summary>
        ///     record type
        /// </summary>
        Record = Complex + 1,

        /// <summary>
        ///     array type
        /// </summary>
        Array = Complex + 2,

        /// <summary>
        ///     Any type - used for internal functions (like WRITELN)
        /// </summary>
        Any = 0x20000,

        /// <summary>
        ///     Complex types
        /// </summary>
        Complex = 0x40000,

        /// <summary>
        ///     Representing a type not array or complex
        /// </summary>
        Simple = 0x10000
    }
}
