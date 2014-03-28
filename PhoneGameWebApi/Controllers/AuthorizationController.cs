using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using PhoneGameService.Models;
using PhoneGameService.Models.OAuthProviders;
namespace PhoneGameWebApi.Controllers
{
    public class AuthorizationController : ApiController
    {
        //Call this method after returning from the OAuth provider with the code in order to get the OauthToken
        [HttpGet]
        [Route("api/authorization/token/{oauthProvider}")]
        public OAuthToken Token(string oauthProvider, string oauthCode)
        {
            OAuthProvider provider = OAuthProviderFactory.GetProvider(oauthProvider);
            if (provider != null)
            {
                return provider.GetToken(oauthCode);
            }
            else
                return null;
        }

    }
}
