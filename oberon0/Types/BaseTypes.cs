#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseTypes.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/BaseTypes.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Types
{
    using System;

    /// <summary>
    /// Standard types 
    /// </summary>
    public enum BaseTypes
    {
        /// <summary>
        /// Standard integer
        /// </summary>
        Int = Simple + 1,

        /// <summary>
        /// The string type - Not in use
        /// </summary>
        String = Simple + 2,

        /// <summary>
        /// The REAL type
        /// </summary>
        Real = Simple + 3,

        /// <summary>
        /// The bool type
        /// </summary>
        Bool = Simple + 4,

        /// <summary>
        /// a "non" type. This means no value (like an empty return value for a function)
        /// </summary>
        Void = Simple + 5,

        /// <summary>
        /// record type
        /// </summary>
        Record = Complex + 1,

        /// <summary>
        /// array type
        /// </summary>
        Array = Complex + 2,

        /// <summary>
        /// Any type - used for internal functions (like WRITELN)
        /// </summary>
        Any = 0x20000,

        /// <summary>
        /// Complex types
        /// </summary>
        Complex = 0x40000,

        /// <summary>
        /// Representing a type not array or complex
        /// </summary>
        Simple = 0x10000
    }
}