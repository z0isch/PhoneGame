using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Configuration;
using System.Security.Cryptography;
using PhoneGameService.Models.OAuthTokens;

namespace PhoneGameService.Models.OAuthProviders
{
    public class Google : OAuthProvider
    {
        private readonly string _clientID = ConfigurationSettings.AppSettings["GoogleAppID"];
        private readonly string _clientSecret = ConfigurationSettings.AppSettings["GoogleAppSecret"];
        private readonly string _redirectUri = ConfigurationSettings.AppSettings["GoogleAppRedirect"];
        public override string GetOAuthUrl()
        {
            string url = String.Format("https://accounts.google.com/o/oauth2/auth?redirect_uri={0}&response_type=code&client_id={1}&scope=profile",
                 HttpUtility.UrlEncode(_redirectUri),
                 HttpUtility.UrlEncode(_clientID));
            return url;
        }
        public override OAuthID GetIdFromProvider(UnEncryptedToken token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/userinfo/v2/me");
            request.Method = "GET";
            request.Headers.Add("Authorization", "Bearer " + token.Token);
            
            GoogleIdentity id = null;

            try
            {
                var response = request.GetResponse();

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    id = JsonConvert.DeserializeObject<GoogleIdentity>(sr.ReadToEnd());
                }
            }
            catch { }
            if (id == null)
            {
                return null;
            }
            else
            {
                return new OAuthID() { ID = id.id, Provider = this };
            }
        }

        public override UnEncryptedToken GetToken(string code)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://accounts.google.com/o/oauth2/token");
            request.Method = "POST";
            string postData = String.Format("client_id={0}&redirect_uri={1}&code={2}&client_secret={3}&grant_type=authorization_code",
                HttpUtility.UrlEncode(_clientID),
                HttpUtility.UrlEncode(_redirectUri),
                HttpUtility.UrlEncode(code),
                HttpUtility.UrlEncode(_clientSecret));
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            GoogleToken token = null;
            try
            {
                var response = request.GetResponse();

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    token = JsonConvert.DeserializeObject<GoogleToken>(sr.ReadToEnd());

                }
            }
            catch(Exception e)
            { 
            
            }
            if (token == null)
                return null;
            else
            {
                return new UnEncryptedToken()
                {
                    Token = token.access_token,
                    Provider = this
                };
            }
        }
    }
    class GoogleToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string id_token { get; set; }
    }

    class GoogleIdentity
    {
        public string family_name {get;set;}
        public string name { get; set; }
        public string picture { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
        public string link { get; set; }
        public string given_name { get; set; }
        public string id { get; set; }
        public bool verified_email { get; set; }
    }
    
}