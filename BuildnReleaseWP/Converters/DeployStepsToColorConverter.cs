using BuildnReleaseWP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class DeployStepsToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string color = "#9E9E9E";
            Release.DeployStep deploySteps = (Release.DeployStep)value;

            if (deploySteps != null) {

                if(deploySteps.Job != null)
                color = getStatusColor(deploySteps.Job.Status);
               
            }

            return color;
        }

        private string getStatusColor(string result)
        {
            string color = "#9E9E9E";

            if (result.ToLowerInvariant().Equals("succeeded")) return "#4CAF50";
            if (result.ToLowerInvariant().Equals("success")) return "#4CAF50";
            if (result.ToLowerInvariant().Equals("inprogress")) return "#03A9F4";
            if (result.ToLowerInvariant().Equals("active")) return "#03A9F4";
            if (result.ToLowerInvariant().Equals("rejected")) return "#F44336";
            if (result.ToLowerInvariant().Equals("failed")) return "#F44336";
            if (result.ToLowerInvariant().Equals("failure")) return "#F44336";
            if (result.ToLowerInvariant().Equals("cancelled")) return "#F44336";

            return color;
            throw new NotImplementedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
