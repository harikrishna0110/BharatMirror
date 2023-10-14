using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BharatMirror.Services
{
    static public class ShuffleAdvertisement
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> enumlist, int size)
        {
            Random rnd = new Random();
            var list = enumlist.ToList();
            var res = new T[size];
            for (int i = 0; i < size; i++)
            {
                int j = rnd.Next(i);
                res[i] = res[j];
                res[j] = list[i];
            }
            return res;
        }
    }
}
