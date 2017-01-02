using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oberon0.Attributes;

namespace Oberon0
{
    public static partial class Oberon0System
    {
        [Oberon0Export("ToString", "STRING", "INTEGER")]
        public static string ConvertToString(int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        [Oberon0Export("ToString", "STRING", "REAL")]
        public static string ConvertToString(double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

        [Oberon0Export("ToString", "STRING", "BOOLEAN")]
        public static string ConvertToString(bool value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }

    }
}
