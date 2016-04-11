using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class BuildResultToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = (string)value;

            if(result.ToLowerInvariant().Equals("succeeded"))
            {
                return "/Images/ic_build_green.png";
            } else if(result.ToLowerInvariant().Equals("failed"))
            {
                return "/Images/ic_build_red.png";
            } else if(result.ToLowerInvariant().Equals("canceled"))
            {
                return "/Images/ic_build_red.png";
            }
            else if(result.ToLowerInvariant().Equals("partiallysucceeded"))
            {
                return "/Images/ic_build_yellow.png";
            }

            return "/Images/ic_build_gray.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
