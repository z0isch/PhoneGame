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
        public Game Get(int id)
        {
            //TODO authorize that the player has access to the given game
            using (var repository = new TelephoneGameRepository())
            {
                var game = GameService.GetGame(id, repository);
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
        
        [HttpGet]
        [Route("api/players/{playerId}/games")]
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
    }
}
