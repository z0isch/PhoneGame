using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using PhoneGameWebApi.OAuthProviders;

namespace PhoneGameWebApi.OAuthTokens
{
    public class OAuthToken
    {
        public enum TokenType { Encrypted, UnEncrypted };

        public string Id { get; set; }
        public OAuthProvider Provider { get; set; }
        public string Token { get; protected set; }
        public string EncryptedToken { get; protected set; }

        public OAuthToken(string token, TokenType type,string id, OAuthProvider provider)
        {
            this.Id = id;
            this.Provider = provider;
            if (type == TokenType.UnEncrypted)
            {
                this.Token = token;
                this.EncryptedToken = EncryptToken();
            }
            else
            {
                this.EncryptedToken = token;
                this.Token = UnEncryptToken();
            }
        }

        private static string _machineKeyPurpose = "User:{0};Provider:{1}";

        private string GetMachineKeyPurpose()
        {
            return String.Format(_machineKeyPurpose, this.Id, this.Provider.ToString());
        }

        private string EncryptToken()
        {
            var encrypted = MachineKey.Protect(Encoding.UTF8.GetBytes(this.Token), GetMachineKeyPurpose());
            return System.Convert.ToBase64String(encrypted);
        }

        private string UnEncryptToken()
        {
            byte[] token;
            try
            {
                token = MachineKey.Unprotect(System.Convert.FromBase64String(this.EncryptedToken), GetMachineKeyPurpose());
            }
            catch
            {
                return "";
            }
            return Encoding.UTF8.GetString(token);
        }
    }
}