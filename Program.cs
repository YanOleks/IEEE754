using System.Collections;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace IEEE754
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            double minAbs = 0.00000000093132257462;
            double max = 4293918720.0;
            double min = -4293918720.0;
            double one = +1.0e0;
            double inf = double.PositiveInfinity;
            double inf2 = double.NegativeInfinity;
            double nonNormal = 0.00000000000022737368;
            double nan = double.NaN;

            //double pastMax = 42939187210.0;

            PrintNumber(minAbs, "\t- мінімальне за абсолютною величиною ненульове представлення");
            PrintNumber(max, "\t- максимальне додатнє представлення");
            PrintNumber(min, "- мінімальне від’ємне преставлення");
            PrintNumber(one, "\t- число +1,0Е0");
            PrintNumber(inf, "\t\t- значення +∞");
            PrintNumber(inf2, "\t\t- значення -∞;");
            PrintNumber(nonNormal, "\t- будь-який варіант для ненормалізованого ЧПТ");
            PrintNumber(nan, "\t\t- будь-який варіант для NaN-значення");
            //PrintNumber(pastMax);

            Calculator();
        }

        static void PrintNumber(double number, string str  = "")
        {
            Console.WriteLine($"{new IEEE754(number)}: {(number).ToString("E5")} {str}");
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
