using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class DurationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = " - ";
            if(value != null && parameter != null)
            {
                double duration = (double)value;
                string timeFormat = (string)parameter;
                if( duration >= 0.00  && timeFormat.Equals("m"))
                {
                    result = duration.ToString("F", CultureInfo.InvariantCulture);
                }
            }
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
