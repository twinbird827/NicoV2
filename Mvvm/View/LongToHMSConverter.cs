using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NicoV2.Mvvm.View
{
    public class LongToHMSConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            long l;
            if (value is double)
            {
                l = (long)((double)value);
            }
            else if (value is long)
            {
                l = (long)value;
            }
            else if (!long.TryParse(value.ToString(), out l))
            {
                throw new ArgumentException("not long", "value");
            }

            TimeSpan ts = new TimeSpan(l * 100 * 100 * 1000);
            return ts.ToString(@"hh\:mm\:ss");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
