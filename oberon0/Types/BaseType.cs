#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseType.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/BaseType.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Types
{
    /// <summary>
    /// Standard types 
    /// </summary>
    public enum BaseType
    {
        /// <summary>
        /// Standard integer
        /// </summary>
        IntType = SimpleType + 1,

        /// <summary>
        /// The string type - Not in use
        /// </summary>
        StringType = SimpleType + 2,

        /// <summary>
        /// The decimal type
        /// </summary>
        DecimalType = SimpleType + 3,

        /// <summary>
        /// The bool type
        /// </summary>
        BoolType = SimpleType + 4,

        /// <summary>
        /// a "non" type. This means no value (like an empty return value for a function)
        /// </summary>
        VoidType = SimpleType + 5,

        /// <summary>
        /// Any type - used for internal functions (like WRITELN)
        /// </summary>
        AnyType = 0x20000,

        /// <summary>
        /// A non-base type (array or record)
        /// </summary>
        ComplexType = 0x40000,

        /// <summary>
        /// Representing a type not array or complex
        /// </summary>
        SimpleType = 0x10000
    }
}