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
            if (value == 0) return [false];
            List<bool> bools = [];


            return [..bools];
        }
        public static bool[] ToBinary(this int value) {
            if (value == 0) return [false];
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
