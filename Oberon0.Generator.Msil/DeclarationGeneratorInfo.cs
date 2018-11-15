#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeclarationGeneratorInfo.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.Generator.Msil/DeclarationGeneratorInfo.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.Generator.Msil
{
    using System.Linq;

    using Oberon0.Compiler.Definitions;
    using Oberon0.Compiler.Generator;
    using Oberon0.Compiler.Types;

    internal class DeclarationGeneratorInfo : IGeneratorInfo
    {
        public DeclarationGeneratorInfo(int offset)
        {
            this.Offset = offset;
        }

        /// <summary>
        /// Gets the offset.
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Gets or sets the original field.
        /// </summary>
        public Declaration OriginalField { get; set; }

        /// <summary>
        /// Gets or sets the replaced by field.
        /// </summary>
        public Declaration ReplacedBy { get; set; }

        public bool IsVar(Declaration declaration, VariableSelector selector)
        {
            if (selector != null)
            {
                return selector.LastOrDefault()?.TypeDefinition.Type.HasFlag(BaseTypes.Simple) ?? false;
            }

            return declaration.Type.Type.HasFlag(BaseTypes.Simple);
        }
    }
}