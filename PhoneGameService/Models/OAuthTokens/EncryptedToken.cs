using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace PhoneGameService.Models.OAuthTokens
{
    public class EncryptedToken : OAuthToken
    {
        public string MachineKeyPurpose { get; set; }

        internal EncryptedToken() { }
    }
}
