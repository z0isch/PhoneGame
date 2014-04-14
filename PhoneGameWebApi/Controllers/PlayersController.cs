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

namespace PhoneGameWebApi.Controllers
{
    public class PlayersController : ApiController
    {
        public IEnumerable<Player> Get()
        {
            using (var repository = new TelephoneGameRepository())
            {
                return GameService.GetRecentPlayers(repository);
            }
        }
        public Player Get(string id)
        {
            using (var repository = new TelephoneGameRepository())
            {
                var player = GameService.GetPlayerByID(id, repository);
                if (player == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return player;
            }
        }
    }
}