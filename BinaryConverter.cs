using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEEE754
{
    public static class BinaryConverter
    {
        public const int MAX_SIZE = 32;
        public static bool[] WholePartToBinary(this int numeral)
        {
            if (numeral == 0) return [false];
            bool[] bits = new bool[(int)Math.Log2(numeral) + 1];
            int i = 0;
            foreach (char a in Convert.ToString(numeral, 2))
            {
                bits[i] = a == '1';
                i++;
            }
            return bits;
        }
        public static bool[] FractionPartToBinary(this double numeral)
        {
            if (numeral == 0) return [false];
            List<bool> bits = [];

            while (bits.Count != MAX_SIZE)
            {
                numeral *= 2;
                bits.Add(numeral >= 1);
                if (numeral == 1) break;
                numeral -= Math.Floor(numeral);          
            }
            return [.. bits];
        }

        public static int ToNumeral(this BitArray binary)
        {
            throw new NotImplementedException("Or not");
        }
    }
}
