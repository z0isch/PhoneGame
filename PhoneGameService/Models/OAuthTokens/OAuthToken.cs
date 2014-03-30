using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using PhoneGameService.Models.OAuthProviders;

namespace PhoneGameService.Models.OAuthTokens
{
    public class OAuthToken
    {
        public OAuthProvider Provider { get; set; }
        public string Token { get; set; }
     
        internal OAuthToken() {}
    }
}