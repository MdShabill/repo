using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp
{
    public class CheckPrimeNumber
    {
        public void PrimeNumber()
        {
            //Find out the prime numbers between 1 to 100

            for (int number = 1; number <= 100; number++)
            {
                int count = 0;
                for (int i = 1; i <= number; i++)
                {
                    if (number % i == 0)
                        count++;
                }
                if (count == 2)
                {
                    Console.WriteLine(number);
                    Console.WriteLine("\n These are prime numbers");
                }
            }
        }
    }
}
