using System.Collections;

namespace IEEE754
{
    internal class Program
    {
        static void Main(string[] args)
        {
            IEEE754 test = new(0.14329485249857f);
            test.Print();
            //foreach (var a in BinaryConverter.ToBinary(-325.34f))
            //{
            //    Console.Write(a ? "1" : "0");
            //}
        }
    }
}
