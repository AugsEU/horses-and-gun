﻿using System;

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
			int Rand1 = RandomManager.I.GetIntInRange(1, 6);
			int Rand2 = RandomManager.I.GetIntInRange(1, 6);

			mValue = Math.Min(Rand1, Rand2);
		}
	}
}
