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
        public Game AddPlayer([FromUri]string gameId, [FromUri] string phoneNumber)
        {
            using (var repository = new TelephoneGameRepository())
            {
				// Testing other
                //Need a service method to get a game based on id
                var player = GameService.FindPlayer(phoneNumber, repository);
                if (player != null)
                {

                }
                else
                    throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return null;
        }
        public Game Start([FromUri]string phoneNumber)
        {
            using (var repository = new TelephoneGameRepository())
            {
                var player = GameService.FindPlayer(phoneNumber, repository);
                if (player != null)
                {
                    var g = GameService.CreateNewGame<TwoPlayersOriginal>(player, repository);
                    if (GameService.TransitionGameState(g, g.Edges[0], repository))
                    {
                        return g;
                    }
                    else
                    {
                        throw new HttpResponseException(HttpStatusCode.InternalServerError);
                    }
                }
                else
                    throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }
    }
}
