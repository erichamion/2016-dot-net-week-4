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
    public class Wallet_Tests
    {
        [TestMethod()]
        public void Wallet_Ctor_HasGivenBalance()
        {
            // Arrange variables
            Wallet wallet;
            double expectedBalance = 27.05;

            // Act
            wallet = new Wallet(expectedBalance);

            // Assert
            Assert.AreEqual(expectedBalance, wallet.Balance, 0.00001);
        }

        [TestMethod()]
        public void AddBalance_WithValidParameters_AddsBalance()
        {
            // Arrange variables
            double initialBalance = 37.05;
            double addAmount = 25.73;
            double expectedBalance = initialBalance + addAmount;
            Wallet wallet = new Wallet(initialBalance);

            // Act
            wallet.AddBalance(addAmount);

            // Assert
            Assert.AreEqual(expectedBalance, wallet.Balance, 0.00001);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddBalance_NegativeAmount_ThrowsArgumentOutOfRangeException()
        {
            // Arrange variables
            double initialBalance = 37.05;
            double addAmount = -25.73;
            Wallet wallet = new Wallet(initialBalance);

            // Act
            wallet.AddBalance(addAmount);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        public void Pay_WithValidParameters_SubtractsBalance()
        {
            // Arrange variables
            double initialBalance = 37.05;
            double subtractAmount = 25.73;
            double expectedBalance = initialBalance - subtractAmount;
            Wallet wallet = new Wallet(initialBalance);

            // Act
            wallet.Pay(subtractAmount);

            // Assert
            Assert.AreEqual(expectedBalance, wallet.Balance, 0.00001);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Pay_NegativeAmount_ThrowsArgumentOutOfRangeException()
        {
            // Arrange variables
            double initialBalance = 37.05;
            double subtractAmount = -25.73;
            Wallet wallet = new Wallet(initialBalance);

            // Act
            wallet.Pay(subtractAmount);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentException))]
        public void Pay_GreaterThanBalance_ThrowsArgumentException()
        {
            // Arrange variables
            double initialBalance = 37.05;
            double subtractAmount = 55.73;
            Wallet wallet = new Wallet(initialBalance);

            // Act
            wallet.Pay(subtractAmount);

            // Assert handled by ExpectedException
        }

        [TestMethod()]
        public void CanPay_LessThanBalance_IsTrue()
        {
            // Arrange variables
            double initialBalance = 37.05;
            double payAmount = 37.04;
            Wallet wallet = new Wallet(initialBalance);
            bool canPay;

            // Act
            canPay = wallet.CanPay(payAmount);

            // Assert
            Assert.IsTrue(canPay);
        }

        [TestMethod()]
        public void CanPay_GreaterThanBalance_IsFalse()
        {
            // Arrange variables
            double initialBalance = 37.05;
            double payAmount = 37.06;
            Wallet wallet = new Wallet(initialBalance);
            bool canPay;

            // Act
            canPay = wallet.CanPay(payAmount);

            // Assert
            Assert.IsFalse(canPay);
        }

        [TestMethod()]
        public void ToString_ValidCall_GivesCorrectString()
        {
            // Arrange variables
            double initialBalance = 37.05;
            String expectedString = "$37.05";
            Wallet wallet = new Wallet(initialBalance);
            String resultString;

            // Act
            resultString = wallet.ToString();

            // Assert
            Assert.IsTrue(resultString.Equals(expectedString));
        }
    }
}