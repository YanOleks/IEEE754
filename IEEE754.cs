using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEEE754
{
    internal class IEEE754
    {
        bool[] _array = new bool[size];
        decimal _decimal;
        bool _implicitBit = true;


        public const int EXPONENT_SIZE = 6;
        public const int MANTISSA_SIZE = 11;

        public const int size = EXPONENT_SIZE + MANTISSA_SIZE + 1;

        public decimal Decimal
        {
            get => _decimal;
            set
            {
                _decimal = value;
                Sign = Math.Sign(value);
                throw new NotImplementedException("Needs converting");               
            }
        }
        public int Sign 
        {
            get => _array[0]?-1:1;
            private set => _array[0] = Math.Sign(value) == -1;
        }
        public int ImplicitBit
        {
            get => _implicitBit ? 1 : 0;
            private set => _implicitBit = value == 1;
        }
        public string Exponent
        {
            get;set;
        }
        public string Mantissa
        {
            get;set;
        }


    }
}
