﻿
using System.Globalization;
namespace queueing_theory
{
    class Convert
    {
        public static double toDouble(object value)
        {
            NumberFormatInfo nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            return double.Parse(value.ToString(), nfi);
        }
    }
}
