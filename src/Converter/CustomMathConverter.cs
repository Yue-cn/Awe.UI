using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Awe.UI.Converter
{
    public class CustomMathConverter : IValueConverter
    {
        public Func<object,object> Calc { get; set; } = delegate { return -1; };

        public CustomMathConverter() { }

        public CustomMathConverter(Func<object,object> calc) { this.Calc = calc; }
        

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (Calc is not null)
            {
                return Calc(value);
            }
            return -1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
