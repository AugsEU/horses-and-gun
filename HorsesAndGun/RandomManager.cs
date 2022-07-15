using System;
using System.Collections.Generic;
using System.Text;

namespace HorsesAndGun
{
    internal class RandomManager : Singleton<RandomManager>
    {
        Random mRandom = new Random();

        public int GetIntInRange(int low, int high)
        {
            return mRandom.Next(low, high + 1);
        }

    }
}
