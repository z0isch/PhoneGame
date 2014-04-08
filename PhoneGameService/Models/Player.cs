using PhoneGameService.Models.OAuthTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace PhoneGameService.Models
{
    public class Player : IEquatable<Player>, IComparable<Player>
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

        public int CompareTo(Player other)
        {
            if (Equals(other))
                return 0;

            return ID.CompareTo(other.ID);
        }

        public bool Equals(Player other)
        {
            return ID == other.ID;
        }
    }
}
