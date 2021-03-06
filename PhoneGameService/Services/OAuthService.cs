﻿using PhoneGameService.Models;
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
using System.Web;
using log4net;
using PhoneGameService.Logging;

namespace PhoneGameService.Services
{
    public static class OAuthService
    {
        private static ILog log = LogManager.GetLogger("OAuthService");

        public static OAuthID GetOAuthIDByPlayer(TelephoneGameRepository repository, Player player, OAuthProvider provider)
        {
            LogHelper.Begin(log, "GetOAuthIDByPlayer()");
            try
            {
                var p = repository.GetOAuthIDByPlayer(player, provider);
                return p;
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "GetOAuthIDByPlayer()"); }
        }

        public static Player GetPlayerByOAuthID(TelephoneGameRepository repository, OAuthID id)
        {
            LogHelper.Begin(log, "GetPlayerByOAuthID()");
            try
            {
                var p = repository.GetPlayerByOAuthID(id);
                return p;
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "GetPlayerByOAuthID()"); }
        }

        /// <summary>
        /// Use only when turning a code that comes from an OAuth provider like Gooogle into an OAuthToken.
        /// </summary>
        public static void SaveTokenFromOAuthProvider(TelephoneGameRepository repository, HashedToken token, OAuthID id)
        {
            LogHelper.Begin(log, "SaveTokenFromOAuthProvider()");
            try
            {
                var player = repository.GetPlayerByOAuthID(id);
                repository.AddOrUpdateOAuthToken(player, token);
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "SaveTokenFromOAuthProvider()"); }
        }

        /// <summary>
        /// Make sure that the token that is presented is the same one that is saved in the database for the player
        /// </summary>
        public static bool VerifyPlayer(TelephoneGameRepository repository, Player player, UnEncryptedToken token)
        {
            LogHelper.Begin(log, "VerifyPlayer()");
            try
            {
                return repository.VerifyPlayer(player, token);
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "VerifyPlayer()"); }
        }

        public static HashedToken HashTokenWithRandomSalt(UnEncryptedToken token)
        {
            LogHelper.Begin(log, "VerifyPlayer()");
            try
            {
                return HashToken(token, RandomString(10));
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "VerifyPlayer()"); }
        }

        public static HashedToken HashToken(UnEncryptedToken token, string salt)
        {
            LogHelper.Begin(log, "HashToken()");
            try
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
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "HashToken()"); }
        }

        public static EncryptedToken EncryptToken(UnEncryptedToken token, OAuthID id)
        {
            LogHelper.Begin(log, "EncryptToken()");
            try
            {
                var encrypted = MachineKey.Protect(Encoding.UTF8.GetBytes(token.Token), id.ID);

                return new EncryptedToken()
                {
                    Token = System.Convert.ToBase64String(encrypted),
                    MachineKeyPurpose = id.ID,
                    Provider = token.Provider
                };
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "EncryptToken()"); }
        }

        public static EncryptedToken GetNewEncryptedToken(string token, OAuthID id)
        {
            LogHelper.Begin(log, "GetNewEncryptedToken()");
            try
            {
                return new EncryptedToken() { Token = token, MachineKeyPurpose = id.ID, Provider = id.Provider };
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "GetNewEncryptedToken()"); }
        }

        public static UnEncryptedToken UnEncryptToken(EncryptedToken token)
        {
            LogHelper.Begin(log, "UnEncryptToken()");
            try
            {
                byte[] t = MachineKey.Unprotect(System.Convert.FromBase64String(token.Token), token.MachineKeyPurpose);
                return new UnEncryptedToken()
                {
                    Token = Encoding.UTF8.GetString(t),
                    Provider = token.Provider
                };
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "UnEncryptToken()"); }
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
