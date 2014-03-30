using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PhoneGameService.Models.OAuthTokens
{
    public class HashedToken : OAuthToken
    {
        public string Salt { get; set; }

        internal HashedToken() { }
    }
}
