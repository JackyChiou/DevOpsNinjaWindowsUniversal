using BuildnReleaseWP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class ApprovalsToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string approvalImageUrl = "/Images/release/ic_approval_gray.png";
            List<Approval> approvalsList = (List<Approval>)value;

            if(approvalsList != null && approvalsList.Count > 0)
            {
                if(approvalsList.Find(x => x.Status.ToLowerInvariant().Equals("pending")) != null)
                {
                    approvalImageUrl = "/Images/release/ic_approval_blue.png";
                }
                else if (approvalsList.Find(x => x.Status.ToLowerInvariant().Equals("rejected"))!= null)
                {
                    approvalImageUrl = "/Images/release/ic_approval_red.png";
                }
                else if (approvalsList.Find(x => x.Status.ToLowerInvariant().Equals("approved")) != null)
                {
                    approvalImageUrl = "/Images/release/ic_approval_green.png";
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
