﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Game
{
    public class Pot
    {
        public double Award { get; private set; }

        public Pot()
        {
            Award = 0;
        }

        public void Add(double amount)
        {
            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("Cannot add a negative amount to the pot");
            }
            Award += amount;
        }

        public double PayOut()
        {
            double result = Award;
            Award = 0;
            return result;
        }

        public override string ToString()
        {
            return String.Format("{0:C}", Award);
        }
    }
}