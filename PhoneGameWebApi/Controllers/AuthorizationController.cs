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
using PhoneGameService.Models.OAuthTokens;
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
                UnEncryptedToken token = provider.GetToken(oauthCode);
                if (token != null)
                {
                    OAuthID id = provider.GetIdFromProvider(token);
                    using (TelephoneGameRepository repo = new TelephoneGameRepository())
                    {
                        var encrypted = OAuthService.EncryptToken(token, id);
                        var hashed = OAuthService.HashTokenWithRandomSalt(token);
                        OAuthService.SaveTokenFromOAuthProvider(repo, hashed, id);
                        return new
                        {
                            token = encrypted,
                            id = id
                        };
                    }
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.Forbidden);
                }
            }
            else
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

    }
}
