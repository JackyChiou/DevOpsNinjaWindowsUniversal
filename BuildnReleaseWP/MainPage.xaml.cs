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
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace BuildnReleaseWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += new RoutedEventHandler(Page_Loaded);
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            bool loginContextPresent = LoginContext.GetLoginContext().RestoreSavedSettings();
            bool projectContextPresent = ProjectContext.GetProjectContext().RestoreSavedSettings();

            LoginContext.GetLoginContext().Pro = true;

            await NavigateToRightPage(loginContextPresent, projectContextPresent);
        }

        private async Task NavigateToRightPage(bool loginContextPresent, bool projectContextPresent)
        {
            bool loginSuccess = false; ;
            if(loginContextPresent && projectContextPresent)
            {
                Action executeAction = () =>
                {
                    loginSuccess = LoginService.VSTSLogin();
                };

                executeAction();

                if (loginSuccess == false)
                {
                    
                    MessageDialog msgBox = new MessageDialog("Failed to login. Check internet connectivity and try again!");
                    var res = await msgBox.ShowAsync();

                    if(Frame != null)
                    {
                        Frame.Navigate(typeof(LoginPage));
                    }

                }
                else
                {
                    Frame.Navigate(typeof(ServiceSelectionPage));
                }
            }
            else
            {
                Frame.Navigate(typeof(LoginPage));
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
        }
    }
}
