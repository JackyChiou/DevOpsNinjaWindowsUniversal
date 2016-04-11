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
    public sealed partial class ReleasesPage : Page
    {
        static bool ifRefresh = true;
        static List<ReleaseDefinition> releaseDefList;
        static List<Release> releasesList;
        static private List<Approval> approvalsList;
        static int pivotItemSelectedIndex = -1;

        private bool manuallyselected;

        static List<ItemDetails> source;
        static List<AlphaKeyGroup<ItemDetails>> itemSource;

        public ReleasesPage()
        {
            this.InitializeComponent();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SetDefaultUI();

            if (pivotItemSelectedIndex > -1)
            {
                ReleasesP.SelectedIndex = pivotItemSelectedIndex;
            }

            LoadDataAndSetPIItems();
        }

        private static void SetDefaultUI()
        {
            Utility.SetStatusBarColor("#56218b", "#ffffff", 1);
        }

        internal static void ResetData()
        {
            ifRefresh = true;
            releaseDefList = null;
            releasesList = null;
            approvalsList = null;
            pivotItemSelectedIndex = -1;
            source = null;
            itemSource = null;
    }

        private void ReleasesP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;
            manuallyselected = false;

            LoadDataAndSetPIItems();
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
                ItemDetails rdName = lv.SelectedItem as ItemDetails;
                if (rdName != null && rdName.Name != null)
                {
                    ReleaseDefinition rd = releaseDefList.Find(x => x.Name.ToLowerInvariant().Equals(rdName.Name.ToLowerInvariant()));
                    if (rd != null)
                    {
                        Frame.Navigate(typeof(RD_ReleasesPage), rd);
                    }
                }
            }
        }

        private void approvalsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;

            ListView lv = sender as ListView;

            Approval approval = lv.SelectedItem as Approval;

            if(Frame != null)
            {
                Frame.Navigate(typeof(ApprovalDetailsPage), approval);
            }
            
        }

        private void releasesLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Frame != null && sender != null)
            {
                Release r = (sender as ListView).SelectedItem as Release;
                Frame.Navigate(typeof(ReleaseDetailsPage),r.Id);
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

        private void LoadDataAndSetPIItems()
        {
            Pivot p = ReleasesP;
            if (p != null)
            {
                PivotItem pi = p.SelectedItem as PivotItem;
                if (pi != null)
                {
                    if (pi.Name.Equals("definitionsPI"))
                    {
                        ShowDefintionDetails();
                    }
                    else if (pi.Name.Equals("releasesPI"))
                    {
                        ShowReleases();
                    }
                    else if (pi.Name.Equals("approvalsPI"))
                    {
                        ShowApprovals();
                    }
                    pivotItemSelectedIndex = p.SelectedIndex;
                    ifRefresh = false;
                }
            }
        }

        private void ShowApprovals()
        {
            if (approvalsList == null || approvalsList.Count < 1 || ifRefresh)
            {
                ShowProgressBar();
                Task.Factory.StartNew(() =>
                {
                    //if(releasesList.Count == 0)
                    //{
                    //    approvalsList = VSTSService.GetApprovals();
                    //}
                    //else
                    //{
                    //    approvalsList = VSTSService.GetApprovals(releasesList);
                    //}
                    approvalsList = VSTSService.GetApprovals();
                }).ContinueWith(async (Task t) =>
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        approvalsLV.ItemsSource = approvalsList;
                        HideProgressBar();
                    });
                });
            }
            else
            {
                approvalsLV.ItemsSource = approvalsList;
                HideProgressBar();
            }
        }
        
        private void ShowDefintionDetails()
        {
            if (releaseDefList == null || releaseDefList.Count < 1 || ifRefresh)
            {
                ShowProgressBar();
                Task.Factory.StartNew(() =>
                {
                    releaseDefList = VSTSService.GetReleaseDefinitions();
                }).ContinueWith(async (Task t) =>
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        if (releaseDefList != null)
                        {
                            source = new List<ItemDetails>();

                            foreach (var bd in releaseDefList)
                            {
                                source.Add(new ItemDetails(bd.Name));
                            }
                            itemSource = AlphaKeyGroup<ItemDetails>.CreateGroups(source,
                                CultureInfo.CurrentUICulture, s => s.Name, true);

                            ((CollectionViewSource)Resources["DefinitionGroups"]).Source = itemSource;
                            manuallyselected = true;
                            HideProgressBar();
                        }
                        else
                        {
                            Utility.ShowMsg("Unable to get release definitions list. Check internet connection and try again later.");
                        }
                    });
                });
            }
            else
            {
                ((CollectionViewSource)Resources["DefinitionGroups"]).Source = itemSource;
                manuallyselected = true;
            }
        }

        private void ShowReleases()
        {
            if (releasesList == null || releasesList.Count < 1 || ifRefresh)
            {
                ShowProgressBar();
                Task.Factory.StartNew(() =>
                {
                    releasesList = VSTSService.GetReleases();
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

        private void approvalCBCloseBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
