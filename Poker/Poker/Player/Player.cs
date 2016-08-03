using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Player
{
    public class Player
    {
        public Cards.PokerHand Hand { get; private set; } = null;
        public bool IsActive { get; private set; } = true;
        public String Name { get; }
        public double Cash { get { return _wallet.Balance; } }

        private Wallet _wallet;

        public Player(String name, double initialCash)
        {
            _wallet = new Wallet(initialCash);
            Name = name;
        }

        public void CreateHand(params Cards.Card[] cards)
        {
            Hand = new Cards.PokerHand(cards);
        }

        public bool TryBet(double amount)
        {
            if (_wallet.CanPay(amount))
            {
                _wallet.Pay(amount);
                return true;
            }
            else
            {
                IsActive = false;
                return false;
            }
        }

        public void CollectWinnings(double amount)
        {
            _wallet.AddBalance(amount);
        }


    }
}
