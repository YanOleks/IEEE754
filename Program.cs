using System.Collections;

namespace IEEE754
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IEEE754 test = new(0.000000000001);
            test.Print();
        }
    }
}
