using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using PhoneGameService.Repositories;

namespace PhoneGameService.Models.OAuthProviders
{
    public abstract class OAuthProvider
    {
        protected abstract string GetIdFromProvider(string token);
        public abstract OAuthToken GetToken(string code);
        
        //See if this person is who they say they are by hashing the token and comparing it to this users hashed token
        public IPrincipal GetPrincipalFromDatabase(OAuthToken token)
        {
            using (var repository = new TelephoneGameRepository())
            {
                var p = repository.GetPlayerByOauthId(this, token.Id);
                if(p.Token.HashedToken == token.HashedToken)
                    return new GenericPrincipal(new GenericIdentity(p.Name), new string[0]);
            }
           return null;
        }
        
        //Save this users new token
        public void SaveToken(OAuthToken token)
        {
            using (var repository = new TelephoneGameRepository())
            {
                var p = repository.GetPlayerByOauthId(this, token.Id);
                p.Token = token;
            }
        }
    }
}
