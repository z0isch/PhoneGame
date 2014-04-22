using log4net;
using PhoneGameService.Logging;
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
        private static ILog log = LogManager.GetLogger("PhrasesController");

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
            try
            {
                using (var repository = new TelephoneGameRepository())
                {
                    var game = GameService.GetGame(gameId, repository);
                    var phrase = GameService.GetPhraseById(phraseId, repository);
                    GameService.PickPhraseForGame(phrase, game, repository);
                    GameService.TransitionGameState(game, game.Edges[0], repository);
                    return game;
                }
            }
            catch (PhoneGameClientException ex) { throw new PhoneGameAPIException(HttpStatusCode.NotFound, "Game or phrase doesn't exist"); }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw new PhoneGameAPIException(ex.Message); }
        }
    }
}
