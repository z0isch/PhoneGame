using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Plivo.XML;
using PhoneGameService.Repositories;
using PhoneGameService.Services;
using PhoneGameService.Models;

namespace PhoneGameWebApi.Controllers
{
    public class PhoneServiceController : ApiController
    {
        [Route("/api/phonesvc/{gameId}/{audioId}/speak")]
        [AcceptVerbs("GET")]
        public Response HandleSpeak(int gameId, int audioId)
        {
            using (var repository = new TelephoneGameRepository())
            {
                Game game = GameService.GetGame(gameId, repository);

                Response myResponse = new Response();
                myResponse.AddSpeak("Speak your phrase at the beep.", null);
                myResponse.AddRecord(new Dictionary<string, string>() {
                    { "action", string.Format("{0}/{1}/{2}/recordresult", Server.AnswerURLBase, game.ID, audioId) },
                });

                return myResponse;
            }
        }
    }
}
