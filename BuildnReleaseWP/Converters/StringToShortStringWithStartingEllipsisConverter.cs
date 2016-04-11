using BuildnReleaseWP.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class StringToShortStringWithStartingEllipsisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string str = (string)value;
            int len = 45;
            Int32.TryParse((string)parameter, out len);

            return Utility.GetShortDisplayNameWithstartingElipsis(str, len);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
