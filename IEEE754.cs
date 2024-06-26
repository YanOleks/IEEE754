﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace IEEE754
{
    internal class IEEE754
    {
        #region Fields
        bool[] _array = new bool[size];
        decimal _decimal;
        bool _implicitBit = true;
        int _bias = 0;

        bool _isNormal = true;

        public const int EXPONENT_SIZE = 6;
        public const int MANTISSA_SIZE = 11;

        public const int size = EXPONENT_SIZE + MANTISSA_SIZE + 1;
        #endregion

        #region Properties
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
            get => _array[1..(EXPONENT_SIZE + 1)];
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
                foreach (var a in value)
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

        private int exp
        {
            get => _bias + (int)Math.Pow(2, EXPONENT_SIZE - 1) - 1;
        }
        private int maxBias
        {
            get => (int)Math.Pow(2, EXPONENT_SIZE - 1) - 2;
        }
        #endregion

        public IEEE754(double value)
        {
            Sign = double.IsNegative(value);
            if (value == 0)
            {
                Exponent = [.. Enumerable.Repeat(false, EXPONENT_SIZE)];
                Mantissa = [.. Enumerable.Repeat(false, MANTISSA_SIZE)];
                return;
            }
            if (double.IsInfinity(value))
            {
                _implicitBit = false;
                Exponent = [.. Enumerable.Repeat(true, EXPONENT_SIZE)];
                return;
            }
            if (double.IsNaN(value))
            {
                Exponent = [.. Enumerable.Repeat(true, EXPONENT_SIZE)];
                Mantissa = [true];
                return;
            }
            Mantissa = ToBinary(value);
            Exponent = ToBinary(exp);
        }

        #region Private Functions
        private bool IsInf()
        {
            //throw new NotImplementedException();
            return _bias > maxBias + 1;
        }
        private bool IsNonNormal()
        {
            _isNormal = !(_bias < -(maxBias));
            return !_isNormal;
        }
        private void Clear()
        {
            for (int i = 0; i < size; i++)
            {
                _array[i] = false;
            }
        }

        private bool[] ToBinary(double value)
        {
            if (value == 0) return [.. Enumerable.Repeat(false, MANTISSA_SIZE)];
            value = Math.Abs(value);

            List<bool> wholePart = WholeToBinary(value);
            int limit = wholePart.Count > 0 ? MANTISSA_SIZE - wholePart.Count + 2 : -1;
            List<bool> fractionPart = FractionToBinary(value, limit);
            List<bool> binary = new(wholePart.Concat(fractionPart));

            if (wholePart.Count > 0)
            {
                _bias = wholePart.Count - 1;
                binary = new(binary[1..]);
            }
            else
            {
                int firstOne = fractionPart.IndexOf(true);
                _bias = -(firstOne + 1);
                if (IsNonNormal())
                {
                    ImplicitBit = false;
                    if (binary[^1])
                    {
                        int i;
                        for (i = binary.Count - 2; i >= 0; i--)
                        {
                            if (binary[i])
                            {
                                binary[i] = false;
                                continue;
                            }
                            binary[i] = true;
                            break;
                        }
                        if (i == -1)
                        {
                            _bias++;
                        }
                    }
                    return [.. binary[maxBias..(maxBias + MANTISSA_SIZE)]];
                }
                binary = new(binary[(firstOne + 1)..]);
            }
            if (IsInf()) return [.. Enumerable.Repeat(false, MANTISSA_SIZE)];

            if (binary.Count > MANTISSA_SIZE)
            {
                if (binary[MANTISSA_SIZE] == true)
                {
                    int i;
                    for (i = MANTISSA_SIZE - 1; i >= 0; i--)
                    {
                        if (binary[i])
                        {
                            binary[i] = false;
                            continue;
                        }
                        binary[i] = true;
                        break;
                    }
                    if (i == -1)
                    {
                        _bias++;
                    }
                }
                binary.RemoveRange(MANTISSA_SIZE, binary.Count - MANTISSA_SIZE);
            }
            else
            {
                binary.AddRange(Enumerable.Repeat(false, MANTISSA_SIZE - binary.Count));
            }

            return [.. binary];

        }
        private List<bool> WholeToBinary(double value)
        {
            double wholePart = Math.Truncate(value);
            if (wholePart == 0) return [];
            List<bool> result = [];
            while (wholePart > 0)
            {
                result.Add(wholePart % 2 == 1);
                wholePart = Math.Truncate(wholePart / 2);
            }
            result.Reverse();
            return result;
        }
        private List<bool> FractionToBinary(double value, int limit)
        {
            double fraction = value - Math.Truncate(value);
            if (fraction == 0) return [];
            List<bool> result = [];
            while (fraction != 1.0 && (result.Count < limit || limit == -1))
            {
                fraction *= 2;
                if (fraction >= 1)
                {
                    if (limit == -1)
                    {
                        limit = MANTISSA_SIZE + maxBias + 1;
                    }
                    fraction -= Math.Truncate(fraction);
                    result.Add(true);
                    continue;
                }
                result.Add(false);
            }
            return result;
        }

        private bool[] ToBinary(int value)
        {
            if (value == 0 || !_isNormal) return [.. Enumerable.Repeat(false, EXPONENT_SIZE)];
            if (IsInf()) return [.. Enumerable.Repeat(true, EXPONENT_SIZE)];
            List<bool> bools = [];
            value = Math.Abs(value);
            while (value > 0)
            {
                bools.Add(value % 2 == 1);
                value /= 2;
            }
            bools.Reverse();
            return [.. bools];
        }
        #endregion

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.Append(Sign ? "1 " : "0 ");
            foreach (var i in Exponent)
            {
                sb.Append(i ? "1" : "0");
            }
            sb.Append(_implicitBit ? " 1 " : " 0 ");
            int j = 0;
            foreach (var i in Mantissa)
            {
                j++;
                sb.Append(i ? "1" : "0");
                if (j % 4 == 0)
                {
                    j = 0;
                    sb.Append(' ');
                }
            }
            return sb.ToString();
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
            Console.Write(_implicitBit ? "1 " : "0 ");
            int j = 0;
            foreach (var i in Mantissa)
            {
                j++;
                Console.Write(i ? "1" : "0");
                if (j % 4 == 0)
                {
                    j = 0;
                    Console.Write(" ");
                }
            }
        }
    }
}
