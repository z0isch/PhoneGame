using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;

namespace PhoneGameWebApi.Models
{
    public class PlayerPrincipal : IPrincipal
    {
        public PlayerPrincipal(IIdentity identity)
        {
            Identity = identity;
        }
        #region IPrincipal Members

        public IIdentity Identity { get; set; }

        public bool IsInRole(string role)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}