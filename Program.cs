﻿using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IEEE754
{
    internal class Program
    {
        static void Main(string[] args)
        {
            double minAbs = 0.00000000093132257462;
            double max = 4293918720.0;
            double min = -4293918720.0;
            double one = +1.0e0;
            double inf = double.PositiveInfinity;
            double inf2 = double.NegativeInfinity;
            double nonNormal = 0.00000000000022737368;
            double nan = double.NaN;

            //double pastMax = 42939187210.0;

            PrintNumber(minAbs);
            PrintNumber(max);
            PrintNumber(min);
            PrintNumber(one);
            PrintNumber(inf);
            PrintNumber(inf2);
            PrintNumber(nonNormal);
            PrintNumber(nan);
            //PrintNumber(pastMax);

            Calculator();
        }

        static void PrintNumber(double number )
        {
            Console.WriteLine($"{new IEEE754(number)}: {(number).ToString("E5")}");
        }
        static void Calculator()
        {
            Console.WriteLine();
            Console.Write("Enter value: ");
            string? value = Console.ReadLine();
            if (Double.TryParse(value, out double number))            
                PrintNumber(number);
            else
                Console.WriteLine("Unable to parse '{0}'.", value);
        }
    }
}
