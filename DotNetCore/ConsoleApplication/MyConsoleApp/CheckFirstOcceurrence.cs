using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp
{
    public class CheckFirstOcceurrence
    {
        public void ArrayFirstOcceurrence(int[] numbers, int inputNumber)
        {
            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] == inputNumber)
                {
                    Console.WriteLine($"{inputNumber} exists in the index {i}.");
                    return;
                }
            }

            Console.WriteLine($"{inputNumber} does not exist in the array.");
        }
    }
}
