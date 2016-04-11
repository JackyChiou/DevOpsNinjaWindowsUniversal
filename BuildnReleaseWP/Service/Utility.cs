using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace BuildnReleaseWP.Service
{
    class Utility
    {
        public static async void ShowMsg(string p)
        {
            MessageDialog msg = new MessageDialog(p);
            await msg.ShowAsync();
        }

        public static string GetShortDisplayNameWithstartingElipsis(string str, int maxLen)
        {
            if (str.Length > maxLen)
            {
                int startIndex = str.Length - maxLen;
                str = str.Substring(startIndex, maxLen);
                str = "..." + str;
            }
            return str;
        }
        public static Color GetColorFromHexa(string hexaColor)
        {
            byte R = Convert.ToByte(hexaColor.Substring(1, 2), 16);
            byte G = Convert.ToByte(hexaColor.Substring(3, 2), 16);
            byte B = Convert.ToByte(hexaColor.Substring(5, 2), 16);
            return Color.FromArgb(0xFF, R, G, B);
        }

        public static SolidColorBrush GetSolidColorBrushFromHexa(string hexaColor)
        {
            byte R = Convert.ToByte(hexaColor.Substring(1, 2), 16);
            byte G = Convert.ToByte(hexaColor.Substring(3, 2), 16);
            byte B = Convert.ToByte(hexaColor.Substring(5, 2), 16);
            SolidColorBrush scb = new SolidColorBrush(Color.FromArgb(0xFF, R, G, B));
            return scb;
        }

        public static void ResetAllData()
        {
            ReleasesPage.ResetData();
            ReleaseDetailsPage.ResetData();
            ReleaseCreatePage.ResetData();
            RD_ReleasesPage.ResetData();

            BuildsPage.ResetData();
            BD_BuildsPage.ResetData();
            BuildDetailsPage.ResetData();

            ApprovalsPage.ResetData();
            ApprovalDetailsPage.ResetData();
        }

        public static void ShowToastMessage(string title, string detail)
        {
            var toastXmlContent = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);

            var txtNodes = toastXmlContent.GetElementsByTagName("text");
            txtNodes[0].AppendChild(toastXmlContent.CreateTextNode(title));
            txtNodes[1].AppendChild(toastXmlContent.CreateTextNode(detail));

            var toast = new ToastNotification(toastXmlContent);
            toast.ExpirationTime = DateTimeOffset.UtcNow.AddSeconds(3600);
            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            toastNotifier.Show(toast);
        }

        public static void SetStatusBarColor(string bgColor, string fgColor, double opacity)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                //await statusBar.HideAsync();
                statusBar.BackgroundColor = GetColorFromHexa(bgColor);
                statusBar.ForegroundColor = GetColorFromHexa(fgColor);
                statusBar.BackgroundOpacity = opacity;
            }
        }

        private async void HideStatusBar()
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
                await statusBar.HideAsync();
                //statusBar.BackgroundColor = Windows.UI.Colors.Green;
                //statusBar.BackgroundOpacity = 1;
            }
        }

        public static bool isNumber(string num)
        {
            if (num.Equals(String.Empty))
            {
                return false;
            }

            return true;
        }

        public static string GetShortFormattedDateStr(DateTime dt)
        {
            if (dt == null || dt.Equals(DateTime.MinValue)) return "";
            return dt.ToString("ddd dd MMM");
        }

        public static string makeStrNChars(string text, int n)
        {
            string outText = text;
            int len = text.Length;

            if (len > n)
            {
                outText = text.Substring(0, n - 3);
                outText = String.Concat(outText + "...");
            }

            return outText;
        }

        internal static async void ShowUpgradeMsg(string v)
        {
            MessageDialog msg = new MessageDialog(v);
            await msg.ShowAsync();
        }
    }
}
