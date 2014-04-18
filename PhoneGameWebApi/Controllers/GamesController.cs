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

namespace PhoneGameWebApi.Controllers
{
    public class GamesController : ApiController
    {
        [Route("api/players/{playerId}/games")]
        [HttpGet]
        public IEnumerable<Game> GetAllGamesForPlayer(string playerId)
        {
            using (var repository = new TelephoneGameRepository())
            {
                var player = GameService.GetPlayerByID(playerId, repository);
                if (player != null)
                {
                    return GameService.GetGames(player, repository);
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

            }
        }

        [HttpGet]
        [Route("api/games/{gameId}")]
        public Game GetGameById(int gameId)
        {
            //TODO authorize that the player has access to the given game

            using (var repository = new TelephoneGameRepository())
            {
                var game = GameService.GetGame(gameId, repository);
                if (game != null)
                {
                    return game;
                }
                else
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }
        }

        [HttpPost]
        [Route("api/games/")]
        public Game AddNewGame([FromBody]JObject obj)
        {
            using (var repository = new TelephoneGameRepository())
            {
                List<Player> players = obj["playerIds"].Select( id=> GameService.GetPlayerByID(id.ToString(), repository)).ToList();

                if (players.Any(p => p==null))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                else
                {
                    if (players.Count == 2)
                    {
                        var game = GameService.CreateNewGame<TwoPlayersOriginal>(players[0],repository);
                        GameService.TransitionGameState(game, game.Edges[0], repository);
                        GameService.AddPlayerToGame(players[1], game, repository);
                        GameService.TransitionGameState(game, game.Edges[0], repository);
                        return game;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.NotImplemented);
                    }
                }

            }
        }
    }
}
