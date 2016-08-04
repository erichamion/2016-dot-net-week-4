using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Game
{
    public class Game
    {
        public const int MIN_PLAYERS = 3;
        public const int MAX_PLAYERS = 4;
        public const double MIN_WALLET_TO_ANTE_RATIO = 3.0;

        public GameState State { get; private set; } = GameState.NOT_STARTED;
        public int? Winner { get; private set; } = null;

        private List<Player.Player> _players;
        private double _ante;
        private int _dealer = -1;



        public Game(int playerCount, String humanPlayerName, double ante, double walletSize)
        {
            if (playerCount < MIN_PLAYERS || playerCount > MAX_PLAYERS)
            {
                throw new ArgumentOutOfRangeException("playerCount", playerCount, "Invalid number of players, must be greater than 2 and less than 5");
            }
            if (ante <= 0.0)
            {
                throw new ArgumentOutOfRangeException("ante", ante, "Ante must be positive");
            }
            if (walletSize / ante < MIN_WALLET_TO_ANTE_RATIO)
            {
                throw new ArgumentException(String.Format("walletSize must be at least {0} times ante", MIN_WALLET_TO_ANTE_RATIO));
            }


            _ante = ante;

            _players = new List<Player.Player>(playerCount);
            Player.Player humanPlayer = new Player.Player(humanPlayerName, walletSize);
            _players.Add(humanPlayer);
            for (int i = 1; i < playerCount; i++)
            {
                String playerName = String.Format("Player {0}", i + 1);
                _players.Add(new Player.Player(playerName, walletSize));
            }
        }

        /// <summary>
        /// Performs the next portion of a round.
        /// </summary>
        /// <param name="playerStates">out. Contains the player states after the activity is performed.</param>
        /// <returns>The new state</returns>
        public GameState AdvanceRound(out List<PlayerState> playerStates)
        {
            switch (State)
            {
                case GameState.NOT_STARTED:
                    return StartRound(out playerStates);

                case GameState.PRE_ANTE:
                    break;

                case GameState.POST_ANTE:
                    break;

                case GameState.PRE_SCORE:
                    break;

                case GameState.POST_SCORE:
                    break;

                case GameState.OVER:
                default:
                    throw new InvalidOperationException("Cannot continue a game after it is over. Start a new game instead.");
            }
            throw new NotImplementedException();
        }

        private GameState StartRound(out List<PlayerState> playerStates)
        {
            _dealer = FindDealer(_dealer);
            Winner = null;

            playerStates = new List<PlayerState>(_players.Count);
            for (int i = 0; i < _players.Count; i++)
            {
                Player.Player player = _players[i];
                bool isHuman = (i == 0);
                playerStates.Add(new PlayerState(player.Name, player.ToString(false), i == _dealer, isHuman, player.IsActive));
            }
            return GameState.PRE_ANTE;
        }

        

        private List<PlayerState> GetPlayerStates(bool showHumanHand, bool showAllHands)
        {
            List<PlayerState> result = new List<PlayerState>(_players.Count);
            for (int i = 0; i < _players.Count; i++)
            {
                Player.Player player = _players[i];
                bool isHuman = (i == 0);
                bool showHand = (isHuman && showHumanHand) || showAllHands;
                result.Add(new PlayerState(player.Name, player.ToString(false), i == _dealer, isHuman, player.IsActive));
            }
        }

        private int FindDealer(int oldDealer)
        {
            int dealer = oldDealer;
            do
            {
                if (++dealer >= _players.Count)
                {
                    dealer = 0;
                }
            } while (!_players[dealer].IsActive);

            return dealer;
        }

        


        public enum GameState
        {
            NOT_STARTED,
            PRE_ANTE,
            POST_ANTE,
            PRE_SCORE,
            POST_SCORE,
            BETWEEN_ROUNDS,
            OVER
        }
    }
}
