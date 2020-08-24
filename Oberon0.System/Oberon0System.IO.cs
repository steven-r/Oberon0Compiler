#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using System;
using JetBrains.Annotations;
using Oberon0System.Attributes;

namespace Oberon0System
{
    /// <summary>
    ///     The oberon 0 system library.
    /// </summary>
    [Oberon0Library]
    public static partial class Oberon0System
    {
        /// <summary>
        ///     Check if there's no more data available on standard input
        /// </summary>
        /// <returns><c>true</c> if end of file reached, <c>false</c> otherwise.</returns>
        [UsedImplicitly]
        [Oberon0Export("eot", "BOOLEAN")]
        // ReSharper disable once InconsistentNaming
#pragma warning disable IDE1006 // Naming Styles
        public static bool eot()
#pragma warning restore IDE1006 // Naming Styles
        {
            return Console.In.Peek() < 0;
        }
    }
}
