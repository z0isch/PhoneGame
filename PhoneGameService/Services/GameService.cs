using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoneGameService.Models;
using PhoneGameService.Repositories;
using PhoneGameService.Models.GameTypes;
using PhoneGameService.Models.EdgeConditionals;
using PhoneGameService.Models.GameStates;
using System.IO;
using PhoneGameService.Logging;
using log4net;

namespace PhoneGameService.Services
{
    public static class GameService
    {
        private static ILog log = LogManager.GetLogger("GameService");

        public static IList<Player> GetRecentPlayers(TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "GetRecentPlayers()");
            try
            {
                return repository.GetPlayers();
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "GetRecentPlayers()"); }
        }

        public static Game GetGame(int gameId, TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "GetGame()");
            try
            {
                return repository.GetGame(gameId);
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "GetGame()"); }
        }

        public static IList<Game> GetGames(Player player, TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "GetGames()");
            try
            {
                return repository.GetGames(player);
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "GetGames()"); }
        }

        public static void RequestNewPlayer(PlayerCreationRequest request, TelephoneGameRepository repository)
        {
            throw new NotImplementedException();
        }

        public static Player ValidatePlayerRequest(PlayerCreationValidation validation, TelephoneGameRepository repository)
        {
            throw new NotImplementedException();
        }

        public static IList<GameType> GetGameTypes(TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "GetAllGameTypes()");
            try
            {
                return repository.GetAllGameTypes();
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "GetAllGameTypes()"); }
        }

        public static Game CreateNewGame<T>(TelephoneGameRepository repository) where T : GameType
        {
            LogHelper.Begin(log, string.Format("CreateNewGame<{0}>()", typeof(T).Name));
            try
            {
                Game newGame = repository.CreateGame<T>();
                newGame._currentNodeNumber = newGame._gameType.startNode.id;
                return newGame;
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "CreateNewGame()"); }
        }

        public static Game CreateNewGame<T>(Player player1, TelephoneGameRepository repository) where T : GameType
        {
            LogHelper.Begin(log, string.Format("CreateNewGame<{0}>(Player)", typeof(T).Name));
            try
            {
                Game newGame = repository.CreateGame<T>();
                newGame.AddPlayer(player1, repository);
                newGame._currentNodeNumber = newGame._gameType.startNode.id;
                return newGame;
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "CreateNewGame(Player)"); }
        }

        public static Player FindPlayer(PhoneNumber phoneNumber, TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "FindPlayer(PhoneNumber)");
            try
            {
                return repository.GetPlayerByPhoneNumber(phoneNumber);
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "FindPlayer(PhoneNumber)"); }
        }

        public static Player FindPlayer(string phoneNumber, TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "FindPlayer(string)");
            try
            {
                return repository.GetPlayerByPhoneNumber(phoneNumber);
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "FindPlayer(string)"); }
        }

        public static Player GetPlayerByID(string id, TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "GetPlayerByID()");
            try
            {
                return repository.GetPlayerByID(id);
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "GetPlayerByID()"); }
        }

        public static void AddPlayerToGame(Player player, Game game, TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "AddPlayerToGame()");
            try
            {
                game.AddPlayer(player, repository);
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "AddPlayerToGame()"); }
        }

        public static void PickPhraseForGame(GamePhrase phrase, Game game, TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "PickPhraseForGame()");
            try
            {
                game.PickPhrase(phrase, repository);
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "PickPhraseForGame()"); }
        }

        public static IList<GamePhrase> GetPhraseList(TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "GetPhraseList()");
            try
            {
                return repository.GetAllGamePhrases();
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "GetPhraseList()"); }
        }

        public static bool IsPlayersTurn(Player player, Game game, TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "IsPlayersTurn()");
            try
            {
                if (!game.players.Values.Contains<Player>(player))
                {
                    throw new PhoneGameClientException(game, string.Format("Player {0} is not in game {1}", player.Name, game.ID));
                }

                KeyValuePair<int, Player> kvPair = game.players.FirstOrDefault<KeyValuePair<int, Player>>(p => p.Value.ID == player.ID);
                return kvPair.Key == game.gameType.GetNode(game.currentNodeNumber).activePlayerNumber;
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "IsPlayersTurn()"); }
        }

        public static TransitionResult TransitionGameState(Game game, EdgeConditional edge, TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "TransitionGameState()");
            try
            {
                return edge.Transition(game, repository);
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "TransitionGameState()"); }
        }

        public static void RefreshGameState(Game game, TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "RefreshGameState()");
            try
            {
                game = repository.GetGame(game.ID);
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "RefreshGameState()"); }
        }

        public static GameAudio GetGameAudio(AudioIdentifier audioID, TelephoneGameRepository repository)
        {
            throw new NotImplementedException();
        }

        public static void PutGameAudio(GameAudio audio, Game game, TelephoneGameRepository repository)
        {
            throw new NotImplementedException();
        }

        public static GameScore GetGameScore(Game game, TelephoneGameRepository repository)
        {
            throw new NotImplementedException();
        }

        public static TotalPlayerScore GetTotalPlayerScore(Player player, TelephoneGameRepository repository)
        {
            throw new NotImplementedException();
        }

        public static void RenderGameTypeDotGraph<T>(string filename) where T : GameType
        {
            LogHelper.Begin(log, "RenderGameTypeDotGraph()");
            try
            {
                GameType gameType = GameTypeFactory.GetGameType<T>();
                var output = new StringBuilder();
                output.AppendFormat("digraph {0}", gameType.GetType().Name);
                output.AppendLine("{");
                output.AppendLine("graph [overlap=\"false\",mode=\"hier\",splines=\"true\",root=\"INQUIRED\",levelsgap=.2]");
                output.AppendLine("edge [fontsize=6,labelfloat=\"false\",labelangle=35,labeldistance=1.75]");
                output.AppendLine("node [fontsize=8]");
                foreach (GameStateNode n in gameType.nodes.Values)
                {
                    foreach(EdgeConditional edge in n.edgeConditionals)
                    {
                        output.AppendFormat("{0}->{1}[headlabel=\"{2}\"]\n", n.uniqueName, edge.nextNode.uniqueName, edge.text);
                    }
                }

                foreach (GameStateNode n in gameType.nodes.Values)
                {
                    output.AppendFormat("{0};\n", n.uniqueName);
                }

                output.Append("}");

                var file = new FileInfo(filename);
                using (FileStream fs = file.OpenWrite())
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(output.ToString());
                    sw.Close();
                }
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "RenderGameTypeDotGraph()"); }
        }

        public static GamePhrase GetPhraseById(int phraseId, TelephoneGameRepository repository)
        {
            LogHelper.Begin(log, "GetPhraseById()");
            try
            {
                return repository.GetPhraseByID(phraseId);
            }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw; }
            finally { LogHelper.End(log, "GetPhraseById()"); }
        }
    }
}