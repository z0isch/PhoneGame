using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using PhoneGameService.Models;

namespace PhoneGameWebApi.Models
{
    public class PlayerIdentity : IIdentity
    {
        public Player Player { get; set; }
        public string Name { get { return Player.Name; } }
        public PlayerIdentity(Player p)
        {
            Player = p;
        }
        #region IIdentity Members

        public string AuthenticationType { get { return "OAuth"; } }

        public bool IsAuthenticated { get; set; }

        #endregion
    }
}