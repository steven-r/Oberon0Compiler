#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.MsilBin.PredefinedFunctions
{
    public class StandardFunctionMetadataView
    {
        /// <summary>
        /// Gets or sets the name of the function.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the types of the parameter
        /// </summary>
        /// <value>The parameter types.</value>
        public string ParameterTypes { get; set; }

        /// <summary>
        /// Gets or sets the name of the return type
        /// </summary>
        public string ReturnType { get; set; }
    }
}