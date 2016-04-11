using BuildnReleaseWP.Context;
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
    public sealed partial class ProjectSelectionPage : Page
    {
        
        public ProjectSelectionPage()
        {
            this.InitializeComponent();


            string account = LoginContext.GetLoginContext().VSTSAccountUrl;
            if(account.Contains("https://"))
            {
                account = account.Substring(8);
            }

            accountTB.Text = account.ToUpperInvariant();
        }
                

        private async void projectsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!manuallyselected)
            {
                projectsLV.SelectedIndex = -1;
                return;
            }
            var listView = sender as ListView;
            if (listView != null)
            {
                var vsoProject = listView.SelectedItem as ItemDetails;
                if (vsoProject != null)
                {

                    Action executeAction = () =>
                    {
                        pc.Project = vsoProject.Name;
                    };

                    await ProgressBarIndicator.ExecuteActionWithProgressBar(executeAction);
                    if (Frame != null) Frame.Navigate(typeof(ServiceSelectionPage));
                }
            }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SetDefaultUI();

            manuallyselected = false;
            Action executeAction = () =>
            {
                SetItemSource();

            };

            await ProgressBarIndicator.ExecuteActionWithProgressBar(executeAction);

        }

        private static void SetDefaultUI()
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

        static List<Project> projs = new List<Project>();
        static List<ItemDetails> source;
        static List<AlphaKeyGroup<ItemDetails>> itemSource;
        private void SetItemSource()
        {
            ShowProgressBar();
            Task.Factory.StartNew(() =>
            {
                Task.Delay(5000);
                projs = VSTSService.GetVSTSProjects();
                
            }).ContinueWith(async (Task t) =>
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    source = new List<ItemDetails>();

                    foreach (var proj in projs)
                    {
                        source.Add(new ItemDetails(proj.Name));
                    }
                    itemSource = AlphaKeyGroup<ItemDetails>.CreateGroups(source,
                        CultureInfo.CurrentUICulture, s => s.Name, true);

                    ((CollectionViewSource)Resources["ProjectGroups"]).Source = itemSource;

                    manuallyselected = true;
                    HideProgressBar();
                });
            });
            
        }

        ProjectContext pc = ProjectContext.GetProjectContext();
        private bool manuallyselected;

        private void accountBtn_Click(object sender, RoutedEventArgs e)
        {
            if(sender != null && Frame != null)
            {
                Frame.Navigate(typeof(LoginPage));
            }
        }

        private void logoutABB_Click(object sender, RoutedEventArgs e)
        {

        }

        private void settingsABB_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && Frame != null)
            {
                Frame.Navigate(typeof(SettingsPage));
            }
        }

        private void likeDislikeABB_Click(object sender, RoutedEventArgs e)
        {
            if (sender != null && Frame != null)
            {
                Frame.Navigate(typeof(FeedbackPage));
            }
        }
    }
}
