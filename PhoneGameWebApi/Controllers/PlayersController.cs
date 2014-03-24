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
    public class PlayersController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<Player> Get()
        {
            using (var repository = new TelephoneGameRepository())
            {
                return GameService.GetRecentPlayers(repository);
            }
        }

        // GET api/<controller>/5
        public object Get(string id)
        {
            using (var repository = new TelephoneGameRepository())
            {
                var player = GameService.FindPlayer(id, repository);
                if (player == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return new
                {
                    Player = player,
                    //Games = GameService.GetGames(player, repository)
                };
            }
        }

    }
}