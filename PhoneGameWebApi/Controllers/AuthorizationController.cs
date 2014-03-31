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
                        Player p = OAuthService.GetPlayerByOAuthID(repo, id);

                        OAuthService.SaveTokenFromOAuthProvider(repo, hashed, id);
                        return new
                        {
                            oauth_encrypted_token = encrypted.Token,
                            oauth_provider = id.Provider.Name,
                            phone_game_id = p.ID,
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

        [HttpGet]
        [Authorize]
        [Route("api/authorization/tryAuthentication")]
        public string TryAuthentication()
        {
            return "You have been successfully authenticated, " + HttpContext.Current.User.Identity.Name;
        }

    }
}
