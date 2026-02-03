#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;

namespace Oberon0.Compiler.Generator;

public class ProcessOutputReceivedEventArgs(CreateBinaryOptions options, string data) : EventArgs
{
    public CreateBinaryOptions Options { get; } = options;

    public string Data { get; } = data;
}
