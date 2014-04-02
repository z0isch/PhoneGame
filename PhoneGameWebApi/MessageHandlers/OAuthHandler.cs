using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using PhoneGameService.Models.OAuthProviders;
using PhoneGameService.Models;
using PhoneGameService.Repositories;
using PhoneGameService.Services;
using PhoneGameService.Models.OAuthTokens;
using Newtonsoft.Json;

namespace PhoneGameWebApi.MessageHandlers
{
    public class OAuthHandler : DelegatingHandler
    {

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            PlayerPrincipal principal;
            if(TryGetPrincipal(request, out principal))
            {
                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                    HttpContext.Current.User = principal;
            }
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }

        private static bool TryGetPrincipal(HttpRequestMessage request, out PlayerPrincipal principal)
        {
            using (TelephoneGameRepository repo = new TelephoneGameRepository())
            {
                Player player = GetPlayer(request, repo);
                OAuthProvider provider = GetOuthProvider(request);
                if (player != null && provider != null)
                {
                    OAuthID id = OAuthService.GetOAuthIDByPlayer(repo, player, provider);
                    EncryptedToken token = GetOauthToken(request, id);

                    if (token != null)
                    {
                        UnEncryptedToken unencrypted = OAuthService.UnEncryptToken(token);
                        if (OAuthService.VerifyPlayer(repo, player, unencrypted))
                        {
                            player.IsAuthenticated = true;
                            principal = new PlayerPrincipal(player);
                            return true;
                        }
                    }
                }
            }
            principal = null;
            return false;
        }

        private static Player GetPlayer(HttpRequestMessage request, TelephoneGameRepository repo)
        {
            string id = GetHeader(request, "phone_game_id");
            if (!String.IsNullOrEmpty(id))
            {
                return GameService.GetPlayerByID(id, repo);
            }
            return null;
        }
        private static OAuthProvider GetOuthProvider(HttpRequestMessage request)
        {
            string providerName = GetHeader(request, "oauth_provider");
            if (!String.IsNullOrEmpty(providerName))
            {
                return OAuthProviderFactory.GetProvider(providerName);
            }
            return null;
        }
        private static EncryptedToken GetOauthToken(HttpRequestMessage request, OAuthID id)
        {
            string token = GetHeader(request, "oauth_encrypted_token");
             if (!String.IsNullOrEmpty(token))
             {
                 return OAuthService.GetNewEncryptedToken(token, id);
             }
             else
                 return null;
        }
        private static string GetHeader(HttpRequestMessage request, string headerName)
        {
            string ret = "";
            var retHeader = request.Headers.Where(h => h.Key == headerName).FirstOrDefault().Value;
            if (retHeader != null)
            {
                ret = retHeader.FirstOrDefault();
            }
            return ret;
        }
    }
}