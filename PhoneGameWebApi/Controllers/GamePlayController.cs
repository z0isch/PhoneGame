using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PhoneGameService.Models;
using PhoneGameService.Services;
using PhoneGameService.Repositories;
using PhoneGameService.Models.EdgeConditionals;
using PhoneGameService.Logging;
using log4net;

namespace PhoneGameWebApi.Controllers
{
    public class GamePlayController : ApiController
    {
        private static ILog log = LogManager.GetLogger("GamePlayController");

        [Route("api/gameplay/{gameId}")]
        [HttpGet]
        public Game GetGameState(int gameId)
        {
            try
            {
                // TODO authorize the player is in this game
                using (var repository = new TelephoneGameRepository())
                {
                    Game game = GameService.GetGame(gameId, repository);
                    return game;
                }
            }
            catch (PhoneGameClientException ex) { throw new PhoneGameAPIException(HttpStatusCode.NotFound, ex.Message); }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw new PhoneGameAPIException(ex.Message); }
        }

        [Route("api/gameplay/{gameId}/edges")]
        [HttpGet]
        public IList<EdgeConditional> GetCurrentGameEdges(int gameId)
        {
            try
            {
                // TODO authorize the player is in this game
                using (var repository = new TelephoneGameRepository())
                {
                    Game game = GameService.GetGame(gameId, repository);
                    return game.Edges;
                }
            }
            catch (PhoneGameClientException ex) { throw new PhoneGameAPIException(HttpStatusCode.NotFound, ex.Message); }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw new PhoneGameAPIException(ex.Message); }
        }

        [Route("api/gameplay/{gameId}/next")]
        [HttpPost]
        public IList<EdgeConditional> TransitionGameState(int gameId, [FromUri] int edgeId)
        {
            try
            {
                // TODO authorize the player is in this game
                using (var repository = new TelephoneGameRepository())
                {
                    Game game = GameService.GetGame(gameId, repository);

                    EdgeConditional edge = game.Edges.FirstOrDefault(e => e.id == edgeId);
                    if (null == edge)
                    {
                        throw new PhoneGameAPIException(HttpStatusCode.NotFound, game, string.Format("No such transition available (Bad edge id {0})", edgeId));
                    }
                    TransitionResult result = GameService.TransitionGameState(game, edge, repository);
                    if (!result.Success)
                    {
                        throw new PhoneGameAPIException(HttpStatusCode.MethodNotAllowed, game, result.Message);
                    }

                    return game.Edges;
                }
            }
            catch (PhoneGameClientException ex) { throw new PhoneGameAPIException(HttpStatusCode.NotFound, ex.Message); }
            catch (PhoneGameAPIException) { throw; }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw new PhoneGameAPIException(ex.Message); }
        }

        [Route("api/gameplay/{gameId}/addplayer/")]
        [HttpPost]
        public void AddPlayerToGame(int gameId, [FromUri] string playerId)
        {
            try
            {
                // TODO authorize the player is in this game
                using (var repository = new TelephoneGameRepository())
                {
                    Game game = GameService.GetGame(gameId, repository);
                    Player addPlayer = GameService.GetPlayerByID(playerId, repository);
                    GameService.AddPlayerToGame(addPlayer, game, repository);
                }
            }
            catch (PhoneGameClientException ex) { throw new PhoneGameAPIException(HttpStatusCode.NotFound, ex.Message); }
            catch (PhoneGameAPIException) { throw; }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw new PhoneGameAPIException(ex.Message); }
        }

        [Route("api/gameplay/{gameId}/pickphrase/")]
        [HttpPost]
        public void PickPhraseForGame(int gameId, [FromUri] int phraseId)
        {
            try
            {
                // TODO authorize the player is in this game
                using (var repository = new TelephoneGameRepository())
                {
                    Game game = GameService.GetGame(gameId, repository);
                    GamePhrase phrase = GameService.GetPhraseById(phraseId, repository);
                    GameService.PickPhraseForGame(phrase, game, repository);
                }
            }
            catch (PhoneGameClientException ex) { throw new PhoneGameAPIException(HttpStatusCode.NotFound, ex.Message); }
            catch (PhoneGameAPIException) { throw; }
            catch (Exception ex) { ExceptionHandler.LogAll(log, ex); throw new PhoneGameAPIException(ex.Message); }
        }

    }
}
