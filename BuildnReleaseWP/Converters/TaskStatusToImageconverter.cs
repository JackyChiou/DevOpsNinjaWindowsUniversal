using BuildnReleaseWP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class TaskStatusToImageconverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string statusImage = "/Images/ic_unchecked.png";
            
            string status = (string)value;

            if(!String.IsNullOrWhiteSpace(status))
            {
                status = status.ToLowerInvariant();
                if (status.Equals("success") || status.Equals("succeeded") || status.Equals("done") || status.Equals("completed") || status.ToLowerInvariant().Equals("succeededwithissues")) return "/Images/ic_check.png";
                if (status.Equals("failure") || status.Equals("failed") || status.Equals("rejected") || status.Equals("abandoned")) return "/Images/ic_cross.png";
                if (status.Equals("cancelled")) return "/Images/ic_cancel.png";
                if (status.Equals("inprogress")) return "/Images/ic_play.png";
                if (status.Equals("skipped")) return "/Images/ic_cancel.png";
                if (status.Equals("notstarted") || status.Equals("pending")) return "/Images/ic_unchecked.png";
            }

            return statusImage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
