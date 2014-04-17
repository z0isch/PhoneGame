using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game = PhoneGameService.Models.Game;

namespace PhoneGameService.Logging
{
    public class PhoneGameClientException : Exception
    {
        private static Game _emptyGame = new Game()
        {
            ID = -1,
            _currentNodeNumber = -1
        };

        public PhoneGameClientException(Game game, string message, Exception innerException)
            : base(string.Format("Game {0} Current Node {1}: {2}", (game ?? _emptyGame).ID, (game ?? _emptyGame).currentNodeNumber, message), innerException)
        {
        }

        public PhoneGameClientException(Game game, string message)
            : base(string.Format("Game {0} Current Node {1}: {2}", (game ?? _emptyGame).ID, (game ?? _emptyGame).currentNodeNumber, message))
        {
        }
    }
}
