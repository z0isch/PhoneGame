﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using PhoneGameService.Models;
using PhoneGameService.Models.GameTypes;
using PhoneGameService.Models.OAuthProviders;

namespace PhoneGameService.Repositories
{
    public class TelephoneGameRepository : IDisposable
    {
        #region IDisposable Members
        public void Dispose()
        {
        }
        #endregion

        private static Player[] _players = { new Player { ID="70C5AB60-F65C-4319-85B4-2DF3398E24DC", Name="Vern Fridell", TelephoneNumber= new PhoneNumber() { ID=1, Number="15022967010"} },
                                             new Player { ID="34C94F02-7F26-4C5B-8C5D-3C64DEB979A3", Name="AJ Ruf", TelephoneNumber= new PhoneNumber() { ID=2, Number="15022967466"} }, 
                                             new Player { ID="67516D71-8B31-4B1F-8DC2-6A0E60E92605", Name="Ellie Fridell", TelephoneNumber= new PhoneNumber() { ID=3, Number="15027625695"} }, 
                                           };
        private static GameType[] _gameTypes = { };
        private static PlayerCreationRequest[] _creationRequests = { };

        private static GamePhrase[] _gamePhrases = { new GamePhrase() { id=1, text = "blah" }, new GamePhrase() { id=2, text = "blah2" } };

        private static Dictionary<OAuthID, Player> _oauthToPlayers = new Dictionary<OAuthID, Player>()
        {
            {new OAuthID(){ ID = "113626801228454940516", Provider= new Google()},_players[1] }
        };

        public TelephoneGameRepository()
        {
        }

        internal void AddOAuthID(Player player, OAuthID id)
        {
            player.OAuthIDs.Add(id);
        }
        internal Player GetPlayerByOAuthID(OAuthID id)
        {
            return _oauthToPlayers.FirstOrDefault(kvp => kvp.Key.Equals(id)).Value;
        }
        internal void AddOrUpdateOAuthToken(Player player, OAuthToken token)
        {
            var exisitngToken = player.OAuthTokens.Where(t => t.Provider.Name == token.Provider.Name).FirstOrDefault();
            if (exisitngToken == null)
                player.OAuthTokens.Add(token);
            else
            {
                player.OAuthTokens.Remove(exisitngToken);
                player.OAuthTokens.Add(token);
            }
        }
        
        /// <summary>
        /// Make sure that the saved hashed value of the token from the database is the same as the hashed value of the token we are given
        /// </summary>
        /// <param name="player"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        internal bool VerifyPlayer(Player player, OAuthToken token)
        {
            OAuthToken match = player.OAuthTokens.Where(t => t.Provider.Name == token.Provider.Name && t.HashedToken == token.HashedToken).FirstOrDefault();
            return match != null;
        }
        internal IList<Player> GetPlayers()
        {
            return _players.ToList<Player>();
        }

        internal Player GetPlayerByID(string ID)
        {
            return _players.FirstOrDefault<Player>(p => p.ID == ID);
        }

        internal Player GetPlayerByPhoneNumber(PhoneNumber number)
        {
            return _players.FirstOrDefault<Player>(p => p.TelephoneNumber.CompareTo(number) == 0);
        }

        internal Player GetPlayerByPhoneNumber(string number)
        {
            return _players.FirstOrDefault<Player>(p => p.TelephoneNumber.CompareTo(number) == 0);
        }

        internal IList<GameType> GetAllGameTypes()
        {
            return GameTypeFactory.GetAllGameTypes();
        }

        internal GameType GetGameType<T>() where T : GameType
        {
            return GameTypeFactory.GetGameType<T>();
        }

        internal IList<GamePhrase> GetAllGamePhrases()
        {
            return _gamePhrases.ToList();
        }
    }
}
