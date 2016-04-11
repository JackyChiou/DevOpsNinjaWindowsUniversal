using BuildnReleaseWP.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BuildnReleaseWP.Service
{
    public static class LoginService
    {
        public static HttpClient VSTSHttpClient;

        public static HttpClient Connect(string userName, string password)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(
                new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(
                    System.Text.UTF8Encoding.UTF8.GetBytes(
                        string.Format("{0}:{1}", userName, password))));
            return client;
        }

        public static bool VSTSLogin()
        {
            LoginContext lc = LoginContext.GetLoginContext();
            return VSTSLogin(lc.VSTSAccountUrl, lc.UserName, lc.Password);
        }

        public static bool VSTSLogin(string vsoUrl, string username, string passwd)
        {
            bool loginSuccess = false;

            if (String.IsNullOrEmpty(username) ||
                String.IsNullOrEmpty(passwd) ||
                String.IsNullOrEmpty(vsoUrl))
            {
                return false;
            }
            
            VSTSHttpClient = Connect(username, passwd);

            string tfsURL = string.Format("{0}/DefaultCollection", vsoUrl);
            try
            {
                using (HttpResponseMessage response = VSTSHttpClient.GetAsync(
                                tfsURL).Result)
                {
                    int rs = (int)response.StatusCode;
                    response.EnsureSuccessStatusCode();
                    loginSuccess = true;
                }
            }
            catch
            {
                loginSuccess = false;
            }
            return loginSuccess;
        }
    }
}
