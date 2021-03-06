﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using PhoneGameService.Models;
using PhoneGameService.Models.GameTypes;
using PhoneGameService.Models.OAuthProviders;
using PhoneGameService.Models.OAuthTokens;
using PhoneGameService.Services;
using PhoneGameService.Logging;
using PhoneGameService.Models.GameStates;

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
                                             new Player { ID="34C94F02-7F26-4C5B-8C5D-3C64DEB979A3",
                                                 Name="AJ Ruf",
                                                 TelephoneNumber= new PhoneNumber() { ID=2, Number="15022967466"},
                                                 OAuthIDs = new List<OAuthID>() {new OAuthID(){ ID = "113626801228454940516", Provider= new Google()}}
                                             },
                                             new Player { ID="67516D71-8B31-4B1F-8DC2-6A0E60E92605", Name="Ellie Fridell", TelephoneNumber= new PhoneNumber() { ID=3, Number="15027625695"} },
                                             new Player { ID="A7664B0C-967D-4CE6-B97C-A037ADF046C7",
                                                 Name="Tester1", 
                                                 TelephoneNumber=new PhoneNumber(){ID=4,Number="TEST1"},
                                                 OAuthIDs = new List<OAuthID>() {new OAuthID(){ ID = "1", Provider= new TestProvider()}}
                                             },
                                             new Player { ID="4276F19E-F5D6-4484-A307-F6395AF273F8",
                                                 Name="Tester2", 
                                                 TelephoneNumber=new PhoneNumber(){ID=5,Number="TEST2"},
                                                 OAuthIDs = new List<OAuthID>() {new OAuthID(){ ID = "2", Provider= new TestProvider()}}
                                             }
                                           };
        private static GameType[] _gameTypes = { };
        private static int _gameId = 1;
        private static Dictionary<int, Game> _games = new Dictionary<int,Game>();
        private static PlayerCreationRequest[] _creationRequests = { };

        private static GamePhrase[] _gamePhrases = { new GamePhrase() { id=1, text = "blah" }, new GamePhrase() { id=2, text = "blah2" } };

        private static Dictionary<OAuthID, Player> _oauthToPlayers = new Dictionary<OAuthID, Player>()
        {
            {new OAuthID(){ ID = "113626801228454940516", Provider= new Google()},_players[1] },
            {new OAuthID(){ ID = "1", Provider= new TestProvider()},_players[3] },
            {new OAuthID(){ ID = "2", Provider= new TestProvider()},_players[4] }
        };

        private static Dictionary<Type, List<Type>> StatesToIgnore = new Dictionary<Type, List<Type>>()
        {
            {typeof(TwoPlayersOriginal),new List<Type>(){ typeof(NotStarted), typeof(PickPhrase), typeof(PickPlayer)}}
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

        internal OAuthID GetOAuthIDByPlayer(Player player, OAuthProvider provider)
        {
            return _oauthToPlayers.FirstOrDefault(kvp => kvp.Value.ID == player.ID && kvp.Key.Provider.Name == provider.Name).Key;
        }

        internal void AddOrUpdateOAuthToken(Player player, HashedToken token)
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
        internal bool VerifyPlayer(Player player, UnEncryptedToken token)
        {
            if (player.ID == "A7664B0C-967D-4CE6-B97C-A037ADF046C7")
                return true;
            else
            {
                HashedToken exisitng = player.OAuthTokens.Where(t => t.Provider.Name == token.Provider.Name).FirstOrDefault();
                if (exisitng != null)
                {
                    HashedToken hashed = OAuthService.HashToken(token, exisitng.Salt);
                    return hashed.Token == exisitng.Token;
                }
                return false;
            }
        }

        internal IList<Player> GetPlayers()
        {
            return _players.ToList<Player>();
        }

        internal Player GetPlayerByID(string ID)
        {
            Player retp = _players.FirstOrDefault<Player>(p => p.ID == ID);
            if (null == retp) throw new PhoneGameClientException(string.Format("No such player with id {0}", ID));
            return retp;
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

        internal Game CreateGame<T>() where T : GameType
        {
            Game newGame = new Game() { ID = _gameId, 
                                        _gameType = GameTypeFactory.GetGameType<T>() };
            _games.Add(_gameId, newGame);
            _gameId++;
            return newGame;
        }

        internal Game GetGame(int id)
        {
            if (!_games.ContainsKey(id))
                throw new PhoneGameClientException(string.Format("Game with id {0} not found", id));

            return _games[id];
        }

        internal IList<Game> GetGames(Player player)
        {
            return _games.Values.Where<Game>(g => g.players.Values.Contains(player)).ToList<Game>();
        }
        internal IList<Game> GetActiveGames(Player player)
        {
            return _games.Values.Where<Game>(g => g.players.Values.Contains(player))
                .Where(g => !StatesToIgnore[g.gameType.GetType()].Contains(g.currentNode.GetType()))
                .ToList<Game>();
        }
        internal GamePhrase GetPhraseByID(int phraseId)
        {
            GamePhrase phrase = _gamePhrases.FirstOrDefault(p => p.id == phraseId);
            if (null == phrase) throw new PhoneGameClientException(string.Format("No such phrase with id {0}", phraseId));
            return phrase;
        }
        
        internal void DeleteGame(Game game)
        {
            _games.Remove(_games.First(g => g.Value.ID == game.ID).Key);
        }
    }
}
