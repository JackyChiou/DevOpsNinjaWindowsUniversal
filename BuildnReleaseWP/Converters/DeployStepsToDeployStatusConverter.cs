using BuildnReleaseWP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class DeployStepsToDeployStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string status = "NOT-STARTED";
            Release.DeployStep deploySteps = (Release.DeployStep)value;

            if (deploySteps != null) {

                if (deploySteps.Job != null)
                {
                    status = deploySteps.Job.Status.ToUpperInvariant();
                }


            }

            return status;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
