using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Cards
{
    public abstract class Hand
    {
        protected List<Card> Cards { get; }

        public Hand()
        {
            Cards = new List<Card>();
        }

        public Card this[int idx] { get { return Cards[idx]; } }

        /// <summary>
        /// Compares the score of this hand to the score of another hand.
        /// </summary>
        /// <param name="other">The hand to compare. </param>
        /// <returns>Negative if this object's score is less than other's score,
        /// positive if this object's score is greater than other's score, or 0
        /// if the scores are equal</returns>
        /// <exception cref="ArgumentException">Derived types are generally 
        /// expected to throw ArgumentException if the type of other does not 
        /// exactly match the type of this. If this's class knows that other's
        /// class has compatible scoring and can be sensibly compared, then
        /// ArgumentException is not required to be thrown.</exception>
        public abstract int CompareHands(Hand other);
    }
}
