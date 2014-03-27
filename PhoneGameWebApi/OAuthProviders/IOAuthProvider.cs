using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace PhoneGameWebApi.OAuthProviders
{
    public interface IOAuthProvider
    {
        IPrincipal GetPrincipal(string token);
        string GetToken(string code);
    }
}
