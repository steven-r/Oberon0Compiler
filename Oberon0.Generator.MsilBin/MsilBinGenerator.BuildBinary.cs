#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Shared;

namespace Oberon0.Generator.MsilBin
{
    public partial class MsilBinGenerator
    {
        /// <inheritdoc />
        public bool GenerateBinary(CreateBinaryOptions options = null)
        {
            var binary = new CreateBinary(this, options);
            return binary.Execute();
        }
    }
}
