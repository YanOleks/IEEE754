using System.Collections;

namespace IEEE754
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IEEE754 test = new(double.NaN);
            test.Print();
        }
    }
}
