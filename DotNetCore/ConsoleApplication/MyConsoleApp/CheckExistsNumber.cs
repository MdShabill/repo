using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp
{
    public class CheckExistsNumber
    {
        public void CheckNumber()
        {
            //---- Check whether that number exists in array or not

            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Console.Write("Enter Number: ");

            int inputNumber = int.Parse(Console.ReadLine());

            for (int i = 0; i < numbers.Length; i++)
            {
                if (numbers[i] == inputNumber)
                {
                    Console.WriteLine($"{inputNumber} exist in the array.");
                    return;
                }
            }
            Console.WriteLine($"{inputNumber} does not exist in the array.");
        }
    }
}
