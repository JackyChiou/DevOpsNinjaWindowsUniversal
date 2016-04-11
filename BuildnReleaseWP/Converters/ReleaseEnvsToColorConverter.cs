using BuildnReleaseWP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class ReleaseEnvsToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            List<Release.REnvironment> envs = value as List<Release.REnvironment>;
            if (envs != null && envs.Count > 0)
            {
                string indexStr = (string)parameter;

                if (!String.IsNullOrWhiteSpace(indexStr))
                {
                    int index = 0;
                    Int32.TryParse(indexStr, out index);

                    if (index < envs.Count && index < 6)
                    {
                        string eStatus = envs.ElementAt<Release.REnvironment>(index).Status;
                        return getStatusColor(eStatus);
                    }
                    else if(index >= 6 & envs.Count > 6)
                    {
                        int res =envs.Count - 6;
                        return "  " + res + " >";
                    }
                    else
                    {
                        return "";
                    }
                }
            }

            return "#ffffffff";
        }

        private string getStatusColor(string eStatus)
        {
            if (eStatus.ToLowerInvariant().Equals("succeeded")) return "#4CAF50";
            else if (eStatus.ToLowerInvariant().Equals("inprogress")) return "#0070c0";
            else if (eStatus.ToLowerInvariant().Equals("rejected")) return "#D70D24";
            else if (eStatus.ToLowerInvariant().Equals("failed")) return "#D70D24";
            else if (eStatus.ToLowerInvariant().Equals("cancelled")) return "#D70D24";

            return "#9E9E9E";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
