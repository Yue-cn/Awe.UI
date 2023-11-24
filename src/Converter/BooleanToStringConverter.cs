using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Awe.UI.Converter
{
    public class BooleanToStringConverter : IValueConverter
    {

        public string DefaultTrueString { get; set; } = "DefaultTrueString";
        public string DefaultFalseString { get; set; } = "DefaultFalseString";

        public BooleanToStringConverter() { }

        public BooleanToStringConverter(string defaultTrueString,string defaultFalseString)
        {
            this.DefaultTrueString = defaultTrueString;
            this.DefaultFalseString = defaultFalseString;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is true)
            {
                return DefaultTrueString;
            }
            return DefaultFalseString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
