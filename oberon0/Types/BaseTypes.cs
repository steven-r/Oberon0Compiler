#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;

namespace Oberon0.Compiler.Types;

/// <summary>
///     Standard types
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1130:Bitwise operation on enum without Flags attribute", Justification = "<Ausstehend>")]
public enum BaseTypes
{
    None = 0,

    /// <summary>
    ///     Representing a type not array or complex
    /// </summary>
    Simple = 0x10000,

    /// <summary>
    ///     Standard integer
    /// </summary>
    Int = 0x10001,

    /// <summary>
    ///     The string type
    /// </summary>
    String = 0x10002,

    /// <summary>
    ///     The REAL type
    /// </summary>
    Real = 0x10004,

    /// <summary>
    /// Generic number type
    /// </summary>
    Number = Int | Real,

    /// <summary>
    ///     The bool type
    /// </summary>
    Bool = 0x10008,

    /// <summary>
    ///     a "non" type. This means no value (like an empty return value for a function)
    /// </summary>
    Void = 0x10010,

    /// <summary>
    ///     Any type - used for internal functions (like WRITELN)
    /// </summary>
    Any = 0x20000,

    /// <summary>
    ///     Complex types
    /// </summary>
    Complex = 0x40000,

    /// <summary>
    ///     record type
    /// </summary>
    Record = 0x40001,

    /// <summary>
    ///     array type
    /// </summary>
    Array = 0x40002,
}
