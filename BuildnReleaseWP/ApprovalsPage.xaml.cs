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
    public sealed partial class ApprovalsPage : Page
    {
        static List<Approval> myApprovalsList;
        static List<Approval> allApprovalsList;

        public static bool ifRefresh = true;

        public ApprovalsPage()
        {
            this.InitializeComponent();
        }

        private void SetDefaultUI()
        {
            Utility.SetStatusBarColor("#B71C1C", "#ffffff", 1);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            SetDefaultUI();

            LoadDataAndSetUI();
        }

        private void LoadDataAndSetUI()
        {

            Pivot p = approvalsP;

            if (p == null) return;

            PivotItem pi = p.SelectedItem as PivotItem;
            if (pi == null) return;

            if (pi.Name.Equals("myApprovalsPI"))
            {
                ShowMyApprovals();
            }
            else if (pi.Name.Equals("allApprovalsPI"))
            {
                ShowAllApprovals();
            }
        }

        private void serviceBtn_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && Frame != null)
            {
                Frame.Navigate(typeof(ServiceSelectionPage));
            }
        }

        internal static void ResetData()
        {
            myApprovalsList = null;
            allApprovalsList = null;

            ifRefresh = true;
        }

        private void ShowMyApprovals()
        {
            if (myApprovalsList == null || myApprovalsList.Count < 1 || ifRefresh)
            {
                ShowProgressBar();
                Task.Factory.StartNew(() =>
                {
                    myApprovalsList = VSTSService.GetMyApprovals();
                }).ContinueWith(async (Task t) =>
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        myApprovalsLV.ItemsSource = myApprovalsList;
                        HideProgressBar();
                    });
                });
            }
            else
            {
                myApprovalsLV.ItemsSource = myApprovalsList;
                HideProgressBar();
            }
        }

        private void ShowAllApprovals()
        {
            if (allApprovalsList == null || allApprovalsList.Count < 1 || ifRefresh)
            {
                ShowProgressBar();
                Task.Factory.StartNew(() =>
                {
                    allApprovalsList = VSTSService.GetApprovals();
                }).ContinueWith(async (Task t) =>
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        allApprovalsLV.ItemsSource = allApprovalsList;
                        HideProgressBar();
                    });
                });
            }
            else
            {
                allApprovalsLV.ItemsSource = allApprovalsList;
                HideProgressBar();
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

        private void approvalsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void homeABB_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && Frame != null)
            {
                Frame.Navigate(typeof(ServiceSelectionPage));
            }
        }
       
        private void refreshABB_Click(object sender, RoutedEventArgs e)
        {
            ifRefresh = true;
            LoadDataAndSetUI();            
        }

        private void allApprovalsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   
            if(sender != null)
            {
                NavigateToDetailsPage(sender as ListView);
            }

        }

        private void NavigateToDetailsPage(ListView lv)
        {
            if(lv != null && Frame != null)
            {
                Approval approval = (Approval)lv.SelectedItem;
                Frame.Navigate(typeof(ApprovalDetailsPage), approval);
            }
        }

        private void myApprovalsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender != null)
            {
                NavigateToDetailsPage(sender as ListView);
            }
        }

        private void approvalsP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Pivot p = sender as Pivot;
            if (p == null) return;

            LoadDataAndSetUI();
        }
    }
}
