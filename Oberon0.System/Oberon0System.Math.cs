#region copyright
// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------
#endregion

using JetBrains.Annotations;
using Oberon0System.Attributes;

namespace Oberon0System
{
    /// <summary>
    /// The oberon 0 system math.
    /// </summary>
    public static partial class Oberon0System
    {
        /// <summary>
        /// Check if given double represents "infinity" (see <see cref="double.IsInfinity"/>)
        /// </summary>
        /// <param name="d">The value to check</param>
        /// <returns>the result of <see cref="double.IsInfinity"/>.</returns>
        [UsedImplicitly]
        // ReSharper disable once StringLiteralTypo
        [Oberon0Export("isinfinity", "BOOLEAN", "REAL")]
        public static bool IsInfinity(double d)
        {
            return double.IsInfinity(d);
        }
    }
}