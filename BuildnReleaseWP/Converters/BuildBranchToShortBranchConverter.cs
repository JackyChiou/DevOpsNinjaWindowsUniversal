using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Data;

namespace BuildnReleaseWP.Converters
{
    class BuildBranchToShortBranchConverter : IValueConverter
    {
        private string BRANCH_REF_PART = "refs/heads/";

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string branch = (string)value;
            

            if (branch.Contains(BRANCH_REF_PART))
            {
                int lastIndexOfRefsPart = branch.IndexOf(BRANCH_REF_PART);
                branch = branch.Substring(lastIndexOfRefsPart + BRANCH_REF_PART.Length);
            }

            return branch;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
