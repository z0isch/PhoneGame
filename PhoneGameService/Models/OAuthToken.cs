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
        public enum TokenType { Encrypted, UnEncrypted, Hashed };

        public OAuthProvider Provider { get; set; }
        public string Token { get; protected set; }
        public string EncryptedToken { get; protected set; }
        public string HashedToken { get; protected set; }
        private string Salt { get; set; }

        //Need to figure out a way to manage the salt in the database
        public OAuthToken(string token, TokenType type, OAuthProvider provider, string salt) : this(token, type, provider)
        {
            this.Salt = salt;
        }
        public OAuthToken(string token, TokenType type, OAuthProvider provider)
        {
            this.Salt = "need to make this different for every person";
            this.Provider = provider;
            switch(type){
                case TokenType.UnEncrypted:            
                    this.Token = token;
                    this.EncryptedToken = EncryptToken(token,this.Salt,provider);
                    this.HashedToken = System.Convert.ToBase64String(GenerateSaltedHash(Encoding.UTF8.GetBytes(token), Encoding.UTF8.GetBytes(this.Salt)));
                    break;
                case TokenType.Encrypted:
                    this.EncryptedToken = token;
                    this.Token = UnEncryptToken(token, this.Salt, provider);
                    this.HashedToken = System.Convert.ToBase64String(GenerateSaltedHash(Encoding.UTF8.GetBytes(this.Token), Encoding.UTF8.GetBytes(this.Salt)));
                    break;
                case TokenType.Hashed:
                    this.HashedToken = token;
                    break;
                default:
                    break;
            }

        }

        private static string EncryptToken(string token, string salt, OAuthProvider provider)
        {
            var encrypted = MachineKey.Protect(Encoding.UTF8.GetBytes(token),salt);
            return System.Convert.ToBase64String(encrypted);
        }

        private static string UnEncryptToken(string encryptedtoken, string salt, OAuthProvider provider)
        {
            byte[] t;
            try
            {
                t = MachineKey.Unprotect(System.Convert.FromBase64String(encryptedtoken), salt);
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

        private static Random _random = new Random((int)DateTime.Now.Ticks);
        private static string RandomString(int size)
        {
            StringBuilder builder = new StringBuilder();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * _random.NextDouble() + 65)));
                builder.Append(ch);
            }

            return builder.ToString();
        }

    }
}