#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;

namespace Oberon0.Shared
{
    public class ProcessOutputReceivedEventArgs : EventArgs
    {
        public ProcessOutputReceivedEventArgs(CreateBinaryOptions options, string data)
        {
            Options = options;
            Data = data;
        }

        public CreateBinaryOptions Options { get; }

        public string Data { get; }
    }
}
