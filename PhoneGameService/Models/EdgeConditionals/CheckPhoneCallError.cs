using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneGameService.Models.EdgeConditionals
{
    public class CheckPhoneCallError : EdgeConditional
    {
        public override TransitionResult Transition(Game game, Repositories.TelephoneGameRepository repository)
        {
            if (!game.error)
            {
                ChangeState(game, repository);
                return new TransitionResult();
            }

            return new TransitionResult() { Success = false, Message = "An error occurred during the phone call" };
        }
    }
}
