using BuildnReleaseWP.Model;
using BuildnReleaseWP.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace BuildnReleaseWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LogPage : Page
    {
        StringKeyValuePair log;
        public LogPage()
        {
            this.InitializeComponent();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Utility.SetStatusBarColor("#212121", "#ffffff", 1);

            log = e.Parameter as StringKeyValuePair;
            logTB.Text = log.key;
            string logStr = "";
            ShowProgressBar();
            logSV.Visibility = Visibility.Collapsed;
            Task.Factory.StartNew(() =>
            {
                string logsUrl = log.value;
                if (logsUrl.ToLowerInvariant().Contains("/release/releases"))
                {
                    logStr = VSTSService.GetReleaseLogs(logsUrl);
                }
                else
                {
                    logStr = VSTSService.GetBuildTimelineRecordLogs(logsUrl);
                }
            }).ContinueWith(async (Task t) =>
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    logTextTB.Text = logStr;
                    HideProgressBar();

                    logSV.Visibility = Visibility.Visible;
                });
            });
        }

        private void ShowProgressBar()
        {
            progressG.Visibility = Visibility.Visible;
        }

        private void HideProgressBar()
        {
            progressG.Visibility = Visibility.Collapsed;
        }

        private void logBtn_Click(object sender, RoutedEventArgs e)
        {
            if(Frame != null && Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void increaseFontSizeABB_Click(object sender, RoutedEventArgs e)
        {
            if(logTextTB.FontSize < 30)
            {
                logTextTB.FontSize += 2.0;
            }
        }

        private void decreaseFontSizeABB_Click(object sender, RoutedEventArgs e)
        {
            if (logTextTB.FontSize > 8)
            {
                logTextTB.FontSize -= 2.0;
            }
        }
    }
}
