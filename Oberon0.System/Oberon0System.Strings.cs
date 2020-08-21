#region copyright

// --------------------------------------------------------------------------------------------------------------------
// Copyright (c) Stephen Reindl. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// --------------------------------------------------------------------------------------------------------------------

#endregion

using System.Globalization;
using JetBrains.Annotations;
using Oberon0System.Attributes;

namespace Oberon0System
{
    public static partial class Oberon0System
    {
        /// <summary>
        ///     Convert INTEGER to STRING.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>the converted string</returns>
        [Oberon0Export("ToString", "STRING", "INTEGER")]
        [UsedImplicitly]
        public static string ConvertToString(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Convert REAL to STRING.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>the converted string</returns>
        [Oberon0Export("ToString", "STRING", "REAL")]
        [UsedImplicitly]
        public static string ConvertToString(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        ///     Convert BOOLEAN to STRING.
        /// </summary>
        /// <param name="value">the value to convert</param>
        /// <returns>the converted string</returns>
        [Oberon0Export("ToString", "STRING", "BOOLEAN")]
        [UsedImplicitly]
        public static string ConvertToString(bool value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
