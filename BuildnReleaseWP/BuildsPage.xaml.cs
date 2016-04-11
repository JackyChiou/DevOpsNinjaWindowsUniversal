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
using Windows.UI;
using Windows.UI.Notifications;
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
    public sealed partial class BuildsPage : Page
    {
        static bool ifRefresh = true;
        static List<BuildDefinition> buildDefList;
        static List<Build> completedBuildsList;
        static List<QueuedBuild> queuedBuildsList;

        private bool manuallyselected;

        static List<ItemDetails> source;
        static List<AlphaKeyGroup<ItemDetails>> itemSource;

        public BuildsPage()
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
            Utility.SetStatusBarColor("#005DA2", "#ffffff", 1);
        }

        private void ShowProgressBar()
        {
            progressG.Visibility = Visibility.Visible;
        }
        private void HideProgressBar()
        {
            progressG.Visibility = Visibility.Collapsed;
        }

        private void BuildP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;
            manuallyselected = false;

            LoadDataAndSetPIItems();
        }

        internal static void ResetData()
        {
            ifRefresh = true;
            buildDefList = null;
            completedBuildsList = null;
            queuedBuildsList = null;

            source = null;
            itemSource = null;
        }

        private void definitionsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!manuallyselected)
            {
                definitionsLV.SelectedIndex = -1;
                return;
            }
            if (sender != null)
            {
                ListView lv = sender as ListView;
                ItemDetails bdName = lv.SelectedItem as ItemDetails;
                if (bdName != null && bdName.Name != null)
                {
                    BuildDefinition bd = buildDefList.Find(x => x.Name.ToLowerInvariant().Equals(bdName.Name.ToLowerInvariant()));
                    if (bd != null)
                    {
                        Frame.Navigate(typeof(BD_BuildsPage), bd);
                    }
                }
            }
        }
                
        private async void showQueueBuildContentDialog(Build b)
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

        private QueuedBuild queueBuild(Build b, string branch)
        {
            bool success = false;
            QueuedBuild queuedB = new QueuedBuild();
            if(String.IsNullOrWhiteSpace(branch) )
            {
                branch = b.SourceBranch;
            }
            if (b.Definition != null)
            {
                success = VSTSService.QueueBuild(b.Definition.Id, branch, "", out queuedB);
            }
            return queuedB;
        }

        private void LoadDataAndSetPIItems()
        {
            Pivot p = BuildP;
            if (p != null)
            {
                PivotItem pi = p.SelectedItem as PivotItem;
                if (pi != null)
                {
                    if (pi.Name.Equals("definitionsPI"))
                    {
                        if (buildDefList == null || buildDefList.Count < 1 || ifRefresh)
                        {
                            ShowProgressBar();
                            Task.Factory.StartNew(() =>
                            {
                                buildDefList = VSTSService.GetBuildDefinitions();
                            }).ContinueWith(async (Task t) =>
                            {
                                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                                    {
                                        source = new List<ItemDetails>();

                                        foreach (var bd in buildDefList)
                                        {
                                            source.Add(new ItemDetails(bd.Name));
                                        }
                                        itemSource = AlphaKeyGroup<ItemDetails>.CreateGroups(source,
                                            CultureInfo.CurrentUICulture, s => s.Name, true);

                                        ((CollectionViewSource)Resources["DefinitionGroups"]).Source = itemSource;
                                        manuallyselected = true;
                                        HideProgressBar();
                                    }
                                    );
                            });

                        }
                        else
                        {
                            ((CollectionViewSource)Resources["DefinitionGroups"]).Source = itemSource;
                            manuallyselected = true;
                        }
                    }
                    else if (pi.Name.Equals("completedBuildsPI"))
                    {
                        if (completedBuildsList == null || completedBuildsList.Count < 1 || ifRefresh)
                        {
                            ShowProgressBar();
                            Task.Factory.StartNew(() =>
                            {
                                completedBuildsList = VSTSService.GetBuilds();
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
                        }
                    }
                    else if (pi.Name.Equals("queuedBuildsPI"))
                    {
                        if (queuedBuildsList == null || queuedBuildsList.Count < 1 || ifRefresh)
                        {

                            ShowProgressBar();
                            Task.Factory.StartNew(() =>
                            {
                                queuedBuildsList = VSTSService.GetQueuedBuilds();
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
                        }
                    }
                    //else if(pi.Name.Equals(""))
                    //{
                    //    showCB(); 
                    //}
                    ifRefresh = false;
                }
            }
        }
        
        private void queuedBuildsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;
        }

        private void completedBuildsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;

            Build build = (sender as ListView).SelectedItem as Build;

            if(Frame != null)
            {
                Frame.Navigate(typeof(BuildDetailsPage), build);
            }
            //showQueueBuildContentDialog(b);
        }


        private void refreshABB_Click(object sender, RoutedEventArgs e)
        {
            ifRefresh = true;
            manuallyselected = false;
            LoadDataAndSetPIItems();
        }

        private void homeABB_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && Frame != null)
                Frame.Navigate(typeof(ServiceSelectionPage));
        }

        private void serviceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && Frame != null)
                Frame.Navigate(typeof(ServiceSelectionPage));
        }
    }


}
