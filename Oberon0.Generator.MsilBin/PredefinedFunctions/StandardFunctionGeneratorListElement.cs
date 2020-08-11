#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System.Diagnostics;
using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Types;

namespace Oberon0.Generator.MsilBin.PredefinedFunctions
{
    [DebuggerDisplay("Function {" + nameof(InstanceKey) + "}")]
    internal class StandardFunctionGeneratorListElement : IStandardFunctionMetadata
    {
        /// <summary>
        /// Gets or sets the instance handling the standard function
        /// </summary>
        /// <value>The instance.</value>
        public IStandardFunctionGenerator Instance { get; set; }

        /// <summary>
        /// Gets or sets the lookup key to find the appropriate function
        /// </summary>
        /// <value>The instance key.</value>
        public string InstanceKey { get; set; }

        public string AdditionalInfo { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }

        /// <inheritdoc />
        public TypeDefinition ReturnType { get; set; }

        /// <inheritdoc />
        public ProcedureParameterDeclaration[] ParameterTypes { get; set; }
    }
}
