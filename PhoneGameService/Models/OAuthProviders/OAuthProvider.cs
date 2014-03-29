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

        public string Name 
        {
            get { return this.ToString(); }
        }
    }
}
