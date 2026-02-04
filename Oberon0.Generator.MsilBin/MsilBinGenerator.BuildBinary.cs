#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using Oberon0.Compiler.Generator;
using Oberon0.Generator.MsilBin.GeneratorInfo;

namespace Oberon0.Generator.MsilBin;

public partial class MsilBinGenerator
{
    /// <inheritdoc />
    public bool GenerateBinary(CreateBinaryOptionsBase? options = null)
    {
        var binary = new CreateBinary(this, options as MsilCreateBinaryOptions);
        return binary.Execute();
    }
}
