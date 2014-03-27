using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using Newtonsoft.Json;

namespace PhoneGameWebApi.OAuthProviders
{
    public class Facebook : IOAuthProvider
    {

        #region IOAuthProvider Members

        public System.Security.Principal.IPrincipal GetPrincipal(string token)
        {
            var url = String.Format(@"https://graph.facebook.com/me?access_token={0}", token);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            
            FacebookIdentity id = null;

            try
            {
                var response = request.GetResponse();

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    id = JsonConvert.DeserializeObject<FacebookIdentity>(sr.ReadToEnd());
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

        public string GetToken(string code)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
    class FacebookIdentity
    {
        public string id { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string link { get; set; }
        public string username { get; set; }
        public string gender { get; set; }
        public string email { get; set; }
    }
}