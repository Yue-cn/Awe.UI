using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Awe.UI.Converter
{
    public class DivisionConverter : IValueConverter
    {
        public int DivideWith { get; set; } = 1;
        public int Digits { get; set; } = 2;

        public DivisionConverter() { }
        public DivisionConverter(int dw,int dg) { this.DivideWith = dw; this.Digits = dg; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double it)
            {
                return Math.Round(it / DivideWith,Digits);
            }
            else if (value is long dt)
            {
                if (dt <= double.MaxValue)
                {
                    return Math.Round((double)dt / DivideWith, Digits);
                }
            }
            else if (value is int it2)
            {
                return Math.Round((double)it2 / DivideWith, Digits);
            }
            return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
