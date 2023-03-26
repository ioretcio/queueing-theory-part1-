using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace queueing_theory
{
    class Convert
    {
        public static double toDouble(string source)
        {
            try { return Convert.toDouble(source.Replace('.', ',')); }
            catch { return Convert.toDouble(source.Replace(',', '.')); }
        }
    }
}
