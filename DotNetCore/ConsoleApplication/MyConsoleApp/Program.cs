using System;

namespace MyConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            NumericHelper numericHelper = new();
            
            ////CheckExistsNumber
            numericHelper.DisplayNumersForAnArrayElements();

            ////CheckFirstOcceurrence
            int[] numbers = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 10 };
            Console.Write("\n Enter the number: ");
            int inputNumber = int.Parse(Console.ReadLine());
            numericHelper.DisplayFirstOcceurrenceByGivenInput(numbers, inputNumber);

            ////Check odd and even number
            numericHelper.ValidateOddandEvenForAnArrayElements();
            Console.WriteLine("\n");

            ////Check prime numbers between 1 to 100
            numericHelper.GetPrimeNumbersBetween1To100();
            Console.WriteLine("\n");

            ////Give nubmer between 1 to 20 and create Multiplication table
            numericHelper.GetTableForGivenNumber();
            Console.WriteLine("\n");

            //Check Fibonacci series
            numericHelper.GetFibonacciSeriesGivenUserInput();

            ////Check Array number and changed into reverse order
            numericHelper.ValidateReverseOrderForAnArrayElements();

            ////Fabonacci series 1 to 10 elements
            numericHelper.DisplayFibonacciSeriesGivenUserInput();
        }
    }
}

//--------------------------------------------------------

//Q: Check whether that number exists in an array or not and the position of that  index number 

//int[] numbers = { 2, 12, 33, 44, 68, 50, 80, 75, 10, 1 };

//Console.Write("Enter Number: ");
//int inputNumber = int.Parse(Console.ReadLine());

//for (int i = 0; i < numbers.Length; i++)
//{
//    if (numbers[i] == inputNumber)
//    {
//        Console.WriteLine($"{inputNumber} exists in the array at index {i}.");
//        return;
//    }
//}
//Console.WriteLine($"{inputNumber} does not exist in the array.");

//Q: Write a program on a random number and search how many times the number exists in the array element

//int[] numbers = { 10, 20, 33, 40, 20, 66, 20, 33, 66, 30, 20 };

//Console.Write("Enter Number to search: ");
//int inputNumber = int.Parse(Console.ReadLine());

//int count = 0;

//for (int i = 0; i < numbers.Length; i++)
//{
//    if (numbers[i] == inputNumber)
//    {
//        count++;
//    }
//}

//if (count > 0)
//{
//    Console.WriteLine($"{inputNumber} This Number Found {count} Times.");
//}
//else
//{
//    Console.WriteLine($"{inputNumber} This Number is Not Found.");
//}



// Write a program on a exists number and searchhow many times number exists and index position of the number in arry?
//int[] numbers = { 15, 10, 25, 30, 10, 15, 60, 70, 80, 10 };

//Console.Write("Enter Number to search: ");
//int inputNumber = int.Parse(Console.ReadLine());

//int count = 0;

//if (Array.IndexOf(numbers, inputNumber) != -1)
//{
//    Console.Write($"{inputNumber} found at index: ");
//    for (int i = 0; i < numbers.Length; i++)
//    {
//        if (numbers[i] == inputNumber)
//        {
//            count++;
//            Console.Write($"{i} ");
//        }
//    }
//    Console.WriteLine($"\n{inputNumber} This Number Found {count} Times ");
//}
//else
//{
//    Console.WriteLine($"\n{inputNumber} This Number is Not Found ");
//}


//Write a pattern  program using loop

//Console.Write("Enter the number of rows for the pattern: ");
//int numRows = Convert.ToInt32(Console.ReadLine());

//Console.WriteLine("Pattern:");

//for (int i = 1; i <= numRows; i++)
//{
//    for (int j = 1; j <= i; j++)
//    {
//        Console.Write("* ");
//    }
//    Console.WriteLine();
//}
//Console.ReadLine();










