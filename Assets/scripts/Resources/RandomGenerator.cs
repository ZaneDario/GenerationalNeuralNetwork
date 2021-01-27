using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.scripts.Resources
{
    class RandomGenerator
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int RandomInt(int min, int max)
        {
            lock (syncLock)
            { // synchronize
                return random.Next(min, max);
            }
        }

        public static float RandomFloat()
        {
            lock (syncLock)
            { // synchronize
                return (float)(random.NextDouble() * 2 - 1);
            }
        }
    }
}
