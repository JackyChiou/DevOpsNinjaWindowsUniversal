using BuildnReleaseWP.Context;
using BuildnReleaseWP.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Core;
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
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
            //SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;           
        }

        private async void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            MessageDialog msgBox = new MessageDialog("Do you want to exit the app?");
            msgBox.Commands.Clear();
            msgBox.Commands.Add(new UICommand { Label = "Yes", Id = 0 });
            msgBox.Commands.Add(new UICommand { Label = "No", Id = 1 });
            var res = await msgBox.ShowAsync();

            if ((int)res.Id == 0)
            {
                Application.Current.Exit();
            }
            else
            {
                // do nothing
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SetDefaultUI();
            loadDefaultValues();
        }

        private void SetDefaultUI()
        {
            Utility.SetStatusBarColor("#005DA2", "#ffffff", 1);
        }

        private void loadDefaultValues()
        {
            LoginContext lc = LoginContext.GetLoginContext();
            lc.RestoreSavedSettings();

            vstsUrlTB.Text = lc.VSTSAccountUrl == null? "": lc.VSTSAccountUrl;
            userTB.Text = lc.UserName == null ? "" : lc.UserName;
            pwdTB.Password = lc.Password == null ? "" : lc.Password;
        }

        string vstsUrl;
        string username;
        string password;
        private async void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            ProjectContext.GetProjectContext().Reset();
            vstsUrl = vstsUrlTB.Text;
            if (!vstsUrl.Contains(".visualstudio.com") && !vstsUrl.Contains(".com"))
            {
                vstsUrl += ".visualstudio.com";
            }

            if(vstsUrl.Contains("http://"))
            {
                MessageDialog msgBox = new MessageDialog("Check URL. Use 'https' and try again!");
                await msgBox.ShowAsync();
                return;
            }
            else if (!vstsUrl.Contains("https://"))
            {
                vstsUrl = "https://" + vstsUrl;
            }

            
            username = userTB.Text;
            password = pwdTB.Password;

            if (isLoginCredsValid())
            {
                LoginWithPresentBasicAuthCreds();
                NavigateToRightPage();
            }
        }

        bool loginSuccess = false;
        bool firstTime = true;
        private void LoginWithPresentBasicAuthCreds()
        {
            //if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            //{
            //    var statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            //    statusBar.BackgroundColor = Windows.UI.Colors.Green;
            //    statusBar.BackgroundOpacity = 1;
            //}
            //progressbarG.Visibility = Visibility.Visible;
            Action executeAction = () =>
            {
                loginSuccess = LoginService.VSTSLogin(vstsUrl, username, password);
            };

            executeAction();

            //progressbarG.Visibility = Visibility.Collapsed;

            if(loginSuccess)
            {
                saveBasicAuthLoginContext();
            }
            //ProgressBarIndicator.ExecuteActionWithProgressBar(executeAction, progressbarG);
        }

        private void saveBasicAuthLoginContext()
        {
            LoginContext loginContext = LoginContext.GetLoginContext();

            loginContext.AuthType = LoginContext.AUTH_BASIC;
            loginContext.VSTSAccountUrl = vstsUrl;
            loginContext.UserName = username;
            loginContext.Password = password;
        }

        private async void NavigateToRightPage()
        {
            if (loginSuccess == false)
            {
                MessageDialog msgBox = new MessageDialog("Failed to login. Check internet connectivity + credentials and try again!");
                await msgBox.ShowAsync();
            }
            else
            {
                Utility.ResetAllData();
                Frame.Navigate(typeof(ProjectSelectionPage));
            }
        }


        private bool isLoginCredsValid()
        {
            bool basicCredOk = true;
            if (!isValidUrl(vstsUrl) || String.IsNullOrWhiteSpace(username) || String.IsNullOrWhiteSpace(password))
            {
                basicCredOk = false;
            }
            return basicCredOk;
        }

        private bool isValidUrl(object vsoUrl)
        {
            return true;
        }

        private async void altCredHelpHLB_Click(object sender, RoutedEventArgs e)
        {
            string urlStr = "https://www.visualstudio.com/en-us/integrate/get-started/auth/overview";
            Uri mailtoUri = new Uri(urlStr);

            await Launcher.LaunchUriAsync(mailtoUri);
        }
    }
}
