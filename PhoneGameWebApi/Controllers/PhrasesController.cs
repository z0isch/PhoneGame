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
    public class PhrasesController : ApiController
    {
        public IList<GamePhrase> Get()
        {
            using (var repository = new TelephoneGameRepository())
            {
                return GameService.GetPhraseList(repository);
            }
        }
        [HttpPost]
        [Route("api/games/{gameId}/phrases/{phraseId}")]
        public Game PickPhrase(int gameId,int phraseId)
        {
            using (var repository = new TelephoneGameRepository())
            {
                var game = GameService.GetGame(gameId,repository);
                var phrase = GameService.GetPhraseById(phraseId,repository);
                if (game == null || phrase == null)
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                else
                {
                    GameService.PickPhraseForGame(phrase, game, repository);
                    GameService.TransitionGameState(game, game.Edges[0], repository);
                    return game;
                }
                
            }
        }
    }
}
