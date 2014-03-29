using PhoneGameService.Models;
using PhoneGameService.Models.OAuthProviders;
using PhoneGameService.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

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
        public static void SaveTokenFromOAuthProvider(TelephoneGameRepository repository, OAuthToken token, OAuthID id)
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
        public static bool VerifyPlayer(TelephoneGameRepository repository, Player player, OAuthToken token)
        {
            return repository.VerifyPlayer(player, token);
        }
    }
}
