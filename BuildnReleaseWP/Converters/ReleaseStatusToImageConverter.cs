using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class ReleaseStatusToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = (string)value;

            if(result.ToLowerInvariant().Equals("active"))
            {
                return "/Images/ic_release_purple.png";
            } 
            else if(result.ToLowerInvariant().Equals("abandoned"))
            {
                return "/Images/ic_release_red.png";
            }

            return "/Images/ic_release_gray.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
