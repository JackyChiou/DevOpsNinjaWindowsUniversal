using BuildnReleaseWP.Model;
using BuildnReleaseWP.Service;
using System;
using System.Collections.Generic;
using System.Globalization;
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
    public sealed partial class RD_ReleasesPage : Page
    {
        static ReleaseDefinition releaseDefinition;
        static bool ifRefresh = true;
        static List<Approval> approvalsList;
        static List<Release> releasesList;

        public RD_ReleasesPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Utility.SetStatusBarColor("#6A1B9A", "#ffffff", 1);
            ReleaseDefinition rdReceived = e.Parameter as ReleaseDefinition;
            
            defTB.Text = rdReceived.Name;

            if(releaseDefinition == null || rdReceived.Id != releaseDefinition.Id)
            {
                ifRefresh = true;
                releaseDefinition = rdReceived;
            }
            GetAndSetPItems();
        }

        private void rdBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && Frame != null)
            {
                Frame.Navigate(typeof(ReleasesPage));
            }
        }

        internal static void ResetData()
        {
            releaseDefinition = null;
            ifRefresh = true;
            approvalsList = null;
            releasesList = null;
        }

        private void ReleasesP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;
            //manuallyselected = false;
            //SetPivotItemHeaderColors();
            GetAndSetPItems();
        }
        
        private void GetAndSetPItems()
        {
            Pivot p = ReleasesP;
            if (p != null)
            {
                PivotItem pi = p.SelectedItem as PivotItem;
                if (pi != null)
                {
                    if (pi.Name.Equals("releasesPI"))
                    {
                        ShowReleases();
                    }
                    else if (pi.Name.Equals("detailsPI"))
                    {
                        ShowRDDetails();
                    }
                   

                    ifRefresh = false;
                }
            }
        }

        private void ShowRDDetails()
        {
            ShowProgressBar();
            detailsSP.Visibility = Visibility.Collapsed;
            Task.Factory.StartNew(() =>
            {
                if (releaseDefinition == null || releaseDefinition.Artifacts == null)
                {
                    releaseDefinition = VSTSService.GetAReleaseDefinition(releaseDefinition.Id);
                }
            }).ContinueWith(async (Task t) =>
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    detailsPI.DataContext = releaseDefinition;
                    HideProgressBar();
                    detailsSP.Visibility = Visibility.Visible;
                });
            });
        }

        private void ShowApprovals()
        {
            ShowProgressBar();
            Task.Factory.StartNew(() =>
            {
                approvalsList = VSTSService.GetApprovals();
            }).ContinueWith(async (Task t) =>
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    //approvalsLV.ItemsSource = approvalsList;
                    HideProgressBar();
                });
            });
        }

        private void ShowReleases()
        {
            if (releasesList == null || releasesList.Count < 1 || ifRefresh)
            {
                ShowProgressBar();
                Task.Factory.StartNew(() =>
                {
                    if (releaseDefinition != null && releaseDefinition.Artifacts != null)
                    {
                        releaseDefinition = VSTSService.GetAReleaseDefinition(releaseDefinition.Id);
                    }
                    releasesList = VSTSService.GetReleases(releaseDefinition);
                }).ContinueWith(async (Task t) =>
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        releasesLV.ItemsSource = releasesList;
                        HideProgressBar();
                    });
                });
            }
            else
            {
                releasesLV.ItemsSource = releasesList;
                HideProgressBar();
            }
        }
        private async Task HandleNewReleaseRequest()
        {
            MessageDialog msg = new MessageDialog("Definition:\n" + releaseDefinition.Name, "Create new Release?");
            msg.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(CommandHandlers)));
            msg.Commands.Add(new UICommand("No", new UICommandInvokedHandler(CommandHandlers)));

            await msg.ShowAsync();
        }

        private void CommandHandlers(IUICommand commandLabel)
        {
            var Actions = commandLabel.Label;
            switch (Actions)
            {
                //Okay Button.
                case "Ok":
                    this.Focus(Windows.UI.Xaml.FocusState.Pointer);
                    break;
                //Quit Button.
                case "Quit":
                    Application.Current.Exit();
                    break;
                    //end.
            }
        }

        private void ShowProgressBar()
        {
            progressG.Visibility = Visibility.Visible;
        }
        private void HideProgressBar()
        {
            progressG.Visibility = Visibility.Collapsed;
        }

        private void releasesLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Frame != null && sender != null)
            {
                Release r = (sender as ListView).SelectedItem as Release;
                Frame.Navigate(typeof(ReleaseDetailsPage), r.Id);
            }
        }

        private void approvalsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void newreleaseABB_Click(object sender, RoutedEventArgs e)
        {
            if (Frame != null && sender != null)
            {
                Frame.Navigate(typeof(ReleaseCreatePage), releaseDefinition.Id);
            }
        }

        private void homeABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void refreshABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void newReleaseBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
