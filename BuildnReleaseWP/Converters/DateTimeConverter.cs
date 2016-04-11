using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    // Usage: ddd dd MMM hh:mm:ss
    class DateTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if(value != null)
            {
                DateTime dt = (DateTime) value;
                //DateTime dtdt = new DateTime();
                //DateTime.TryParse(dt, out dt);
                string format = (string)parameter;
                return dt.ToString(format);
            }
            return "NA";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
