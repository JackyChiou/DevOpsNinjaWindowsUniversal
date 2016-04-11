using BuildnReleaseWP.Context;
using BuildnReleaseWP.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    public sealed partial class ServiceSelectionPage : Page
    {
        public ServiceSelectionPage()
        {
            this.InitializeComponent();
            
            projectTB.Text = ProjectContext.GetProjectContext().Project;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SetDefaultUI();
        }

        private void SetDefaultUI()
        {
            Utility.SetStatusBarColor("#005DA2", "#ffffff", 1);
        }

        private void approvalBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender != null && Frame != null)
            {
                Frame.Navigate(typeof(ApprovalsPage));
            }
        }

        private void AppBarButton_Click(object sender, RoutedEventArgs e)
        {

        }


        private void projectBtn_Click(object sender, RoutedEventArgs e)
        {
            if(sender != null && Frame != null)
            {
                Frame.Navigate(typeof(ProjectSelectionPage));
            }
        }

        private void buildBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && Frame != null)
            {
                Frame.Navigate(typeof(BuildsPage));
            }
        }

        private void releaseBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && Frame != null)
            {
                Frame.Navigate(typeof(ReleasesPage));
            };
        }

        private void approvalBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && Frame != null)
            {
                Frame.Navigate(typeof(ApprovalsPage));
            }
        }

        private void logoutABB_Click(object sender, RoutedEventArgs e)
        {

        }
        private void likeDislikeABB_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && Frame != null)
            {
                Frame.Navigate(typeof(FeedbackPage));
            }
        }
        private void homeABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void settingsABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void bugABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void infoABB_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && Frame != null)
            {
                Frame.Navigate(typeof(FeedbackPage));
            }

        }

        private async void fbABB_Click(object sender, RoutedEventArgs e)
        {
            string urlStr = "https://www.facebook.com/devopsninjapro";
            Uri mailtoUri = new Uri(urlStr);

            await Launcher.LaunchUriAsync(mailtoUri);
        }
    }
}
