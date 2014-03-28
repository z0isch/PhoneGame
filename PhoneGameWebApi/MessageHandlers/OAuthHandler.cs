using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using PhoneGameWebApi.OAuthProviders;
using PhoneGameWebApi.OAuthTokens;

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
                    string oauthToken = GetOauthToken(request, oauthId, provider);

                    if (!String.IsNullOrEmpty(oauthToken))
                    {
                        principal = provider.GetPrincipalFromDatabase(oauthId,oauthToken);
                        return true;
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
        private static string GetOauthToken(HttpRequestMessage request, string ouathId, OAuthProvider provider)
        {
            string token = "";
            var tHeader = request.Headers.Where(h => h.Key == "oauth_token").FirstOrDefault().Value;
            if (tHeader != null)
            {
                var t = new OAuthToken(tHeader.FirstOrDefault(), OAuthToken.TokenType.Encrypted,ouathId,provider);
                token = t.Token;
            }
            return token;
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