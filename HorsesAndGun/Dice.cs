using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace HorsesAndGun
{
    internal class Dice
    {
        int mValue;

        public Dice(int _value)
        {
            mValue = _value;
        }

        public Dice(Dice _dice)
        {
            mValue = _dice.mValue;
        }

        public Dice()
        {
            Roll();
        }


        public int Value
        {
            get { return mValue; }
            set
            {
                mValue = value;
            }
        }

        public void Roll()
        {
            mValue = RandomManager.I.GetIntInRange(1, 6);
        }
    }
}
