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
    public sealed partial class BuildDetailsPage : Page
    {
        Build buildReceived;
        //static int pivotIndex = -1;
        Build build;
        List<TimelineRecord> timelineRecords;
        private List<Artifact> artifactsList;

        public BuildDetailsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SetDefaultUI();
            buildReceived = e.Parameter as Build;
            buildTB.Text = buildReceived.BuildNumber;
            //if (BuildP != null && pivotIndex >= 0)
            //{
            //    BuildP.SelectedIndex = pivotIndex;
            //}
            GetAndSetPItems();

        }

        private void SetDefaultUI()
        {
            Utility.SetStatusBarColor("#005DA2", "#ffffff", 1);
        }

        private void buildBtn_Click(object sender, RoutedEventArgs e)
        {
            if (Frame != null && Frame.CanGoBack)
            {

                Frame.GoBack();
            }
        }

        private void BuildP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;
            GetAndSetPItems();
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


        private QueuedBuild queueBuild(BuildDefinition b, string branch, string description)
        {
            bool success = false;
            QueuedBuild queuedB = new QueuedBuild();
            if (String.IsNullOrWhiteSpace(branch))
            {
                if (b.Repository != null)
                {
                    branch = b.Repository.DefaultBranch == null ? "" : b.Repository.DefaultBranch;
                }
            }

            success = VSTSService.QueueBuild(b.Id, branch, description, out queuedB);

            return queuedB;
        }

        private void GetAndSetPItems()
        {
            Pivot p = BuildP;
            if (p != null)
            {
                PivotItem pi = p.SelectedItem as PivotItem;
                if (pi == null) return;

                if (pi.Name.Equals("summaryPI"))
                {
                    if (build == null)
                    {
                        summarySP.Visibility = Visibility.Collapsed;
                        ShowProgressBar();
                        Task.Factory.StartNew(() =>
                        {
                            build = VSTSService.GetABuild(buildReceived.Url);
                        }).ContinueWith(async (Task t) =>
                        {
                            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                summaryPI.DataContext = build;
                                if (build.FinishTime != null && build.StartTime != null)
                                {
                                    durationTB.Text = build.FinishTime.Subtract(build.StartTime).TotalMinutes + " mins";
                                }
                                HideProgressBar();
                                summarySP.Visibility = Visibility.Visible;
                            });
                        });
                    }
                    else
                    {
                        summaryPI.DataContext = build;
                        summarySP.Visibility = Visibility.Visible;
                    }
                }
                else if (pi.Name.Equals("timelinePI"))
                {
                    ShowProgressBar();
                    if (timelineRecords != null)
                    {
                        HideProgressBar();
                        timelineLV.ItemsSource = timelineRecords;
                    }
                    else {
                        Task.Factory.StartNew(() =>
                        {
                            timelineRecords = VSTSService.GetBuildTimeLineRecords(build.Url + "/timeline");
                        }).ContinueWith(async (Task t) =>
                        {
                            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                timelineLV.ItemsSource = timelineRecords;
                                HideProgressBar();
                            });
                        });
                    }
                }
                else if (pi.Name.Equals("artifactsPI"))
                {
                    ShowProgressBar();
                    if (artifactsList != null)
                    {
                        HideProgressBar();
                        artifactsLV.ItemsSource = artifactsList;
                    }
                    else {
                        Task.Factory.StartNew(() =>
                        {
                            artifactsList = VSTSService.GetBuildArtifactss(build.Url + "/artifacts");
                        }).ContinueWith(async (Task t) =>
                        {
                            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                artifactsLV.ItemsSource = artifactsList;
                                HideProgressBar();
                            });
                        });
                    }
                }
                //if (BuildP != null)
                //{
                //    pivotIndex = BuildP.SelectedIndex;
                //}
            }
        }

        private void timelineLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;

            ListView lv = sender as ListView;

            if (lv == null) return;

            TimelineRecord tr = lv.SelectedItem as TimelineRecord;

            if (tr != null && !String.IsNullOrWhiteSpace(tr.LogUrl) && tr.ParentId != null)
            {
                Frame.Navigate(typeof(LogPage), new StringKeyValuePair { key = tr.Name, value = tr.LogUrl });
            }
        }

        private void sendMailABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void homeABB_Click(object sender, RoutedEventArgs e)
        {

        }
        private async void showQueueBuildContentDialog()
        {
            QueuedBuild qb = new QueuedBuild();
            //if (b == null) return;

            QueueBuildCB.DataContext = build;
            QueueBuildCB.HorizontalAlignment = HorizontalAlignment.Center;
            QueueBuildCB.VerticalAlignment = VerticalAlignment.Center;

            var result = await QueueBuildCB.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                qb = queueBuild();

                if (!String.IsNullOrWhiteSpace(qb.BuildNumber))
                {
                    Utility.ShowToastMessage("Build queued successfully!", qb.BuildNumber);
                }
                else
                {
                    Utility.ShowToastMessage("Failed to queue build.", "Please retry later...");
                }
            }
        }

        private QueuedBuild queueBuild()
        {
            bool success = false;
            string branch = build.SourceBranch;
            QueuedBuild queuedB = new QueuedBuild();
            if (String.IsNullOrWhiteSpace(branch))
            {
                branch = build.SourceBranch == null ? "" : build.SourceBranch;
            }

            success = VSTSService.QueueBuild(build.Definition.Id, branch, "", out queuedB);

            return queuedB;
        }
        private void newBuildABB_Click(object sender, RoutedEventArgs e)
        {
            BuildDefinition bd;
            //Task.Factory.StartNew(() =>
            //{
            //    bd = VSTSService.GetBuildArtifactss(build.Url + "/artifacts");
            //}).ContinueWith(async (Task t) =>
            //{
            //    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            //    {
            //        artifactsLV.ItemsSource = artifactsList;
            //        HideProgressBar();
            //    });
            //});
            showQueueBuildContentDialog();
        }

        private void artifactsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
        private void noProCommentsTB_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            HandleUpgradeScenario("Buy Pro version", "You cannot change the description with the free version. Buy DevOps Ninja Pro to unlock the full capability of the app.");
        }

        private void noProCommentsTB_Tapped(object sender, TappedRoutedEventArgs e)
        {
            HandleUpgradeScenario("Buy Pro version", "You cannot change the description with the free version. Buy DevOps Ninja Pro to unlock the full capability of the app.");
        }
    }
}
