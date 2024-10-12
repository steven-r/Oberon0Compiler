#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using JetBrains.Annotations;

namespace Oberon0.Generator.MsilBin.PredefinedFunctions
{
    [method: UsedImplicitly]
    public class StandardFunctionMetadataView()
    {
        /// <summary>
        ///     Gets or sets the name of the function.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; [UsedImplicitly] set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the types of the parameter
        /// </summary>
        /// <value>The parameter types.</value>
        public string ParameterTypes { get; [UsedImplicitly] set; } = string.Empty;

        /// <summary>
        ///     Gets or sets the name of the return type
        /// </summary>
        public string ReturnType { get; [UsedImplicitly] set; } = string.Empty;
    }
}
