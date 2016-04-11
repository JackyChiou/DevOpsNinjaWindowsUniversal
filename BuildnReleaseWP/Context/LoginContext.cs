using System;
using System.Collections.Generic;
using System.Text;

namespace BuildnReleaseWP.Context
{
    class LoginContext
    {
        private string _userName;
        private string _vstsAccountUrl;
        private string _password;

        private string _authtype;

        public string AuthType
        {
            get { return _authtype; }
            set
            {
                Reset();
                _authtype = value;
                localSettings.Values[str_auth_type] = value;
            }
        }


        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                this._userName = value;
                localSettings.Values[str_username] = value;
            }
        }

        public string VSTSAccountUrl
        {
            get
            {
                return _vstsAccountUrl;
            }
            set
            {
                _vstsAccountUrl = value;
                localSettings.Values[str_vstsAccountUrl] = value;
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                localSettings.Values[str_password] = value;
            }
        }

        public bool Pro { get; internal set; }

        public static LoginContext loginContext = new LoginContext();
        
        public static LoginContext GetLoginContext()
        {
            return loginContext;
        }

        Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
        private string str_pro = "LC_PRO";
        private string str_vstsAccountUrl = "LC_VSTS_URL";
        private string str_username = "LC_USERNAME";
        private string str_password = "LC_PASSWORD";
        private string str_auth_type = "LC_AUTH_TYPE";
        public static string AUTH_BASIC = "AUTH_BASIC";
        public static string AUTH_PAT = "AUTH_PAT";
        public static string AUTH_OAUTH = "AUTH_OAUTH";
        //private string str_autoLogin = "LC_AUTO_LOGIN";

        public bool RestoreSavedSettings()
        {
            string authType = restoreSavedStrSettingsFor(str_auth_type);

            if(!String.IsNullOrWhiteSpace(authType))
            {
                _authtype = authType;

                if(_authtype.Equals(AUTH_BASIC))
                {
                    _userName = restoreSavedStrSettingsFor(str_username);
                    _password = restoreSavedStrSettingsFor(str_password);
                    _vstsAccountUrl = restoreSavedStrSettingsFor(str_vstsAccountUrl);

                    if (!String.IsNullOrWhiteSpace(_userName) && 
                        !String.IsNullOrWhiteSpace(_password) && 
                        !String.IsNullOrWhiteSpace(_vstsAccountUrl))
                    {
                        return true;
                    }
                }
                else if(_authtype.Equals(AUTH_PAT))
                {
                    return false;
                }
                else if(_authtype.Equals(AUTH_OAUTH))
                {
                    return false;
                }

                return false;
            }

            return false;
        }

        private string restoreSavedStrSettingsFor(string setting)
        {
            if (localSettings.Values.ContainsKey(setting))
            {
                return localSettings.Values[setting] as string;
            }
            return String.Empty;
        }

        public void Reset()
        {
            _userName = String.Empty;
            _password = String.Empty;
            _vstsAccountUrl = String.Empty;
            _authtype = "";

            localSettings.Values[str_vstsAccountUrl] = String.Empty;
            localSettings.Values[str_username] = String.Empty;
            localSettings.Values[str_password] = String.Empty;
            localSettings.Values[str_auth_type] = 0;
        }
    }
}
