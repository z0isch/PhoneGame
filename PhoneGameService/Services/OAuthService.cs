using PhoneGameService.Models;
using PhoneGameService.Models.OAuthProviders;
using PhoneGameService.Models.OAuthTokens;
using PhoneGameService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace PhoneGameService.Services
{
    public static class OAuthService
    {

        public static Player GetPlayerByOAuthID(TelephoneGameRepository repository, OAuthID id)
        {
            var p = repository.GetPlayerByOAuthID(id);
            return p;
        }

        /// <summary>
        /// Use only when turning a code that comes from an OAuth provider like Gooogle into an OAuthToken.
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="player"></param>
        /// <param name="token"></param>
        public static void SaveTokenFromOAuthProvider(TelephoneGameRepository repository, HashedToken token, OAuthID id)
        {
            var player = repository.GetPlayerByOAuthID(id);
            repository.AddOrUpdateOAuthToken(player, token);
        }

        /// <summary>
        /// Make sure that the token that is presented is the same one that is saved in the database for the player
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="player"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static bool VerifyPlayer(TelephoneGameRepository repository, Player player, UnEncryptedToken token)
        {
            return repository.VerifyPlayer(player, token);
        }

        public static HashedToken HashTokenWithRandomSalt(UnEncryptedToken token)
        {
            return HashToken(token, RandomString(10));
        }
        public static HashedToken HashToken(UnEncryptedToken token, string salt)
        {
            var tokenBytes = Encoding.UTF8.GetBytes(token.Token);
            var saltBytes = Encoding.UTF8.GetBytes(salt);

            HashAlgorithm algorithm = new SHA256Managed();

            byte[] plainTextWithSaltBytes =
                new byte[tokenBytes.Length + saltBytes.Length];

            for (int i = 0; i < tokenBytes.Length; i++)
            {
                plainTextWithSaltBytes[i] = tokenBytes[i];
            }
            for (int i = 0; i < saltBytes.Length; i++)
            {
                plainTextWithSaltBytes[tokenBytes.Length + i] = saltBytes[i];
            }

            return new HashedToken()
            {
                Token = System.Convert.ToBase64String(algorithm.ComputeHash(plainTextWithSaltBytes)),
                Provider = token.Provider,
                Salt = salt
            };
        }
        public static EncryptedToken EncryptToken(UnEncryptedToken token, OAuthID id)
        {
            var encrypted = MachineKey.Protect(Encoding.UTF8.GetBytes(token.Token), id.ID);
            
            return new EncryptedToken()
            {
                Token = System.Convert.ToBase64String(encrypted),
                MachineKeyPurpose = id.ID,
                Provider = token.Provider
            };
        }
        public static EncryptedToken EncryptedToken(string token, OAuthID id)
        {
            return new EncryptedToken() { Token = token, MachineKeyPurpose = id.ID, Provider = id.Provider };
        }
        public static UnEncryptedToken UnEncryptToken(EncryptedToken token)
        {
            byte[] t;
            try
            {
                t = MachineKey.Unprotect(System.Convert.FromBase64String(token.Token), token.MachineKeyPurpose);
            }
            catch
            {
                return null;
            }
            return new UnEncryptedToken()
            {
                Token = Encoding.UTF8.GetString(t),
                Provider = token.Provider
            };
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
