using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class DateTimeDiffConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value != null && parameter != null)
            {
                DateTime ft = (DateTime)value;
                DateTime st = (DateTime)parameter;
               
                double duration = ft.Subtract(st).TotalMinutes;

                return duration.ToString("F2") + " mins";
            }
            return " - ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
