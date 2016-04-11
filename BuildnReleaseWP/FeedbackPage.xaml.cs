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
    public sealed partial class FeedbackPage : Page
    {
        public FeedbackPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SetDefaultUI();
        }

        private void SetDefaultUI()
        {
            Service.Utility.SetStatusBarColor("#005DA2", "#ffffff", 1);
        }

        private async void fbBtn_Click(object sender, RoutedEventArgs e)
        {
            string urlStr = "https://www.facebook.com/devopsninjapro";
            Uri mailtoUri = new Uri(urlStr);

            await Launcher.LaunchUriAsync(mailtoUri);
        }

        private void twitterBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void mailrBtn_Click(object sender, RoutedEventArgs e)
        {
            string recipient = "openalm@outlook.com";
            string subject = "[DevOps Ninja] Feedback / Comments";
            string body = "<Put your text here>";

            Uri mailtoUri = new Uri("mailto:" + recipient + "?subject=" + subject +"&body=" + body);

            await Launcher.LaunchUriAsync(mailtoUri);
        }

        private void serviceBtn_Click(object sender, RoutedEventArgs e)
        {
            if(sender != null && Frame != null && Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void restAPIHB_Click(object sender, RoutedEventArgs e)
        {
            string urlStr = "https://www.visualstudio.com/integrate/api/overview";
            Uri mailtoUri = new Uri(urlStr);

            await Launcher.LaunchUriAsync(mailtoUri);
        }

        private async void bugBtn_Click(object sender, RoutedEventArgs e)
        {

            string recipient = "openalm@outlook.com";
            string subject = "[DevOps Ninja] Bug";
            string body = "<Report you bug here>";

            Uri mailtoUri = new Uri("mailto:" + recipient + "?subject=" + subject + "&body=" + body);

            await Launcher.LaunchUriAsync(mailtoUri);

        }

        private void shareBtn_Click(object sender, RoutedEventArgs e)
        {
            //ShareLinkTask slt = new ShareLinkTask();
            //slt.Title = "You message title here";
            //slt.Message = "Your message goes here";
            //slt.LinkUri = new Uri("http://HereComesTheLinkThatWillShowInYourPost.com", UriKind.Absolute);
            //slt.Show();
        }
    }
}
