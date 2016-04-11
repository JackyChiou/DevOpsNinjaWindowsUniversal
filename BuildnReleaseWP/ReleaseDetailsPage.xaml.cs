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
    public sealed partial class ReleaseDetailsPage : Page
    {
        static Release release;
        //ReleaseDefinition releaseDefinition;
        static int releaseId;
        static bool ifRefresh = false;
        static int pivotItemSelectedIndex = -1;
        static int envPickerCBSelectedIndex = -1;
        static List<Approval> approvalsList = new List<Approval>();
        Dictionary<ItemDetails, List<TimelineRecord>> envToTasksDictionary = new Dictionary<ItemDetails, List<TimelineRecord>>();

        public ReleaseDetailsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e == null) return;
            Utility.SetStatusBarColor("#56218b", "#ffffff", 1);

            //KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)e.Parameter;

            ////releaseDefinition = kvp.Key;

            string rId =e.Parameter as string;



            if (!String.IsNullOrWhiteSpace(rId))
            {
                Int32.TryParse(rId, out releaseId);
                //headerTB.Text = r.Name;
            }

            headerTB.Text = "RELEASE DETAILS";

            if (release == null || !release.Id.Equals(rId))
            {
                ifRefresh = true;
                release = null;
            }

            if (pivotItemSelectedIndex > -1)
            {
                ReleaseP.SelectedIndex = pivotItemSelectedIndex;
            }

            LoadDataAndSetPI();
        }

        internal static void ResetData()
        {
            release = null;
            releaseId = 0;
            ifRefresh = true;
            pivotItemSelectedIndex = -1;
            envPickerCBSelectedIndex = -1;
            approvalsList = new List<Approval>();
        }

        private void ReleaseP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadDataAndSetPI();
        }

        private void LoadDataAndSetPI()
        {
            Pivot p = ReleaseP as Pivot;

            if (p == null) return;

            PivotItem pi = p.SelectedItem as PivotItem;

            if (pi == null) return;

            if (pi.Name.Equals("summaryPI"))
            { 
                ShowSummaryData();
            }
            else if (pi.Name.Equals("environmentsPI"))
            {
                ShowEnvironmentsData();
            }
            else if (pi.Name.Equals("logsPI"))
            {
                ShowLogsData();
            }
            else if (pi.Name.Equals("approvalsPI"))
            {
                try
                {
                    ShowApprovalsData();
                }
                catch (Exception ex)
                {

                }

            }

            pivotItemSelectedIndex = p.SelectedIndex;
            ifRefresh = false;
        }

        private void ShowApprovalsData()
        {
            if (ifRefresh || release == null || approvalsList == null || approvalsList.Count < 1)
            {
                ShowProgressBar();
                Task.Factory.StartNew(() =>
                {
                    if (release == null)
                    {
                        release = VSTSService.GetARelease(releaseId);
                    }
                    approvalsList = getApprovalListFromRelease(release);
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
                ShowProgressBar();
                approvalsLV.ItemsSource = approvalsList;
                HideProgressBar();
            }
        }

        private void ShowLogsData()
        {
            ShowProgressBar();
            if (ifRefresh || release == null)
            {
                Task.Factory.StartNew(() =>
                {
                    release = VSTSService.GetARelease(releaseId);
                }).ContinueWith(async (Task t) =>
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        envPickerCB.ItemsSource = release.Environments;
                        envPickerCB.SelectedIndex = 0;
                        HideProgressBar();
                    });
                });
            }
            else
            {
                ShowProgressBar();
                envPickerCB.ItemsSource = release.Environments;
                HideProgressBar();
            }
        }

        private void ShowEnvironmentsData()
        {
            if (ifRefresh || release == null)
            {
                Task.Factory.StartNew(() =>
                {
                    release = VSTSService.GetARelease(releaseId);
                }).ContinueWith(async (Task t) =>
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        environmentsLV.ItemsSource = release.Environments;
                        ifRefresh = false;
                    });
                });
            }
            else
            {
                environmentsLV.ItemsSource = release.Environments;
            }
        }

        private void ShowSummaryData()
        {
            if (release == null || ifRefresh)
            {
                ReleaseP.Visibility = Visibility.Collapsed;
                ShowProgressBar();
                Task.Factory.StartNew(() =>
                {
                    release = VSTSService.GetARelease(releaseId);
                }).ContinueWith(async (Task t) =>
                {
                    await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                    {
                        summarySP.DataContext = release;
                        artifactsLV.ItemsSource = release.Artifacts;
                        headerTB.Text = release.Name;
                        HideProgressBar();
                        ReleaseP.Visibility = Visibility.Visible;
                    });
                });
            }
            else
            {
                summarySP.DataContext = release;
                HideProgressBar();
                ReleaseP.Visibility = Visibility.Visible;
            }
        }

        private List<Approval> getApprovalListFromRelease(Release release)
        {
            approvalsList = new List<Approval>();

            foreach (Release.REnvironment re in release.Environments)
            {
                foreach (Approval a in re.PreDeployApprovals)
                {
                    approvalsList.Add(a);
                }
                foreach (Approval a in re.PostDeployApprovals)
                {
                    approvalsList.Add(a);
                }
            }

            return approvalsList;
        }

        private Dictionary<ItemDetails, List<TimelineRecord>> getEnvToTasks(Release release)
        {
            Dictionary<ItemDetails, List<TimelineRecord>> envToTasksDict = new Dictionary<ItemDetails, List<TimelineRecord>>();

            List<Release.REnvironment> envs = release.Environments;

            if (envs != null && envs.Count > 0)
            {
                foreach (var env in envs)
                {
                    ItemDetails id = new ItemDetails(env.Id, env.Name);

                    //TimelineRecord

                    Release.DeployStep deploySteps = env.DeploySteps;

                    if (deploySteps != null && deploySteps.Tasks != null)
                    {
                        //foreach Task
                    }
                }
            }

            return envToTasksDict;
        }

        private void ShowProgressBar()
        {
            progressG.Visibility = Visibility.Visible;
        }

        private void HideProgressBar()
        {
            progressG.Visibility = Visibility.Collapsed;
        }

        private void headerBtn_Click(object sender, RoutedEventArgs e)
        {
            if(Frame != null && sender != null && Frame.CanGoBack)
            {
                Frame.GoBack();
            }

        }

        private void refreshABB_Click(object sender, RoutedEventArgs e)
        {
            ifRefresh = true;
            release = null;
            LoadDataAndSetPI();
        }
        

        private void logsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null || envPickerCB == null) return;

            ListView lv = sender as ListView;

            Release.DTask task = lv.SelectedItem as Release.DTask;

            envPickerCBSelectedIndex = lv.SelectedIndex;

            if (task == null) return;

            Release.REnvironment env = envPickerCB.SelectedItem as Release.REnvironment;

            if (env == null) return;

            string taskLogUrl = getReleaseUrlFor(release) + "/environments/" + env.Id + "/tasks/" + task.Id + "/logs";

            if (Frame != null)
            {
                Frame.Navigate(typeof(LogPage), new StringKeyValuePair { key = task.Name, value = taskLogUrl });
            }

        }

        private string getReleaseUrlFor(Release release)
        {
            string releaseUrl = "INVALID_URL_REQUEST";
            if (release != null)
            {
                if (!String.IsNullOrWhiteSpace(release.Url))
                {
                    releaseUrl = release.Url;
                }
                else
                {
                    releaseUrl = VSTSService.getReleaseServiceUrl() + "/DefaultCollection/" + ProjectContext.GetProjectContext().Project + "/_apis/release/releases/" + release.Id;
                }
            }

            return releaseUrl;
        }

        private void artifactsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void sendMailABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void newReleaseABB_Click(object sender, RoutedEventArgs e)
        {
            if(sender != null && Frame != null)
            {
               
                Frame.Navigate(typeof(ReleaseCreatePage), release.ReleaseDefinition.Id);
            }
            //await ShowConfirmationDialogAndHandleNewReleaseRequest();
        }

        //private async Task ShowConfirmationDialogAndHandleNewReleaseRequest()
        //{
        //    MessageDialog msg = new MessageDialog("Definition: \n" + release.ReleaseDefinition.Name, "Create new Release?");
        //    msg.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(CommandHandlers)));
        //    msg.Commands.Add(new UICommand("No", new UICommandInvokedHandler(CommandHandlers)));

        //    await msg.ShowAsync();
        //}

        //private void CommandHandlers(IUICommand commandLabel)
        //{
        //    var Actions = commandLabel.Label;
        //    switch (Actions)
        //    {
        //        //Okay Button.
        //        case "Yes":
        //            getReleaseDefinitionForId();
        //            this.Focus(Windows.UI.Xaml.FocusState.Pointer);
        //            break;
        //        //Quit Button.
        //        case "No":
        //            this.Focus(Windows.UI.Xaml.FocusState.Pointer);
        //            break;
        //            //end.
        //    }
        //}

       

        private void environmentsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void envPickerCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;

            ComboBox cb = sender as ComboBox;

            if (cb == null || cb.SelectedItem == null) return;
            
            Release.REnvironment re = cb.SelectedItem as Release.REnvironment;


            Release.DeployStep ds = re.DeploySteps;

            if (ds != null && ds.Tasks != null)
            {
                logsLV.ItemsSource = ds.Tasks;
            }
            else
            {
                logsLV.ItemsSource = null;
            }

        }

        private void approvalsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;

            ListView lv = sender as ListView;

            Approval approval = lv.SelectedItem as Approval;

            if (Frame != null)
            {
                Frame.Navigate(typeof(ApprovalDetailsPage), approval);
            }
        }
    }
}
