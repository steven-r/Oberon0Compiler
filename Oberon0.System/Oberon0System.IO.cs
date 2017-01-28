using System;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;
using Oberon0.Attributes;

namespace Oberon0
{
    [Oberon0Library]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static partial class Oberon0System
    {
        /// <summary>
        /// Check if there's no more data available on standard input
        /// </summary>
        /// <returns><c>true</c> if end of file reached, <c>false</c> otherwise.</returns>
        [UsedImplicitly]
        [Oberon0Export("eot", "BOOLEAN")]
        public static bool eot()
        {
            return System.Console.In.Peek() >= 0;
        }
    }
}
