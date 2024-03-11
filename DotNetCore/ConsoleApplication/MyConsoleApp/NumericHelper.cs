using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp
{
    public class NumericHelper
    {
        public void DisplayNumersForAnArrayElements()
        {
            ////---- Check whether that number exists in array or not

            int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            Console.Write("\n Enter Number: ");

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

        public void DisplayFirstOcceurrenceByGivenInput(int[] numbers, int inputNumber)
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

        public void ValidateOddandEvenForAnArrayElements()
        {
            ////identify odd and even number by using loop and array
            int[] num = { 7, 8, 19, 7, 8, 15, 16, 12, 5, 8, 9, };
            for (int i = 0; i < num.Length; i++)
            {
                if (num[i] % 2 == 0)
                {
                    Console.WriteLine("\n Even Number:" + num[i]);
                }
                else
                {
                    Console.WriteLine("\n Odd Number :" + num[i]);
                }
            }
        }

        public void GetPrimeNumbersBetween1To100()
        {
            ////Find out the prime numbers between 1 to 100

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

        public void GetTableForGivenNumber()
        {
            //Give nubmer between 1 to 20 and create Multiplication table

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

        public void GetFibonacciSeriesGivenUserInput()
        {
            //Fibonacci series based on user input
            int num1 = 0, num2 = 1, num3, num4;
            Console.Write("\n Enter Number Of Element : ");
            num3 = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine("\n Your Fibonacci Series Of " + num3 + " Elements is below");
            Console.Write(num1 + " " + num2 + " ");
            for (int i = 2; i < num3; i++)
            {
                num4 = num1 + num2;
                Console.Write(num4 + " ");
                num1 = num2;
                num2 = num4;
            }
            Console.ReadLine();
        }

        public void ValidateReverseOrderForAnArrayElements()
        {
            //Check Array number and changed into reverse order
            int[] numbers = { 1, 2, 3, 4, 5 };

            Console.WriteLine("\n Array in reverse order:");

            for (int i = numbers.Length - 1; i >= 0; i--)
            {
                Console.Write(numbers[i] + " ");
            }
        }

        public void DisplayFibonacciSeriesGivenUserInput()
        {
            //Fabonacci series 1 to 10 elements
            int num1 = 0, num2 = 1, num3;

            Console.WriteLine("Your Fibonacci Series Of 1 to 10 Elements ");

            Console.Write(num1 + " " + num2 + " ");

            for (int i = 2; i < 10; i++)
            {
                num3 = num1 + num2;

                Console.Write(num3 + " ");

                num1 = num2;
                num2 = num3;
            }
            Console.ReadLine();
        }
    }
}
