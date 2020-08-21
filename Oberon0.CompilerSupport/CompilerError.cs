#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using System;
using System.Diagnostics.CodeAnalysis;

namespace Oberon0.TestSupport
{
    [ExcludeFromCodeCoverage]
    public class CompilerError
    {
        public int Column { get; set; }

        public Exception Exception { get; set; }

        public int Line { get; set; }

        public string Message { get; set; }
    }
}
