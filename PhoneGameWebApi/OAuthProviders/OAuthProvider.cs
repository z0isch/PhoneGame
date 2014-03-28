using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using PhoneGameWebApi.OAuthTokens;

namespace PhoneGameWebApi.OAuthProviders
{
    public abstract class OAuthProvider
    {
        protected abstract string GetIdFromProvider(string token);
        public abstract OAuthToken GetToken(string code);
        
        //See if this person is who they say they are by hashing the token and comparing it to this users hashed token
        public IPrincipal GetPrincipalFromDatabase(string id, string token)
        {
            var savedId = id + this.ToString();
            var hashedToken = System.Convert.ToBase64String(GenerateSaltedHash(Encoding.UTF8.GetBytes(token), Encoding.UTF8.GetBytes("salt")));

            if (WebApiApplication.Users.ContainsKey(savedId))
            {
                if (WebApiApplication.Users[savedId] == hashedToken)
                    return new GenericPrincipal(new GenericIdentity(WebApiApplication.Users[savedId]), new string[0]);
                else
                    return null;
            }
            else
                return null;
        }
        
        //Save this users hashed token
        public void SaveTokenInDatabase(string id, string token)
        {
            var savedId = id + this.ToString();
            var hashedToken = System.Convert.ToBase64String(GenerateSaltedHash(Encoding.UTF8.GetBytes(token), Encoding.UTF8.GetBytes("salt")));
            if (!WebApiApplication.Users.ContainsKey(savedId))
                WebApiApplication.Users.Add(savedId, hashedToken);
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
        public static bool CompareByteArrays(byte[] array1, byte[] array2)
        {
            if (array1.Length != array2.Length)
            {
                return false;
            }

            for (int i = 0; i < array1.Length; i++)
            {
                if (array1[i] != array2[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}
