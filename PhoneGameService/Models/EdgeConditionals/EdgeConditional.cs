using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhoneGameService.Models.GameStates;
using PhoneGameService.Repositories;
using Newtonsoft.Json;

namespace PhoneGameService.Models.EdgeConditionals
{
    public abstract class EdgeConditional
    {
        public virtual int id { get; set; }
        public virtual string text { get; set; }
        [JsonIgnore]
        public virtual GameStateNode nextNode { get; set; }

        public abstract bool Transition(Game game, TelephoneGameRepository repository);

        protected virtual void ChangeState(Game game, TelephoneGameRepository repository)
        {
            if (!game.currentNode.edgeConditionals.Contains(this))
            {
                throw new Exception("EdgeConditional is not in the GameStateNode");
            }

            game._currentNodeNumber = nextNode.id;
        }
    }
}
