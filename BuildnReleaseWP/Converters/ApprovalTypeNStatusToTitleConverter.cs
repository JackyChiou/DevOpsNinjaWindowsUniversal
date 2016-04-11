using BuildnReleaseWP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class ApprovalTypeNStatusToTitleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {

            string type = (string)value;

            string status = (string)parameter;

            string title = "";


            if (!String.IsNullOrWhiteSpace(type))
            {
                if(type.ToLowerInvariant().Equals("predeploy")) 
                {
                    title = "Pre-Deploy" + "approval" + status.ToLowerInvariant();
                }
                else if (type.ToLowerInvariant().Equals("postdeploy"))
                {
                    title = "Post-Deploy" + "approval" + status.ToLowerInvariant();
                }
                else 
                {
                    title ="Approval" + status.ToLowerInvariant();
                }
            }

            return title;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
