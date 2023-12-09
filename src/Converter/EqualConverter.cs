using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Awe.UI.Converter
{
    public class EqualConverter : IValueConverter
    {
        public object? TrueResult { get; set; } = default;
        public object? FalseResult { get; set; } = default;

        public EqualConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is null) return parameter is null ? TrueResult ?? true : FalseResult ?? false;

            if (parameter is "True" or "False" && bool.TryParse(parameter.ToString(), out var vl))
            {
                return value.Equals(vl) ? TrueResult ?? true : FalseResult ?? false;
            }
            else if (value is true or false)
            {
                if (targetType.Equals(typeof(System.Windows.Media.Brush)) && ((bool)value ? TrueResult : FalseResult) is System.Windows.Media.Color cl)
                {
                    return new System.Windows.Media.SolidColorBrush
                    {
                        Color = cl
                    };
                }
                return TrueResult ?? true;
            }
            else if (!value.Equals(parameter))
            {
                return FalseResult ?? false;
            }
            return TrueResult ?? true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
