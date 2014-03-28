﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Web;
using Newtonsoft.Json;
using PhoneGameWebApi.OAuthTokens;

namespace PhoneGameWebApi.OAuthProviders
{
    public class Facebook : OAuthProvider
    {
        protected override string GetIdFromProvider(string token)
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
                return id.email;
            }
            
        }

        public override OAuthToken GetToken(string code)
        {
            throw new NotImplementedException();
        }
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