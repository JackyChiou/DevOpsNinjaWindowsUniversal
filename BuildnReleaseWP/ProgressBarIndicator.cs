using System;
using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;

namespace BuildnReleaseWP
{
    public class ProgressBarIndicator
    {
        public static async Task ExecuteActionWithProgressBar(Action executeAction)
        {
            var startTime = DateTime.Now.Ticks;
            //await ShowProgressBar();
            executeAction();
            //await HideProgressBar();
            var endTime = DateTime.Now.Ticks;
            var timeDiff = endTime - startTime;
        }

        public static void ExecuteActionWithProgressBar(Action executeAction, Grid p)
        {
            var startTime = DateTime.Now.Ticks;
            ShowProgressRing(p);
            executeAction();
            HideProgressRing(p);
            var endTime = DateTime.Now.Ticks;
            var timeDiff = endTime - startTime;
        }

        public static void HideProgressRing(Grid p)
        {
            p.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
        }

        public static void ShowProgressRing(Grid p)
        {
            p.Visibility = Windows.UI.Xaml.Visibility.Visible;
        }

        //private static async Task<TitleBar> HideProgressBar()
        //{

        //    var statusBar = ApplicationView.GetForCurrentView().TitleBar;
        //    //await statusBar.ProgressIndicator.HideAsync();
        //    //await statusBar.HideAsync();
        //    return statusBar;
        //}

        //private static async Task<StatusBar> ShowProgressBar()
        //{
        //    var statusBar = StatusBar.GetForCurrentView();
        //    await statusBar.ShowAsync();
        //    await statusBar.ProgressIndicator.ShowAsync();
        //    return statusBar;
        //}
    }
}