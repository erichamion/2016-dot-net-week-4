using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Program
    {
        static void Main(string[] args)
        {
            Cards.Card card = new Cards.Card(Cards.CardRanks._10, Cards.Suits.D);
            Console.WriteLine(card);

            List<Cards.Card> cards = new List<Cards.Card>();
            Console.ReadKey();
        }
    }
}
