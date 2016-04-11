using BuildnReleaseWP.Context;
using BuildnReleaseWP.Model;
using BuildnReleaseWP.Service;
using HandyVS.com.Services;
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
    public sealed partial class ReleaseCreatePage : Page
    {
        ReleaseDefinition releaseDefinition;
        public ReleaseCreatePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Utility.SetStatusBarColor("#56218b","#ffffff", 1);

            if(e != null)
            {
                string releaseDefinitionId = e.Parameter as string;

                GetRDDetailsAndSetUI(releaseDefinitionId);

                SetDefaultUI();
            }
        }

        private ReleaseDefinition getReleaseDefinitionForRelease(string id)
        {
            ReleaseDefinition rd = null;
            
            return rd;
        }

        private void GetRDDetailsAndSetUI(string releaseDefinitionId)
        {
            ShowProgressBar();

            Task.Factory.StartNew(() =>
            {
                releaseDefinition = VSTSService.GetAReleaseDefinition(releaseDefinitionId);
            }).ContinueWith((async (Task t) =>
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    definitionTB.Text = releaseDefinition.Name;
                    GetArtifactsVersions();
                    HideProgressBar();
                });
            }));           
        }

        private void SetDefaultUI()
        {
            headerTB.Text = "CREATE RELEASE";
            if (LoginContext.GetLoginContext().Pro)
            {
                noProDescriptionTB.Visibility = Visibility.Collapsed;
            }
            else
            {
                noProDescriptionTB.Visibility = Visibility.Visible;
            }

        }

        internal static void ResetData()
        {

        }

        private void artifactsLV_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(sender != null)
            {
                ListView lv = sender as ListView;
                //string selecteditemStr = lv.SelectedItem as string; 
            }
        }

        private void headerBtn_Click(object sender, RoutedEventArgs e)
        {
            if(sender != null && Frame != null && Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void homeABB_Click(object sender, RoutedEventArgs e)
        {

        }
        private void ShowProgressBar()
        {
            progressG.Visibility = Visibility.Visible;
        }
        private void HideProgressBar()
        {
            progressG.Visibility = Visibility.Collapsed;
        }
        private void newreleaseABB_Click(object sender, RoutedEventArgs e)
        {
            if (sender == null) return;

            string descriptionStr = descriptionTB.Text;
            if (!LoginContext.GetLoginContext().Pro)
            {
                descriptionStr += noProDescriptionTB.Text;
            }

            if (!haveAllArtifactsRequired())
            {
                return;
            }

            Release r = null;

            ShowProgressBar();
            Task.Factory.StartNew(() =>
            {
                r = VSTSService.CreateARelease(releaseDefinition.Id, descriptionStr, releaseArtifactsWithVersionsList);
            }).ContinueWith(async (Task t) =>
            {
                await this.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    HideProgressBar();
                    if(r != null && !String.IsNullOrWhiteSpace(r.Id))
                    {
                       Utility.ShowToastMessage("Release created successfully!", r.Name);
                        if (Frame.CanGoBack)
                        {
                            Frame.GoBack();
                        }
                    }
                    else
                    {
                        Utility.ShowToastMessage("Failed to create release.", "Please retry later...");
                    }
                });
            });

        }

        private string getReleasePostBody()
        {
            throw new NotImplementedException();
        }

        private bool haveAllArtifactsRequired()
        {
            foreach(ReleaseArtifactWithVersions a in releaseArtifactsWithVersionsList)
            {
                if(a.SelectedArtifactVersion == null)
                {
                    return false;
                }
            }

            return true;
        }

        List<ReleaseArtifactWithVersions> releaseArtifactsWithVersionsList;

        private void GetArtifactsVersions()
        {
            string postBodyForGettingArtifactVersions = getPostBodyForGettingArtifactVersions();

            releaseArtifactsWithVersionsList = VSTSService.GetReleaseArtifactsVersions(releaseDefinition.Artifacts, postBodyForGettingArtifactVersions);
            artifactsLV.ItemsSource = releaseArtifactsWithVersionsList;
        }

        private string getPostBodyForGettingArtifactVersions()
        {
            string postBody = "";
            foreach (ReleaseDefinition.RDArtifact a in releaseDefinition.Artifacts)
            {
                postBody = postBody + getPostBodyForGettingArtifactVersions(a);
                postBody = postBody + ",";
            }
            postBody = postBody.Substring(0, postBody.Length - 1);

            postBody = String.Format(Constants.API_POST_BODY_ARTIFACT_VERSIONS_INTERNAL, postBody);

            return postBody;
        }
        private string getPostBodyForGettingArtifactVersions(ReleaseDefinition.RDArtifact a)
        {
            // "{{\"id\":{0},\"type\":\"{1}\",\"alias\":\"{2}\",\"definitionReference\":{\"definition\":{\"id\":\"{3}\",\"name\":\"{4}\"},\"project\":{\"id\":\"{5}\",\"name\":\"{5}\"}}}}";
            string artifactPostBody = String.Format(Constants.API_POST_BODY_CREATE_A_RELEASE_SINGLE_ARTIFACT, a.Id, a.Type, a.Alias, a.DefinitionReference.Definition.Id, 
                a.DefinitionReference.Definition.Name,a.DefinitionReference.Project.Id,a.DefinitionReference.Project.Name);

            return artifactPostBody;
        }

        private void artifactVersionsCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender == null) return;

            ListView lv = artifactsLV;
            if (lv == null) return;

            //ReleaseArtifactWithVersions currentArtifactWithVersions = lv.SelectedItem as ReleaseArtifactWithVersions;

            ComboBox cb = sender as ComboBox;

            if (cb == null) return;


            ReleaseArtifactVersion rav = cb.SelectedItem as ReleaseArtifactVersion;


            ReleaseArtifactWithVersions currentArtifactWithVersions = releaseArtifactsWithVersionsList.FirstOrDefault(x => x.Artifact.Id == rav.ArtifactSourceId);
            if(currentArtifactWithVersions != null)
            {
                currentArtifactWithVersions.SelectedArtifactVersion = rav;
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
