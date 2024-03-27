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


        public const int EXPONENT_SIZE = 8;
        public const int MANTISSA_SIZE = 23;

        public const int size = EXPONENT_SIZE + MANTISSA_SIZE + 1;

        public decimal Decimal
        {
            get => _decimal;
            set
            {
                _decimal = value;
                Sign = Math.Sign(value) == -1;
                throw new NotImplementedException("Needs converting");               
            }
        }
        public bool Sign 
        {
            get => _array[0];
            private set => _array[0] = value;
        }
        public bool ImplicitBit
        {
            get => _implicitBit;
            private set => _implicitBit = value;
        }
        public bool[] Exponent
        {
            get => _array[1..EXPONENT_SIZE];
            private set
            {
                if (value.Length > EXPONENT_SIZE)
                    throw new ArgumentOutOfRangeException($"Max lenght is ${EXPONENT_SIZE}");
                if (value.Length < EXPONENT_SIZE)
                {
                    int i = 1;
                    for (; i <= EXPONENT_SIZE - value.Length; i++)
                    {
                        _array[i] = false;
                    }
                    foreach (var a in value)
                    {
                        _array[i] = a;
                        i++;
                    }
                    return;
                }
                int j = 1;
                foreach(var a in value)
                {
                    _array[j] = a;
                    j++;
                }
            }
        }
        public bool[] Mantissa
        {
            get => _array[(1 + EXPONENT_SIZE)..size];
            private set
            {
                if (value.Length > MANTISSA_SIZE)
                    throw new ArgumentOutOfRangeException($"Max lenght is ${MANTISSA_SIZE}");
                int i = 1 + EXPONENT_SIZE;
                foreach (var v in value)
                {
                    _array[i++] = v;
                }
            }
        }

        public IEEE754(float value)
        {
            ImplicitBit = true;
            if (value == 0)
            {
                Clear();
                return;
            }
            bool[] binary = BinaryConverter.ToBinary(value);
            Sign = binary[0];            
            int bias = GetBias(binary);           
            bool[] mantissa = binary[0..23];//(23 - MANTISSA_SIZE - 1)
            if (mantissa[0] == true)
            {
                int i;
                for (i = 1;i < mantissa.Length;i++)
                {
                    if (!mantissa[i])
                    {
                        mantissa[i] = true;
                        break;

                    }
                    mantissa[i] = false;
                }
                if (i == mantissa.Length)
                {
                    bias++;
                }
            }
            if (IsInf(bias))
            {
                int i;
                for (i = 1; i <= EXPONENT_SIZE; i++)
                {
                    _array[i] = true;
                }
                for (; i < size; i++)
                {
                    _array[i] = false;
                }
                return;
            }
            int exponent = bias + (int)Math.Pow(2, EXPONENT_SIZE - 1) - 1;
            bool[] binaryExponent = BinaryConverter.ToBinary(exponent);
            Array.Reverse(binaryExponent);
            Exponent = binaryExponent;
            Array.Reverse(mantissa);
            Mantissa = mantissa[0..MANTISSA_SIZE];
        }

        private int GetBias(bool[] bools)
        {
            BitArray expBinary = new(bools[23..31]);
            if (expBinary.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            expBinary.CopyTo(bytes, 0);
            return bytes[0] - 127;
        }
        private bool IsInf(int bias) 
        {
            return bias >= Math.Pow(2, EXPONENT_SIZE - 1) ;
        }
        private void Clear()
        {
            for (int i = 0; i < size; i++)
            {
                _array[i] = false;
            }
        }

        public string GetString()
        {
            char[] chars = new char[size];
            for ( int i = 0; i < size; i++ )
            {
                chars[i] = _array[i]?'1':'0';
            }
            return new string(chars);
        }
        public void Print()
        {

            Console.Write(Sign?"1":"0");
            Console.Write(" ");
            foreach (var i in Exponent)
            {
                Console.Write(i ? "1" : "0");
            }
            Console.Write(" ");
            foreach (var i in Mantissa)
            {
                Console.Write(i ? "1" : "0");
            }
        }
    }
}
