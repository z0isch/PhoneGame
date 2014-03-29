using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using PhoneGameService.Models;
using PhoneGameService.Models.OAuthProviders;
using PhoneGameService.Repositories;
using PhoneGameService.Services;
namespace PhoneGameWebApi.Controllers
{
    public class AuthorizationController : ApiController
    {
        //Call this method after returning from the OAuth provider with the code in order to get the OauthToken
        [HttpGet]
        [Route("api/authorization/token/{oauthProvider}")]
        public object Token(string oauthProvider, string oauthCode)
        {
            OAuthProvider provider = OAuthProviderFactory.GetProvider(oauthProvider);
            if (provider != null)
            {
                OAuthToken token = provider.GetToken(oauthCode);
                OAuthID id = provider.GetIdFromProvider(token);

                using (TelephoneGameRepository repo = new TelephoneGameRepository())
                {
                    OAuthService.SaveTokenFromOAuthProvider(repo, token, id);
                }
                return new
                {
                    token = token,
                    id = id
                };
                
            }
            else
                return null;
        }

    }
}
