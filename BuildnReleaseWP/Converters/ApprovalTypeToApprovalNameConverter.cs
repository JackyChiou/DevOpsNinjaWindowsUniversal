using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class ApprovalTypeToApprovalNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string result = (string)value;

            if (result.ToLowerInvariant().Equals("predeploy"))
            {
                return "PRE-DEPLOY";
            }
            else if (result.ToLowerInvariant().Equals("postdeploy"))
            {
                return "POST-DEPLOY";
            }

            return "UNKNOWN";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
