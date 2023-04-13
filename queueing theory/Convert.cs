
using System.Globalization;
namespace queueing_theory
{
    class Convert
    {
        public static double toDouble(object value) //універсальний конвертер
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            return double.Parse(value.ToString().Replace(',','.'), nfi);
        }
    }
}
