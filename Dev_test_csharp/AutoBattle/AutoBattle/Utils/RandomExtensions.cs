using System;

namespace AutoBattle.Utils
{
    public static class RandomExtensions
    {
        public static int GetRandomInt (int min, int max)
        {
            Random rand = new Random();
            int index = rand.Next(min, max);
            return index;
        }
    }
}
