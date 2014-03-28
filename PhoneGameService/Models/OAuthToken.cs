using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Security;
using PhoneGameService.Models.OAuthProviders;

namespace PhoneGameService.Models
{
    public class OAuthToken
    {
        public enum TokenType { Encrypted, UnEncrypted };

        public string Id { get; set; }
        public OAuthProvider Provider { get; set; }
        public string Token { get; protected set; }
        public string EncryptedToken { get; protected set; }
        public string HashedToken { get; protected set; }

        public OAuthToken(string token, TokenType type,string id, OAuthProvider provider)
        {
            this.Id = id;
            this.Provider = provider;
            if (type == TokenType.UnEncrypted)
            {
                this.Token = token;
                this.EncryptedToken = EncryptToken(token,id,provider);
                this.HashedToken = System.Convert.ToBase64String(GenerateSaltedHash(Encoding.UTF8.GetBytes(token), Encoding.UTF8.GetBytes(this.Id)));
            }
            else
            {
                this.EncryptedToken = token;
                this.Token = UnEncryptToken(token,id,provider);
                this.HashedToken = System.Convert.ToBase64String(GenerateSaltedHash(Encoding.UTF8.GetBytes(this.Token), Encoding.UTF8.GetBytes(this.Id)));
            }
        }

        private static string _machineKeyPurpose = "User:{0};Provider:{1}";
        private static string EncryptToken(string token, string id, OAuthProvider provider)
        {
            var encrypted = MachineKey.Protect(Encoding.UTF8.GetBytes(token), String.Format(_machineKeyPurpose, id, provider.ToString()));
            return System.Convert.ToBase64String(encrypted);
        }

        private static string UnEncryptToken(string encryptedtoken, string id, OAuthProvider provider)
        {
            byte[] t;
            try
            {
                t = MachineKey.Unprotect(System.Convert.FromBase64String(encryptedtoken), String.Format(_machineKeyPurpose, id, provider.ToString()));
            }
            catch
            {
                return "";
            }
            return Encoding.UTF8.GetString(t);
        }
        
        public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
                new byte[plainText.Length + salt.Length];

            for (int i = 0; i < plainText.Length; i++)
            {
                plainTextWithSaltBytes[i] = plainText[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                plainTextWithSaltBytes[plainText.Length + i] = salt[i];
            }

            return algorithm.ComputeHash(plainTextWithSaltBytes);
        }
    }
}