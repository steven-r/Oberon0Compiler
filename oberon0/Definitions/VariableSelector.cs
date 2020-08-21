#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using System.Collections.Generic;
using Oberon0.Compiler.Types;

namespace Oberon0.Compiler.Definitions
{
    /// <summary>
    ///     hold selector lists
    /// </summary>
    public class VariableSelector : List<BaseSelectorElement>
    {
        public VariableSelector(TypeDefinition resultType)
        {
            SelectorResultType = resultType;
        }

        public TypeDefinition SelectorResultType { get; set; }
    }
}
