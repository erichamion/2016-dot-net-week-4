﻿using System;
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
        private int _ante;
        private Pot _pot = new Pot();
        private Cards.Deck _deck;
        private int _dealer = -1;

        public static String GetNextActionString(GameState state)
        {
            switch (state)
            {
                case GameState.NOT_STARTED:
                    return "start the game";

                case GameState.PRE_ANTE:
                    return "submit antes";

                case GameState.POST_ANTE:
                    return "deal";

                case GameState.PRE_SCORE:
                    return "compare hands";

                case GameState.POST_SCORE:
                    return "start next round or game over";

                case GameState.OVER:
                    return "exit or start a new game";

                default:
                    return "";
            }
        }


        public Game(int playerCount, String humanPlayerName, int ante, int walletSize) : 
            this(playerCount, humanPlayerName, ante, walletSize, new Random()) { }

        public Game(int playerCount, String humanPlayerName, int ante, int walletSize, Random random)
        {
            if (playerCount < MIN_PLAYERS || playerCount > MAX_PLAYERS)
            {
                throw new ArgumentOutOfRangeException("playerCount", playerCount, "Invalid number of players, must be greater than 2 and less than 5");
            }
            if (ante < 1)
            {
                throw new ArgumentOutOfRangeException("ante", ante, "Ante must be positive and nonzero");
            }
            if ((double) walletSize / ante < MIN_WALLET_TO_ANTE_RATIO)
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

            _deck = new Cards.Deck(random);
        }

        /// <summary>
        /// Performs the next portion of a round.
        /// </summary>
        /// <param name="playerStates">out. Contains the player states after the activity is performed.</param>
        /// <returns>The new state</returns>
        public GameState AdvanceRound(out List<PlayerState> playerStates)
        {
            GameState newState;
            switch (State)
            {
                case GameState.NOT_STARTED:
                    newState = StartRound(out playerStates);
                    break;

                case GameState.PRE_ANTE:
                    newState = DoAnte(out playerStates);
                    break;

                case GameState.POST_ANTE:
                    newState = Deal(out playerStates);
                    break;

                case GameState.PRE_SCORE:
                    newState = ScorePlayers(out playerStates);
                    break;

                case GameState.POST_SCORE:
                    newState = StartNewRoundOrGameOver(out playerStates);
                    break;

                case GameState.OVER:
                default:
                    throw new InvalidOperationException("Cannot continue a game after it is over. Start a new game instead.");
            }
            State = newState;
            return newState;
        }

        private GameState StartRound(out List<PlayerState> playerStates)
        {
            _deck.Reshuffle();
            _dealer = FindDealer(_dealer);
            Winner = null;

            playerStates = GetPlayerStatesNoHands();
            return GameState.PRE_ANTE;
        }

        private GameState DoAnte(out List<PlayerState> playerStates)
        {
            foreach (Player.Player player in _players.Where(x => x.IsActive))
            {
                // TODO: Consider making Player.TryBet return the amount of the
                // bet if successful, or 0 if unsuccessful. That would simplify
                // this to "_pot.Add(player.TryBet(_ante));"
                if (player.TryBet(_ante))
                {
                    _pot.Add(_ante);
                }
            }

            // player.TryBet deactivates any players who can't meet the ante.

            playerStates = GetPlayerStatesNoHands();
            return GameState.POST_ANTE;
        }

        private GameState Deal(out List<PlayerState> playerStates)
        {
            foreach (Player.Player player in _players.Where(x => x.IsActive))
            {
                Cards.Card[] cards = new Cards.Card[5];
                for (int i = 0; i < 5; i++)
                {
                    cards[i] = _deck.NextCard;
                }

                player.CreateHand(cards);
            }

            playerStates = GetPlayerStates(true, false);
            return GameState.PRE_SCORE;
        }

        private GameState ScorePlayers(out List<PlayerState> playerStates)
        {
            Player.Player winningPlayer = _players.Where(x => x.IsActive).OrderBy(x => x.Hand).Last();
            Winner = _players.IndexOf(winningPlayer);
            winningPlayer.CollectWinnings(_pot.PayOut());

            playerStates = GetPlayerStates(true, true);
            return GameState.POST_SCORE;
        }

        private GameState StartNewRoundOrGameOver(out List<PlayerState> playerStates)
        {
            // How many active players remain?
            if (_players.Count(x => x.IsActive) > 1)
            {
                // Multiple players remaining, continue to the next round
                return StartRound(out playerStates);
            }


            // No more than one remaining player, game over
            playerStates = GetPlayerStatesNoHands();
            return GameState.OVER;
        }

        
        private List<PlayerState> GetPlayerStatesNoHands()
        {
            return GetPlayerStates(false, false, false);
        }

        private List<PlayerState> GetPlayerStates(bool showHumanHand, bool showAllHands)
        {
            return GetPlayerStates(true, showHumanHand, showAllHands);
        }

        private List<PlayerState> GetPlayerStates(bool playersHaveHands, bool showHumanHand, bool showAllHands)
        {
            List<PlayerState> result = new List<PlayerState>(_players.Count);
            for (int i = 0; i < _players.Count; i++)
            {
                Player.Player player = _players[i];
                bool isHuman = (i == 0);

                String desc;
                if (playersHaveHands && player.IsActive)
                {
                    bool showHand = (isHuman && showHumanHand) || showAllHands;
                    desc = showHand ? 
                        player.ToString(true) : 
                        String.Format("{0} (Hand Hidden)", player.ToString(false));
                }
                else
                {
                    desc = player.ToString(false);
                }

                result.Add(new PlayerState(player.Name, desc, i == _dealer, isHuman, player.IsActive));
            }
            return result;
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

        


        
    }
}
