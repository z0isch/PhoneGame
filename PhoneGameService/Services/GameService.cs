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

namespace PhoneGameService.Services
{
    public static class GameService
    {
        public static IList<Player> GetRecentPlayers(TelephoneGameRepository repository)
        {
            return repository.GetPlayers();
        }

        public static Game GetGame(int gameId, TelephoneGameRepository repository)
        {
            return repository.GetGame(gameId);
        }

        public static IList<Game> GetGames(Player player, TelephoneGameRepository repository)
        {
            return repository.GetGames(player);
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
            return repository.GetAllGameTypes();
        }

        public static Game CreateNewGame<T>(Player player1, TelephoneGameRepository repository) where T : GameType
        {
            Game newGame = repository.CreateGame<T>();
            newGame.AddPlayer(player1, repository);
            newGame._currentNodeNumber = newGame._gameType.startNode.id;
            return newGame;
        }

        public static Player FindPlayer(PhoneNumber phoneNumber, TelephoneGameRepository repository)
        {
            return repository.GetPlayerByPhoneNumber(phoneNumber);
        }

        public static Player FindPlayer(string phoneNumber, TelephoneGameRepository repository)
        {
            return repository.GetPlayerByPhoneNumber(phoneNumber);
        }

        public static Player GetPlayerByID(string id, TelephoneGameRepository repository)
        {
            return repository.GetPlayerByID(id);
        }

        public static void AddPlayerToGame(Player player, Game game, TelephoneGameRepository repository)
        {
            game.AddPlayer(player, repository);
        }

        public static void PickPhraseForGame(GamePhrase phrase, Game game, TelephoneGameRepository repository)
        {
            game.PickPhrase(phrase, repository);
        }

        public static IList<GamePhrase> GetPhraseList(TelephoneGameRepository repository)
        {
            return repository.GetAllGamePhrases();
        }

        public static bool IsPlayersTurn(Player player, Game game, TelephoneGameRepository repository)
        {
            if (!game.players.Values.Contains<Player>(player))
            {
                throw new PhoneGameClientException(string.Format("Player {0} is not in game {1}", player.Name, game.ID));
            }

            KeyValuePair<int, Player> kvPair = game.players.FirstOrDefault<KeyValuePair<int, Player>>(p => p.Value.ID == player.ID);
            return kvPair.Key == game.gameType.GetNode(game.currentNodeNumber).activePlayerNumber;
        }

        public static TransitionResult TransitionGameState(Game game, EdgeConditional edge, TelephoneGameRepository repository)
        {
            return edge.Transition(game, repository);
        }

        public static void RefreshGameState(Game game, TelephoneGameRepository repository)
        {
            game = repository.GetGame(game.ID);
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

        public static GamePhrase GetPhraseById(int phraseId, TelephoneGameRepository repository)
        {
            return repository.GetPhraseByID(phraseId);
        }
    }
}