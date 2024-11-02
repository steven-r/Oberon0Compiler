#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;

namespace Oberon0System.Attributes
{
    /// <summary>
    ///     Declare a library export for the Oberon0 language
    /// </summary>
    /// <seealso cref="Attribute" />
    [AttributeUsage(AttributeTargets.Method)]
    public class Oberon0ExportAttribute : Attribute
    {
        /// <summary>
        ///     Attribute being used to lookup library functions
        /// </summary>
        /// <param name="name">The name of the function</param>
        /// <param name="returnType">The return type (Oberon0-Notation)</param>
        /// <param name="parameters">
        ///     list of types (oberon0 notation) describing the parameters. Reference parameters are noted
        ///     with a beginning ampersand (e.g. <code>&amp;INTEGER</code>)
        /// </param>
        public Oberon0ExportAttribute(string name, string returnType, params string[] parameters)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ReturnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
            Parameters = parameters;
        }

        /// <summary>
        ///     Gets the name of the function
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     Gets the parameter list (can be empty)
        /// </summary>
        public string[] Parameters { get; }

        /// <summary>
        ///     Gets the return type
        /// </summary>
        public string ReturnType { get; }
    }
}
