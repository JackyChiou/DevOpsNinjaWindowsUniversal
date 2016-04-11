using BuildnReleaseWP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class ApprovalToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string approvalImageUrl = "/Images/ic_approval_gray.png";
            string status = (string)value;

            if(!String.IsNullOrWhiteSpace(status))
            {
                if(status.ToLowerInvariant().Equals("pending")) 
                {
                    approvalImageUrl = "/Images/ic_approval_blue.png";
                }
                else if (status.ToLowerInvariant().Equals("rejected"))
                {
                    approvalImageUrl = "/Images/ic_approval_red.png";
                }
                else if (status.ToLowerInvariant().Equals("approved"))
                {
                    approvalImageUrl = "/Images/ic_approval_green.png";
                }
            }

            return approvalImageUrl;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
