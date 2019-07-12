using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            object oo = 5;
            Console.WriteLine($"yab {oo}");

            Func<int> q = () => (int)oo;

            Console.WriteLine($"yab {q()}");

            oo = 44;

            Console.WriteLine($"yab {q()}");

            var yuk = Console.ReadKey();
        }
    }
}
