#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StandardFunctionMetadataView.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil/StandardFunctionMetadataView.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil.PredefinedFunctions
{
    public class StandardFunctionMetadataView
    {
        /// <summary>
        /// Gets or sets the name of the function.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the namespace.
        /// </summary>
        /// <value>The namespace.</value>
        public string Namespace { get; set; }

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