using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class TaskStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string color = "#ee434343";
            string result = (string)value;

            if (result != null) {

                color = getStatusColor(result);
               
            }

            return color;
        }

        private string getStatusColor(string result)
        {
            string color = "#ee9e9e9e";

            if (result.ToLowerInvariant().Equals("succeeded")) return "#ee4CAF50";
            if (result.ToLowerInvariant().Equals("success")) return "#ee4CAF50";
            if (result.ToLowerInvariant().Equals("inprogress")) return "#ee03A9F4";
            if (result.ToLowerInvariant().Equals("active")) return "#ee03A9F4";
            if (result.ToLowerInvariant().Equals("rejected")) return "#eeF44336";
            if (result.ToLowerInvariant().Equals("failed")) return "#eeF44336";
            if (result.ToLowerInvariant().Equals("failure")) return "#eeF44336";
            if (result.ToLowerInvariant().Equals("cancelled")) return "#eeF44336";
            if (result.ToLowerInvariant().Equals("abandoned")) return "#eeF44336";
            if (result.ToLowerInvariant().Equals("succeededwithissues")) return "#eeFFC107";
            if (result.ToLowerInvariant().Equals("skipped")) return "#ee9E9E9E";

            return color;
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
