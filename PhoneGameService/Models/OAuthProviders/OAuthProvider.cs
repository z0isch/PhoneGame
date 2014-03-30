using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using PhoneGameService.Repositories;
using PhoneGameService.Models.OAuthTokens;

namespace PhoneGameService.Models.OAuthProviders
{
    public abstract class OAuthProvider
    {
        public abstract OAuthID GetIdFromProvider(UnEncryptedToken token);
        public abstract UnEncryptedToken GetToken(string code);

        public string Name 
        {
            get { return this.ToString(); }
        }
    }
}
