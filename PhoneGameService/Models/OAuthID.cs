using PhoneGameService.Models.OAuthProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneGameService.Models
{
    public class OAuthID : IEquatable<OAuthID>
    {
        public string ID { get; set; }
        public OAuthProvider Provider { get; set; }



        public bool Equals(OAuthID other)
        {
            return other.ID == this.ID && other.Provider.Name == this.Provider.Name;
        }
    }
}
