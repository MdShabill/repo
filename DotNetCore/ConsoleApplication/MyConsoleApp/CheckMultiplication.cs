using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp
{
    public class CheckMultiplication
    {
        public void Multiplication() 
        {
            //Write a program for 1 to 20 Multiplication result

            Console.WriteLine("\n Enter a number to get the  Multiplication result");
            int num = int.Parse(Console.ReadLine());
            if (num > 0 && num <= 20)
            {
                Console.WriteLine("\n Your Multiplication Is Ready");
            }
            else
            {
                Console.WriteLine("\n Please Enter Multiplication Number Between 1 to 20 ");
                return;
            }
            int multiplyResult;
            for (int i = 1; i <= 10; i++)
            {
                multiplyResult = num * i;
                Console.WriteLine("{0} X {1} = {2}\n", num, i, multiplyResult);
            }
        }
    }
}
