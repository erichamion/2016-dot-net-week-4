using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poker.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Player.Tests
{
    [TestClass()]
    public class Player_Tests
    {
        [TestMethod()]
        public void TryBet_WithValidParameters_SucceedsAndSubtractsCash()
        {
            // Arrange variables
            double initialCash = 3.00;
            double betAmount = 2.99;
            Player player = new Player("foo", initialCash);
            bool result;
            double remainingCash;

            // Act
            result = player.TryBet(betAmount);
            remainingCash = player.Cash;

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(0.01, remainingCash, 0.00001);
        }

        [TestMethod()]
        public void TryBet_GreaterThanBalance_FailsAndDoesNotSubtractCash()
        {
            // Arrange variables
            double initialCash = 3.00;
            double betAmount = 3.01;
            Player player = new Player("foo", initialCash);
            bool result;
            double remainingCash;

            // Act
            result = player.TryBet(betAmount);
            remainingCash = player.Cash;

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(initialCash, remainingCash, 0.00001);
        }

        [TestMethod()]
        public void CollectWinnings_ValidParameters_AddsCash()
        {
            // Arrange variables
            double initialCash = 3.00;
            double awardAmount = 2.99;
            double expectedBalance = initialCash + awardAmount;
            Player player = new Player("foo", initialCash);
            double newBalance;

            // Act
            player.CollectWinnings(awardAmount);
            newBalance = player.Cash;

            // Assert
            Assert.AreEqual(expectedBalance, newBalance, 0.00001);
        }
    }
}