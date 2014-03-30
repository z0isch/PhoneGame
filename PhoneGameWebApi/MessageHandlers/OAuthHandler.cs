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

namespace PhoneGameWebApi.MessageHandlers
{
    public class OAuthHandler : DelegatingHandler
    {

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            IPrincipal principal;
            if(TryGetPrincipal(request, out principal))
            {
                Thread.CurrentPrincipal = principal;
                if (HttpContext.Current != null)
                    HttpContext.Current.User = principal;
            }
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }

        private static bool TryGetPrincipal(HttpRequestMessage request, out IPrincipal principal)
        {
            string oauthProvider = GetOauthProvider(request);
            string oauthId = GetOauthId(request);

            if (!String.IsNullOrEmpty(oauthProvider) && !String.IsNullOrEmpty(oauthId))
            {
                OAuthProvider provider = OAuthProviderFactory.GetProvider(oauthProvider);
                if (provider != null)
                {
                    string oauthToken = GetOauthToken(request, provider);
                    var id = new OAuthID() { ID = oauthId, Provider = provider };
                    if (oauthToken != null)
                    {
                        using (TelephoneGameRepository repo = new TelephoneGameRepository())
                        {
                            EncryptedToken encryptedToken = OAuthService.EncryptedToken(oauthToken, id);
                            UnEncryptedToken unencrypted = OAuthService.UnEncryptToken(encryptedToken);
                            Player p = OAuthService.GetPlayerByOAuthID(repo,id);
                            if (OAuthService.VerifyPlayer(repo,p,unencrypted))
                            {
                                principal = new GenericPrincipal(new GenericIdentity(p.Name), new string[0]);
                                return true;
                            }
                        }
                    }
                }
            }
            principal = null;
            return false;
        }

        private static string GetOauthId(HttpRequestMessage request)
        {
            return GetHeader(request, "oauth_id");
        }
        private static string GetOauthToken(HttpRequestMessage request, OAuthProvider provider)
        {
            return GetHeader(request, "oauth_token");
        }
        private static string GetOauthProvider(HttpRequestMessage request)
        {
            return GetHeader(request, "oauth_provider");
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