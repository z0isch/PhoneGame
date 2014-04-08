using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PhoneGameService.Models.EdgeConditionals
{
    public class CheckPhraseChosen : EdgeConditional
    {
        public override TransitionResult Transition(Game game, Repositories.TelephoneGameRepository repository)
        {
            if (null != game.gamePhrase)
            {
                ChangeState(game, repository);
                return new TransitionResult();
            }

            return new TransitionResult() { Success = false, Message = "Game phrase not chosen" };
        }
    }
}
