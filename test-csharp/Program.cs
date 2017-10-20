using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace test_csharp
{
    class Program
    {
        static void Main(string[] args)
        {
            int a, b;
            var x = "y";

            while (x != "n")
                {

                Console.WriteLine("Введите первое число:");
                a = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Введите второе число:");
                b = Convert.ToInt32(Console.ReadLine());
                if (a > b)
                    Console.WriteLine("Первое число больше второго");
                else if (a < b)
                    Console.WriteLine("Второе число больше первого");
                else
                    Console.WriteLine("Числа равны");
                Console.WriteLine("Продолжить? (y/n)");
                x = Console.ReadLine();

            }
                

        }
    }
}
