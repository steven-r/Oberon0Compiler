#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using System.IO;
using Oberon0.Compiler.Definitions;

namespace Oberon0.Shared
{
    /// <summary>
    ///     Code generator interface
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        ///     The module for which the generator is used for
        /// </summary>
        Module Module { get; set; }

        /// <summary>
        ///     The name of the main class (mainly used for testing)
        /// </summary>
        string MainClassName { get; set; }


        /// <summary>
        ///     The name of the used namespace (mainly used for testing)
        /// </summary>
        string MainClassNamespace { get; set; }

        /// <summary>
        ///     Dump generated (source) code to string
        /// </summary>
        /// <returns></returns>
        string IntermediateCode();

        /// <summary>
        ///     Dump generated source code to <see cref="TextWriter" />
        /// </summary>
        /// <param name="writer"></param>
        void WriteIntermediateCode(TextWriter writer);

        /// <summary>
        ///     Starts code generation
        /// </summary>
        void GenerateIntermediateCode();


        /// <summary>
        /// Generate a final binary output (e.g. DLL or EXE) based on <see cref="Module"/> settings.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        //bool GenerateBinary(string filename = null);
    }
}
