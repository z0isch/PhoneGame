using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneGameService.Models
{
    public class Player
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public PhoneNumber TelephoneNumber { get; set; }
        public List<OAuthToken> OAuthTokens { get; set; }
        public List<OAuthID> OAuthIDs { get; set; }

        public Player()
        {
            OAuthIDs = new List<OAuthID>();
            OAuthTokens = new List<OAuthToken>();
        }
    }
}
