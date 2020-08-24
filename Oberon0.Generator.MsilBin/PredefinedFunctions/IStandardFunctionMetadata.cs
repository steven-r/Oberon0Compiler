#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;

namespace Oberon0.Generator.MsilBin.PredefinedFunctions
{
    public interface IStandardFunctionMetadata
    {
        /// <summary>
        ///     The name of the function.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        ///     The name of the return type
        /// </summary>
        TypeDefinition ReturnType { get; }

        /// <summary>
        ///     The types of the parameter
        /// </summary>
        /// <value>The parameter types.</value>
        ProcedureParameterDeclaration[] ParameterTypes { get; }
    }
}
