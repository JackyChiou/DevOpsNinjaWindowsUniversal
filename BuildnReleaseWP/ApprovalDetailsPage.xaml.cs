using BuildnReleaseWP.Context;
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
using Windows.UI.Popups;
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
    public sealed partial class ApprovalDetailsPage : Page
    {
        Approval approval;
        public ApprovalDetailsPage()
        {
            this.InitializeComponent();
        }

        private void SetDefaultUI()
        {
            Utility.SetStatusBarColor("#B71C1C", "#ffffff", 1);
            if(LoginContext.GetLoginContext().Pro)
            {
                noProCommentsTB.Visibility = Visibility.Collapsed;
            }
            else
            {
                noProCommentsTB.Visibility = Visibility.Visible;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e == null) return;

            approval = (Approval)e.Parameter;

            if (approval == null) return;

            SetDefaultUI();

            headerTB.Text = approval.ReleaseEnvironment.Name + " ( " + approval.Release.Name +" )";

            if (!approval.Status.ToLowerInvariant().Equals("pending"))
            {
                noProCommentsTB.Visibility = Visibility.Collapsed;
            }

            approvalCardSP.DataContext = approval;
        }

        private void reassignABB_Click(object sender, RoutedEventArgs e)
        {

        }
        
        private void ShowProgressBar()
        {
            progressG.Visibility = Visibility.Visible;
        }

        internal static void ResetData()
        {

        }

        private void HideProgressBar()
        {
            progressG.Visibility = Visibility.Collapsed;
        }

        private void approveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(sender == null)
            {
                return;
            }

            string comments = getApprovalComments();
            string status = "approved";


            ShowProgressBar();
            Approval responseApproval;
            Task.Factory.StartNew(() =>
            {
                responseApproval = VSTSService.PatchApproval(approval, status, comments);

                if (responseApproval != null)
                {
                    approval = responseApproval;
                }
            }).ContinueWith(async (Task t) =>
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    approvalCardSP.DataContext = approval;
                   
                    if (!approval.Status.ToLowerInvariant().Equals("pending"))
                    {
                        noProCommentsTB.Visibility = Visibility.Collapsed;
                    }
                    HideProgressBar();
                });
            });


        }

        private string getApprovalComments()
        {
            string commentsStr = commentsTB.Text;
            if(!LoginContext.GetLoginContext().Pro)
            {
                commentsStr = commentsStr + noProCommentsTB.Text;
            }

            return commentsStr;
        }

        private void rejectBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            string comments = getApprovalComments();
            string status = "rejected";

            ShowProgressBar();
            Approval responseApproval;
            Task.Factory.StartNew(() =>
            {
                responseApproval = VSTSService.PatchApproval(approval, status, comments);

                if (responseApproval != null)
                {
                    approval = responseApproval;
                }
            }).ContinueWith(async (Task t) =>
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    approvalCardSP.DataContext = approval;
                    HideProgressBar();
                });
            });
        }

        private void headerBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Frame != null && Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private async Task HandleUpgradeScenario(string title, string msg)
        {
            MessageDialog md = new MessageDialog(msg);
            md.Title = title;
            md.Commands.Add(new UICommand("Buy Pro!", new UICommandInvokedHandler(CommandHandlers)));
            md.Commands.Add(new UICommand("Not now", new UICommandInvokedHandler(CommandHandlers)));

            await md.ShowAsync();
        }

        private async void CommandHandlers(IUICommand commandLabel)
        {
            string appid = "dd889a2a-9d1f-4942-a1bd-17f7f6c49a00";
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                //Okay Button.
                case "Buy Pro!":
                    this.Focus(Windows.UI.Xaml.FocusState.Pointer);
                    var uri = new Uri(string.Format("ms-windows-store:navigate?appid={0}", appid));
                    await Windows.System.Launcher.LaunchUriAsync(uri);
                    break;
                //Quit Button.
                case "Not now":
                    break;
                    //end.
            }
        }

        private void noProCommentsTB_Tapped(object sender, TappedRoutedEventArgs e)
        {
            HandleUpgradeScenario("Buy Pro version", "You cannot change the comments with the free version. Buy DevOps Ninja Pro to unlock the full capability of the app?");
        }

        private void noProCommentsTB_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            HandleUpgradeScenario("Buy Pro version", "You cannot change the comments with the free version. Buy DevOps Ninja Pro to unlock the full capability of the app?");
        }
    }
}
