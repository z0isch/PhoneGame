using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using PhoneGameWebApi.OAuthProviders;

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

        private bool TryGetPrincipal(HttpRequestMessage request, out IPrincipal principal)
        {
            string oauthToken = GetOauthToken(request);
            string oauthProvider = GetOauthProvider(request);

            if (!String.IsNullOrEmpty(oauthProvider) && !String.IsNullOrEmpty(oauthToken))
            {
                IOAuthProvider provider = OAuthProviderFactory.GetProvider(oauthProvider);
                if (provider != null)
                {
                     principal = provider.GetPrincipal(oauthToken);
                     return true;
                }
            }
            principal = null;
            return false;
        }
        private string GetOauthToken(HttpRequestMessage request)
        {
            string token = "";
            var tHeader = request.Headers.Where(h => h.Key == "oauth_token").FirstOrDefault().Value;
            if (tHeader != null)
            {
                token = tHeader.FirstOrDefault();
            }
            return token;
        }
        private string GetOauthProvider(HttpRequestMessage request)
        {
            string provider = "";
            var pHeader = request.Headers.Where(h => h.Key == "oauth_provider").FirstOrDefault().Value;
            if (pHeader != null)
            {
                provider = pHeader.FirstOrDefault();
            }
            return provider;
        }
    }
}