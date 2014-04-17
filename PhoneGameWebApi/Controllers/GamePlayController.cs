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
                    if (null == game)
                    {
                        throw new PhoneGameAPIException("No such game");
                    }
                    return game;
                }
            }
            catch (PhoneGameAPIException) { throw; }
            catch (Exception ex)
            {
                ExceptionHandler.LogAll(log, ex); 
                throw new PhoneGameAPIException(HttpStatusCode.InternalServerError, ex.Message); 
            }
        }

        [Route("api/gameplay/{gameId}/edges")]
        [HttpGet]
        public IList<EdgeConditional> GetCurrentGameEdges(int gameId)
        {
            // TODO authorize the player is in this game
            using (var repository = new TelephoneGameRepository())
            {
                Game game = GameService.GetGame(gameId, repository);
                if (null == game)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return game.Edges;
            }
        }

        [Route("api/gameplay/{gameId}/next")]
        [HttpPost]
        public IList<EdgeConditional> TransitionGameState(int gameId, [FromUri] int edgeId)
        {
            // TODO authorize the player is in this game
            using (var repository = new TelephoneGameRepository())
            {
                Game game = GameService.GetGame(gameId, repository);
                if (null == game)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                EdgeConditional edge = game.Edges.FirstOrDefault( e => e.id == edgeId);
                if (null == edge)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                TransitionResult result = GameService.TransitionGameState(game, edge, repository);
                if (!result.Success)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = result.Message });
                }
                
                return game.Edges;
            }
        }

        [Route("api/gameplay/{gameId}/addplayer/")]
        [HttpPost]
        public void AddPlayerToGame(int gameId, [FromUri] string playerId)
        {
            // TODO authorize the player is in this game
            using (var repository = new TelephoneGameRepository())
            {
                Game game = GameService.GetGame(gameId, repository);
                if (null == game)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                Player addPlayer = GameService.GetPlayerByID(playerId, repository);
                if (null == addPlayer)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                try
                {
                    GameService.AddPlayerToGame(addPlayer, game, repository);
                }
                catch (PhoneGameClientException ex)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = ex.Message });
                }
            }
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
                    if (null == game)
                    {
                        throw new HttpResponseException(HttpStatusCode.NotFound);
                    }
                    GamePhrase phrase = GameService.GetPhraseById(phraseId, repository);
                    if (null == phrase)
                    {
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "Bad phrase id" });
                    }

                    GameService.PickPhraseForGame(phrase, game, repository);
                }
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = ex.Message });
            }
        }

    }
}
