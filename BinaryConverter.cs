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
        public static bool[] ToBinary(this float value)
        {
            int bitCount = sizeof(float) * 8;
            bool[] result = new bool[bitCount]; 

            int intValue = BitConverter.ToInt32(BitConverter.GetBytes(value), 0);

            //if (value < 0) result[bitCount - 1] = true;
            for (int bit = 0; bit < bitCount ; ++bit)
            {
                int maskedValue = intValue & (1 << bit);
                result[bit] = maskedValue != 0; 
            }

            return result;
        }
        public static bool[] ToBinary(this int value) {
            List<bool> bools = [];
            value = Math.Abs(value);
            while (value > 0)
            {
                bools.Add(value % 2 == 1);
                value /= 2;
            }
            return [.. bools];
        }

        public static int ToNumeral(this BitArray binary)
        {
            throw new NotImplementedException("Or not");
        }
    }
}
