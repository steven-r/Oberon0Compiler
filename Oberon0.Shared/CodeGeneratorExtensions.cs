#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Shared
{
    /// <summary>
    /// Extension methods for <see cref="ICodeGenerator"/> being used in several parts
    /// </summary>
    public static class CodeGeneratorExtensions
    {
        /// <summary>
        /// Generate a full qualified main class name
        /// </summary>
        /// <param name="cg">The code generator class to be used</param>
        /// <returns>The fully qualified class name</returns>
        public static string GetMainClassName(this ICodeGenerator cg)
        {
            return cg.MainClassNamespace + "." + cg.MainClassName;
        }
    }
}
