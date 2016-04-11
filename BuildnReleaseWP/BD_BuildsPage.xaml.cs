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
    public sealed partial class BD_BuildsPage : Page
    {
        static bool ifRefresh = true;
        static BuildDefinition bd;
        BuildDefinition bdReceived;
        static List<Build> completedBuildsList;
        static List<QueuedBuild> queuedBuildsList;
        private static string completedBuildsBDId = "";
        private static string queuedBuildsBDId = "";

        public BD_BuildsPage()
        {
            this.InitializeComponent();

            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SetDefaultUI();
            bdReceived = e.Parameter as BuildDefinition;
            bdTB.Text = bdReceived.Name;

            GetAndSetPItems();
        }

        private void SetDefaultUI()
        {
            Service.Utility.SetStatusBarColor("#005DA2", "#ffffff", 1);
        }

        private void ShowProgressBar()
        {
            progressG.Visibility = Visibility.Visible;
        }

        internal static void ResetData()
        {
            ifRefresh = true;
            bd = null;
            completedBuildsList = null;
            queuedBuildsList = null;
            completedBuildsBDId = "";
            queuedBuildsBDId = "";
        }

        private void HideProgressBar()
        {
            progressG.Visibility = Visibility.Collapsed;
        }
        private void completedBuildsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null)
                return;

            ListView bLV = sender as ListView;

            if (bLV == null) return;

            Build b = bLV.SelectedItem as Build;

            if(b != null)
            {
                Frame.Navigate(typeof(BuildDetailsPage), b);
            }
        }

        private void queuedBuildsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BuildP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;
            GetAndSetPItems();
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
                    if (bd == null ||  ifRefresh || !bd.Id.Equals(bdReceived.Id))
                    {
                        summarySP.Visibility = Visibility.Collapsed;
                        ShowProgressBar();
                        Task.Factory.StartNew(() =>
                        {
                            bd = VSTSService.GetABuildDefinition(bdReceived);
                        }).ContinueWith(async (Task t) =>
                        {
                            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                summaryPI.DataContext = bd;
                                HideProgressBar();

                                summarySP.Visibility = Visibility.Visible;
                            });
                        });
                    }
                    else
                    {
                        summaryPI.DataContext = bd;
                        summarySP.Visibility = Visibility.Visible;
                    }
                }
                else if (pi.Name.Equals("completedBuildsPI"))
                {
                    if (completedBuildsList == null || completedBuildsList.Count < 1 || ifRefresh || !bd.Id.Equals(completedBuildsBDId))
                    {
                        ShowProgressBar();
                        Task.Factory.StartNew(() =>
                        {
                            completedBuildsList = VSTSService.GetBuilds(bd);
                            completedBuildsBDId = bd.Id;
                        }).ContinueWith(async (Task t) =>
                        {
                            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                completedBuildsLV.ItemsSource = completedBuildsList;
                                HideProgressBar();
                            });
                        });
                    }
                    else
                    {
                        completedBuildsLV.ItemsSource = completedBuildsList;
                        HideProgressBar();
                    }
                }
                else if (pi.Name.Equals("queuedBuildsPI"))
                {
                    if (queuedBuildsList == null || queuedBuildsList.Count < 1 || ifRefresh || !bd.Id.Equals(queuedBuildsBDId))
                    {
                        ShowProgressBar();
                        Task.Factory.StartNew(() =>
                        {
                            queuedBuildsList = VSTSService.GetQueuedBuilds(bd);
                            queuedBuildsBDId = bd.Id;
                        }).ContinueWith(async (Task t) =>
                        {
                            await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                            {
                                queuedBuildsLV.ItemsSource = queuedBuildsList;
                                HideProgressBar();
                            });
                        });
                    }
                    else
                    {
                        queuedBuildsLV.ItemsSource = queuedBuildsList;
                        HideProgressBar();
                    }
                }

                ifRefresh = false;
            }
        }

        private async void showQueueBuildContentDialog(BuildDefinition b)
        {
            QueuedBuild qb = new QueuedBuild();
            if (b == null) return;

            QueueBuildCB.DataContext = b;
            QueueBuildCB.HorizontalAlignment = HorizontalAlignment.Center;
            QueueBuildCB.VerticalAlignment = VerticalAlignment.Center;

            var result = await QueueBuildCB.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                qb = queueBuild(b, "");

                if (!String.IsNullOrWhiteSpace(qb.BuildNumber))
                {
                    Utility.ShowToastMessage("Build queued successfully!", qb.BuildNumber);
                    ifRefresh = true;
                }
                else
                {
                    Utility.ShowToastMessage("Failed to queue build.", "Please retry later...");
                }
            }
        }

        private QueuedBuild queueBuild(BuildDefinition b, string branch)
        {
            bool success = false;
            QueuedBuild queuedB = new QueuedBuild();
            if (String.IsNullOrWhiteSpace(branch))
            {
                if(b.Repository != null)
                {
                    branch = b.Repository.DefaultBranch == null?"": b.Repository.DefaultBranch;
                }
            }

            success = VSTSService.QueueBuild(b.Id, branch, "", out queuedB);

            return queuedB;
        }

        private void bdBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && Frame != null)
            {
                Frame.Navigate(typeof(BuildsPage));
            }
        }

        private void newBuildABB_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;
            showQueueBuildContentDialog(bd);
        }

        private void homeABB_Click(object sender, RoutedEventArgs e)
        {
            if(sender != null && Frame != null)
            {
                Frame.Navigate(typeof(ServiceSelectionPage));
            }
        }

        private void refreshABB_Click(object sender, RoutedEventArgs e)
        {
            ifRefresh = true;
            GetAndSetPItems();
        }
    }
}
