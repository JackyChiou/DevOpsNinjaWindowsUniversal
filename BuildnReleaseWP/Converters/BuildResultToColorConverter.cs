using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class BuildResultToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = (string)value;

            if (result.ToLowerInvariant().Equals("succeeded"))
            {
                return "#4CAF50";
            }
            else if (result.ToLowerInvariant().Equals("failed"))
            {
                return "#F44336";
            }
            else if (result.ToLowerInvariant().Equals("canceled"))
            {
                return "#F44336";
            }
            else if (result.ToLowerInvariant().Equals("partiallysucceeded"))
            {
                return "#FFC107";
            }
            else if (result.ToLowerInvariant().Equals("inprogress"))
            {
                return "#3F51B5";
            }
            else if (result.ToLowerInvariant().Equals("notstarted"))
            {
                return "#9E9E9E";
            }

            return "#9E9E9E";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
