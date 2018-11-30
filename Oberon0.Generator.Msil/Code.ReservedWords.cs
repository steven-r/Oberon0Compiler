#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Code.ReservedWords.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil/Code.ReservedWords.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil
{
    using System.Collections.Generic;

    /// <summary>
    /// The code.
    /// </summary>
    public partial class Code
    {
        private static readonly HashSet<string> ReservedWords = new HashSet<string>
            {
                "true",
                "false",
                "module",
                "field",
                "int",
                "int32",
                "double",
                "nop",
                "void",
                "ret",
                "instance"
            };

        public static string MakeName(string originalName)
        {
            if (ReservedWords.Contains(originalName))
            {
                return $"'{originalName}'";
            }

            return originalName;
        }
    }
}