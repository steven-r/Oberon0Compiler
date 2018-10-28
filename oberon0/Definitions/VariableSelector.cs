#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VariableSelector.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0Compiler/VariableSelector.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Compiler.Definitions
{
    using System.Collections.Generic;

    using Oberon0.Compiler.Types;

    /// <summary>
    /// hold selector lists
    /// </summary>
    public class VariableSelector : List<BaseSelectorElement>
    {
        public TypeDefinition SelectorResultType { get; set; }
    }
}