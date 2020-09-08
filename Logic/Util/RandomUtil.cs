using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Logic.Util
{
    public class RandomUtil
    {
        public static int GetNextInt32(RNGCryptoServiceProvider rnd)
        {
            byte[] randomInt = new byte[4];
            rnd.GetBytes(randomInt);
            return Convert.ToInt32(randomInt[0]);
        }

        public static IEnumerable<T> GetShuffleArray<T>(RNGCryptoServiceProvider rnd, List<T> oldArray) 
        {
            return oldArray.OrderBy(x => GetNextInt32(rnd));
        }

        public static IEnumerable<T> GetShuffleArray<T>(List<T> oldArray)
        {
            return GetShuffleArray(new RNGCryptoServiceProvider(), oldArray);
        }
    }
}
