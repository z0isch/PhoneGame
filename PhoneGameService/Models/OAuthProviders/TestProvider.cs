using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneGameService.Models.OAuthTokens;

namespace PhoneGameService.Models.OAuthProviders
{
    public class TestProvider : OAuthProvider
    {
        public override string GetOAuthUrl()
        {
            return "";
        }

        public override OAuthID GetIdFromProvider(OAuthTokens.UnEncryptedToken token)
        {
            return new OAuthID() { ID = "1", Provider = this };
        }

        public override OAuthTokens.UnEncryptedToken GetToken(string code)
        {
            return new UnEncryptedToken()
            {
                Token = "Test",
                Provider = this
            };
        }
    }
}
