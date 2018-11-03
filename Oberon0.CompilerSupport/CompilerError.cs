#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CompilerError.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.TestSupport/CompilerError.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0.TestSupport
{
    using System;

    public class CompilerError
    {
        public int Column { get; set; }

        public Exception Exception { get; set; }

        public int Line { get; set; }

        public string Message { get; set; }
    }
}