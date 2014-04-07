using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhoneGameService.Models.OAuthProviders
{
    public class OAuthProviderFactory
    {
        public enum OAuthProviders {Google,Facebook,TestProvider};

        public static OAuthProvider GetProvider(string providerName)
        {
            OAuthProviders provider;
            if (Enum.TryParse<OAuthProviders>(providerName, true, out provider))
            {
                switch (provider)
                {
                    case OAuthProviders.Google:
                        return new Google();
                    case OAuthProviders.Facebook:
                        return new Facebook(); 
                    case OAuthProviders.TestProvider:
                        return new TestProvider();
                    default:
                        return null;
                }
            }
            return null;
        }
    }
}