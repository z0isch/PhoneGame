using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Game = PhoneGameService.Models.Game;

namespace PhoneGameWebApi
{
    public class PhoneGameAPIException : HttpResponseException
    {
        private static Game _emptyGame = new Game()
        {
            ID = -1,
        };

        public PhoneGameAPIException(string message)
            : base(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = message })
        {
        }

        public PhoneGameAPIException(Game game, string message)
            : base(new HttpResponseMessage(HttpStatusCode.NotFound) { ReasonPhrase = string.Format("Game {0}: {1}", (game ?? _emptyGame).ID, message) })
        {
        }

        public PhoneGameAPIException(HttpStatusCode status, string message)
            : base(new HttpResponseMessage(status) { ReasonPhrase = message })
        {
        }

        public PhoneGameAPIException(HttpStatusCode status, Game game, string message)
            : base(new HttpResponseMessage(status) { ReasonPhrase = string.Format("Game {0}: {1}", (game ?? _emptyGame).ID, message) })
        {
        }

    }
}