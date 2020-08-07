#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Definitions;
using Oberon0.Compiler.Generator;

namespace Oberon0.Generator.MsilBin
{
    internal class DeclarationGeneratorInfo : IGeneratorInfo
    {
        private int v;

        public DeclarationGeneratorInfo(int v)
        {
            this.v = v;
        }

        public ProcedureParameterDeclaration OriginalField { get; internal set; }
        public Declaration ReplacedBy { get; internal set; }
    }
}