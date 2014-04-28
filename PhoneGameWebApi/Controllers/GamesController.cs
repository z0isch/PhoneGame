using Newtonsoft.Json.Linq;
using PhoneGameService.Models;
using PhoneGameService.Repositories;
using PhoneGameService.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PhoneGameService.Logging;
using log4net;

namespace PhoneGameWebApi.Controllers
{
    public class GamesController : ApiController
    {
        private static ILog log = LogManager.GetLogger("GamePlayController");

        /// <summary>
        /// Get a list of games and wether or not it is your turn
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns>[{YourTurn: bool, Game = Game},...]</returns>
        [Route("api/players/{playerId}/games")]
        [HttpGet]
        public IEnumerable<object> GetAllGamesForPlayer(string playerId)
        {
            try
            {
                using (var repository = new TelephoneGameRepository())
                {
                    var player = GameService.GetPlayerByID(playerId, repository);
                    var games = GameService.GetGames(player, repository);
                    return games.Select(g => new { YourTurn = GameService.IsPlayersTurn(player, g, repository), Game = g });
                }
            }
            catch (PhoneGameClientException ex) { throw new PhoneGameAPIException(HttpStatusCode.NotFound, ex.Message); }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw new PhoneGameAPIException(ex.Message); }
        }

        [HttpGet]
        [Route("api/games/{gameId}")]
        public Game GetGameById(int gameId)
        {
            try
            {
                //TODO authorize that the player has access to the given game
                using (var repository = new TelephoneGameRepository())
                {
                    var game = GameService.GetGame(gameId, repository);
                    return game;
                }
            }
            catch (PhoneGameClientException ex) { throw new PhoneGameAPIException(HttpStatusCode.NotFound, ex.Message); }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw new PhoneGameAPIException(ex.Message); }
        }

        [HttpPost]
        [Route("api/games/")]
        public Game AddNewGame([FromBody]JObject obj)
        {
            try
            {
                using (var repository = new TelephoneGameRepository())
                {
                    List<Player> players = obj["playerIds"].Select(id => GameService.GetPlayerByID(id.ToString(), repository)).ToList();

                    if (players.Any(p => p == null))
                    {
                        throw new PhoneGameAPIException(HttpStatusCode.NotFound, "No players selected");
                    }
                    else
                    {
                        var game = GameService.CreateNewGame<TwoPlayersOriginal>(players[0], repository);
                        if(game.CanIPlayWithThisMany(players.Count))
                        {
                            GameService.TransitionGameState(game, game.Edges[0], repository);
                            GameService.AddPlayerToGame(players[1], game, repository);
                            GameService.TransitionGameState(game, game.Edges[0], repository);
                            return game;
                        }
                        else
                        {
                            throw new PhoneGameAPIException(HttpStatusCode.NotImplemented, "Wrong number of players");
                        }
                    }

                }
            }
            catch (PhoneGameClientException ex) { throw new PhoneGameAPIException(HttpStatusCode.NotFound, ex.Message); }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw new PhoneGameAPIException(ex.Message); }
        }

        [HttpDelete]
        [Route("api/games/{gameId}")]
        public void Delete(int gameId)
        {
            try
            {
                using (var repository = new TelephoneGameRepository())
                {
                    var game = GameService.GetGame(gameId, repository);
                    GameService.DeleteGame(game,repository);
                }
            }
            catch (PhoneGameClientException ex) { throw new PhoneGameAPIException(HttpStatusCode.NotFound, ex.Message); }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw new PhoneGameAPIException(ex.Message); }
        }
    }
}
