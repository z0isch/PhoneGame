using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PhoneGameService.Repositories;
using PhoneGameService.Services;
using PhoneGameService.Models;
using PhoneGameService.Models.Plivo;
using log4net;
using Newtonsoft.Json;
using System.Net.Http.Formatting;

namespace PhoneGameWebApi.Controllers
{
//    [UseXmlFormatter]
    public class PhoneServiceController : ApiController
    {
        private static ILog _log = LogManager.GetLogger("PhoneServiceController");

        [Route("api/phonesvc/{gameId}/{audioId}/speak")]
        [AcceptVerbs("GET", "POST")]
        public PlivoResponse HandleSpeak(int gameId, int audioId)
        {
            using (var repository = new TelephoneGameRepository())
            {
                Game game = GameService.GetGame(gameId, repository);

                PlivoResponse myResponse = new PlivoResponse();
                myResponse.speak = new Speak() { body = "Please speak the phrase after the beep.  When you are done, just hang up.  Oooooooooooooooooo." };
                myResponse.record = new Record() { action = string.Format("{0}/{1}/{2}/recordresult", Server.AnswerURLBase, game.ID, audioId) };

                return myResponse;
            }
        }

        [Route("api/phonesvc/{gameId}/{audioId}/recordresult")]
        [AcceptVerbs("POST")]
        public void HandleRecording(int gameId, int audioId, [FromBody] FormDataCollection response)
        {
            if (null != response)
            {
                Plivo.API.MessageResponse messageResponse = JsonConvert.DeserializeObject<Plivo.API.MessageResponse>(response["response"]);

                using (var repository = new TelephoneGameRepository())
                {
                    _log.DebugFormat("Response was {0}.\tAPI ID: {1}\tMessage UUID: {2}", messageResponse.message, messageResponse.api_id, messageResponse.message_uuid);
                    Game game = GameService.GetGame(gameId, repository);
                }
            }
            else
            {
                _log.Error("HandleRecording: No Response Data");
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "No response data to handle!" });
            }
        }

        [Route("api/phonesvc/{gameId}/{audioId}/callback")]
        [AcceptVerbs("POST")]
        public void HandleCallback(int gameId, int audioId, [FromBody] FormDataCollection response)
        {
            if (null != response)
            {
                Plivo.API.MessageResponse messageResponse = JsonConvert.DeserializeObject<Plivo.API.MessageResponse>(response["response"]);

                using (var repository = new TelephoneGameRepository())
                {
                    _log.DebugFormat("Response was {0}.\tAPI ID: {1}\tMessage UUID: {2}", messageResponse.message, messageResponse.api_id, messageResponse.message_uuid);
                    if (!string.IsNullOrEmpty(messageResponse.error))
                        _log.ErrorFormat("Error from Plivo: {0}", messageResponse.error);

                }
            }
            else
            {
                _log.Error("HandleRecording: No Response Data");
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = "No response data to handle!" });
            }
        }

    }
}
