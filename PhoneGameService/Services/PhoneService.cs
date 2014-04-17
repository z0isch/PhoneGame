using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using PhoneGameService.Models;
using PhoneGameService.Repositories;
using Plivo.API;
using RestSharp;

namespace PhoneGameService.Services
{
    public static class PhoneService
    {
        private static readonly string auth_id = "MAYZI0ZDG2ZWFJZDVIYZ";  // obtained from Plivo account dashboard
        private static readonly string auth_token = "MDVhOWZiMWMzNDUwZGI3YzE1ZDNkNGIyNmQ5MmVl";  // obtained from Plivo account dashboard
        private static ILog _log = LogManager.GetLogger("PhoneService");

        public static void MakeCall(int callId, Game game, TelephoneGameRepository repository)
        {

            // Creating the Plivo Client
            RestAPI plivo = new RestAPI(auth_id, auth_token);

            // Making a Call
            string from_number = "15022778448";
            string to_number = "15022967010";


            IRestResponse<Call> response = plivo.make_call(new Dictionary<string, string>() {
                { "from", from_number },
                { "to", to_number }, 
                { "answer_url", string.Format("{0}/{1}/{2}/speak", Server.AnswerURLBase, game.ID, callId) }, 
                { "answer_method", "GET" },
                { "time_limit", "60" },
                { "caller_name", "PhoneGame" },
                { "callback_url", string.Format("{0}/{1}/{2}/callback", Server.AnswerURLBase, game.ID, callId) }
            });

            // The "Outbound call" API response has four properties -
            // message, request_uuid, error, and api_id.
            // error - contains the error response sent back from the server.
            if (response.Data != null)
            {
                if (!string.IsNullOrEmpty(response.Data.error))
                {
                    _log.ErrorFormat("Error response from server: {0}", response.Data.error);
                }
            }
            else
            {
                // ErrorMessage - contains error related to network failure.
                _log.ErrorFormat(response.ErrorMessage);
            }
        }
    }
}
