using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using PhoneGameWebApi.OAuthProviders;

namespace PhoneGameWebApi.Controllers
{
    public class AuthorizationController : ApiController
    {
        [HttpGet]
        [Route("api/authorization/token/{oauthProvider}")]
        public object Token(string oauthProvider, string oauthCode)
        {
            IOAuthProvider provider = OAuthProviderFactory.GetProvider(oauthProvider);
            if (provider != null)
            {
                return new
                {
                    token = provider.GetToken(oauthCode)
                };
            }
            else
                return null;
        }

    }
}
