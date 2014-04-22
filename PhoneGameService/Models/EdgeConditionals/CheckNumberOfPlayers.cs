using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneGameService.Models.EdgeConditionals
{
    public class CheckNumberOfPlayers : EdgeConditional
    {
        public override TransitionResult Transition(Game game, Repositories.TelephoneGameRepository repository)
        {
            if (!game.enoughPlayers)
                return new TransitionResult() { Success = false, Message = "Not enough players to begin game" };

            if(game.numberOfPlayers > game.maxNumberOfPlayers)
                return new TransitionResult() { Success = false, Message = "Too many players to begin game" };

            ChangeState(game, repository);
            return new TransitionResult();
        }
    }
}
