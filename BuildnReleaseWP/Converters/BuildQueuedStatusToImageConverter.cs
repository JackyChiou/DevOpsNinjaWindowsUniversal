using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class BuildQueuedStatusToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string status = (string)value;

            if (status.ToLowerInvariant().Equals("notstarted"))
            {
                return "/Images/ic_build_gray.png";
            }
            else if (status.ToLowerInvariant().Equals("inprogress"))
            {
                return "/Images/ic_build_blue_striped.png";
            }

            return "/Images/ic_build_gray.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
