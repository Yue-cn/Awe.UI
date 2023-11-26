using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Awe.UI.Converter
{
    public class CollectionContainConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values != null && values.Length is 2)
            {
                if (parameter is Type tdp)
                {
                    var mt = values[0].GetType().GetMethod("Contains");

                    if (mt is not null && mt.ReturnType == typeof(bool))
                    {
                        return mt.Invoke(values[0], new object[] { values[1] });
                    }
                }
                
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
