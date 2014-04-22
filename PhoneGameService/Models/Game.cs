using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using PhoneGameService.Repositories;
using PhoneGameService.Models.GameStates;
using PhoneGameService.Models.EdgeConditionals;
using PhoneGameService.Logging;

namespace PhoneGameService.Models
{
    public class Game
    {
        public int ID { get; set; }

        internal GameType _gameType { get; set; }
        public GameType gameType { get { return _gameType; } }

        internal int _currentNodeNumber;
        public int currentNodeNumber { get { return _currentNodeNumber; } }

        public GameStateNode currentNode { get { return gameType.GetNode(_currentNodeNumber); } }

        internal GamePhrase _gamePhrase;
        public GamePhrase gamePhrase { get { return _gamePhrase; } }

        internal bool _error = false;
        public bool error { get { return _error; } }

        private Dictionary<int, Player> _players = new Dictionary<int, Player>();
        private int _nextPlayerNumber = 1;
        public IDictionary<int, Player> players { get { return _players; } }

        private Dictionary<int, GameAudio> _audioList = new Dictionary<int, GameAudio>();
        private int _nextAudioSequence = 1;
        public Dictionary<int, GameAudio> audioList { get; set; }

        public IList<EdgeConditional> Edges { get { return currentNode.edgeConditionals; } }

        public bool enoughPlayers { get { return _players.Count >= minNumberOfPlayers; } }
        public int numberOfPlayers { get { return _players.Count; } }
        public int maxNumberOfPlayers { get { return _gameType.maxNumberOfPlayers; } }
        public int minNumberOfPlayers { get { return _gameType.minNumberOfPlayers; } }

        public bool CanIPlayWithThisMany(int numPlayers) { return numPlayers >= minNumberOfPlayers && numPlayers <= maxNumberOfPlayers; }

        internal bool PlayerInGame(Player player, TelephoneGameRepository repository)
        {
            return _players.ContainsValue(player);
        }

        internal void AddPlayer(Player player, TelephoneGameRepository repository)
        {
            if (_players.Count >= gameType.maxNumberOfPlayers)
            {
                throw new PhoneGameClientException(this, string.Format("Cannot add more than the maximum number of players for this game type: {0}", gameType.maxNumberOfPlayers));
            }

            if (null != _players.Values.FirstOrDefault<Player>(p => p.ID == player.ID))
            {
                throw new PhoneGameClientException(this, "Player is already added to this game!");
            }

            _players[_nextPlayerNumber++] = player;
        }

        internal void PickPhrase(GamePhrase phrase, TelephoneGameRepository repository)
        {
            _gamePhrase = phrase;
        }

    }
}
