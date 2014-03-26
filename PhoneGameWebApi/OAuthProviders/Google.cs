using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace PhoneGameWebApi.OAuthProviders
{
    public class Google : IOAuthProvider
    {
        #region IOAuthProvider Members

        public IPrincipal GetPrincipal(string token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.googleapis.com/userinfo/v2/me");
            request.Method = "GET";
            request.Headers.Add("Authorization", "Bearer " + token);
            
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
                return new GenericPrincipal(new GenericIdentity(id.email), new string[] { "seeAllPlayers" }); ;
            }
        }

        #endregion
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