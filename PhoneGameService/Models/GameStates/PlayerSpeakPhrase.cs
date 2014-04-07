using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoneGameService.Services;

namespace PhoneGameService.Models.GameStates
{
    public class PlayerSpeakPhrase : GameStateNode
    {
        public override void Execute(Game game, Repositories.TelephoneGameRepository repository)
        {
            PhoneService.MakeCall(1, game, repository);
        }
    }
}
