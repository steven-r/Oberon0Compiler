#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Oberon0System.IO.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.System/Oberon0System.IO.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0
{
    using System;

    using JetBrains.Annotations;

    using Oberon0.Attributes;

    /// <summary>
    /// The oberon 0 system library.
    /// </summary>
    [Oberon0Library]
    public static partial class Oberon0System
    {
        /// <summary>
        /// Check if there's no more data available on standard input
        /// </summary>
        /// <returns><c>true</c> if end of file reached, <c>false</c> otherwise.</returns>
        [UsedImplicitly]
        [Oberon0Export("eot", "BOOLEAN")]
        // ReSharper disable once InconsistentNaming
        public static bool eot()
        {
            return Console.In.Peek() < 0;
        }
    }
}