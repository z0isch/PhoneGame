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

        //Responds to api/playersFromPhoneNumbers?phoneNumbers[0]=123&phoneNumbers[1]=345...
        [HttpGet]
        [Route("api/playersFromPhoneNumbers")]
        public Dictionary<string, string> PlayersFromPhoneNumbers([FromUri] List<string> phoneNumbers)
        {
            var players = new Dictionary<string, string>();
            using (var repository = new TelephoneGameRepository())
            {
                foreach (var num in phoneNumbers)
                {
                    var number = num.Replace("+", "").Replace("-", "").Replace("(","").Replace(")","").Replace(" ","");
                    var p = GameService.FindPlayer(number, repository);
                    if (p != null)
                    {
                        players.Add(num, p.ID);
                    }
                }
            }
            return players;
        }
    }
}