#region copyright
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Oberon0System.Math.cs" company="Stephen Reindl">
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT license. See LICENSE.md file in the project root for full license information.
// </copyright>
// <summary>
//     Part of oberon0 - Oberon0.System/Oberon0System.Math.cs
// </summary>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace Oberon0
{
    using JetBrains.Annotations;

    using Oberon0.Attributes;

    /// <summary>
    /// The oberon 0 system math.
    /// </summary>
    public static partial class Oberon0System
    {
        [UsedImplicitly]
        [Oberon0Export("isinfinity", "BOOLEAN", "REAL")]
        public static bool IsInfinity(double d)
        {
            return double.IsInfinity(d);
        }
    }
}