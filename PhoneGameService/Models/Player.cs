using PhoneGameService.Models.OAuthTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace PhoneGameService.Models
{
    public class PlayerPrincipal : IPrincipal
    {
        public PlayerPrincipal(IIdentity identity)
        {
            Identity = identity;
        }
        #region IPrincipal Members

        public IIdentity Identity{get;set;}
        
        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public class Player : IIdentity
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public PhoneNumber TelephoneNumber { get; set; }
        public List<HashedToken> OAuthTokens { get; set; }
        public List<OAuthID> OAuthIDs { get; set; }

        public Player()
        {
            OAuthIDs = new List<OAuthID>();
            OAuthTokens = new List<HashedToken>();
        }

        #region IIdentity Members

        public string AuthenticationType { get { return "OAuth"; } }

        public bool IsAuthenticated {get;set;}

        #endregion
    }
}
