#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

namespace Oberon0.Generator.MsilBin.PredefinedFunctions
{
    public interface IStandardFunctionExportMetadata
    {
        /// <summary>
        ///     The name of the function.
        /// </summary>
        /// <value>The name.</value>
        string Name { get; }

        /// <summary>
        ///     The name of the return type
        /// </summary>
        string ReturnType { get; }

        /// <summary>
        ///     A comma separated list of parameter types. Reference parameters need to
        ///     be pretended with '&amp;', e.g. <c>INTEGER,&amp;BOOL,REAL</c>
        /// </summary>
        /// <value>The parameter types.</value>
        string ParameterTypes { get; }
    }
}
