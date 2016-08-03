using Microsoft.VisualStudio.TestTools.UnitTesting;
using Poker.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Game.Tests
{
    [TestClass()]
    public class Pot_Tests
    {
        [TestMethod()]
        public void Pot_Ctor_HasZeroAward()
        {
            // Arrange variables
            Pot pot;
            double expectedAward = 0.0;

            // Act
            pot = new Pot();

            // Assert
            Assert.AreEqual(expectedAward, pot.Award, 0.00001);
        }

        [TestMethod()]
        public void Add_ValidParameters_AddsToAward()
        {
            // Arrange variables
            Pot pot = new Pot();
            double firstAmount = 27.32;
            double secondAmount = 35.74;
            double expectedAward = firstAmount + secondAmount;

            // Act
            pot.Add(firstAmount);
            pot.Add(secondAmount);

            // Assert
            Assert.AreEqual(expectedAward, pot.Award, 0.00001);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Add_NegativeAmount_ThrowsArgumentOutOfRangeException()
        {
            // Arrange variables
            Pot pot = new Pot();
            double amount = -0.01;

            // Act
            pot.Add(amount);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        public void PayOut_ValidCall_ReturnsAwardAmount()
        {
            // Arrange variables
            Pot pot = new Pot();
            double expectedAward = 27.32;
            double actualAward;

            // Act
            pot.Add(expectedAward);
            actualAward = pot.PayOut();

            // Assert
            Assert.AreEqual(expectedAward, actualAward, 0.00001);
        }

        [TestMethod()]
        public void PayOut_ValidCall_ResetsAwardToZero()
        {
            // Arrange variables
            Pot pot = new Pot();
            double addAmount = 27.32;
            double newAward;

            // Act
            pot.Add(addAmount);
            pot.PayOut();
            newAward = pot.Award;

            // Assert
            Assert.AreEqual(0, newAward, 0.00001);
        }

        [TestMethod()]
        public void ToString_ValidCall_GivesCorrectString()
        {
            // Arrange variables
            Pot pot = new Pot();
            double addAmount = 27.32;
            String expectedString = "$27.32";
            String actualString;

            // Act
            pot.Add(addAmount);
            actualString = pot.ToString();

            // Assert
            Assert.IsTrue(actualString.Equals(expectedString));
        }
    }
}