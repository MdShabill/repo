using System;

namespace MyConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            NumericHelper numericHelper = new();

            ////CheckExistsNumber
            numericHelper.ValidateGivenNumberExistsInArray();
            Console.WriteLine("\n");

            ////CheckFirstOcceurrence
            Console.Write("\n Enter the number: ");
            int inputNumber = int.Parse(Console.ReadLine());
            numericHelper.GetFirstIndexForGivenNumberInAnArray(inputNumber);

            //////Check odd and even number
            numericHelper.ValidateOddandEvenForArrayElements();
            Console.WriteLine("\n");

            //////Check prime numbers between 1 to 100
            numericHelper.GetPrimeNumbersBetween1To100();
            Console.WriteLine("\n");

            //////Give nubmer between 1 to 20 and create Multiplication table
            numericHelper.GetTableForGivenNumber();
            Console.WriteLine("\n");

            ////Check Fibonacci series
            numericHelper.GetFibonacciSeriesGivenNumber();
            Console.WriteLine("\n");

            //////Check Array number and changed into reverse order
            numericHelper.ReverseAnArray();
            Console.WriteLine("\n");

            //////Fabonacci series 1 to 10 elements
            numericHelper.DisplayFibonacciSeriesBetween1to10();
            Console.WriteLine("\n");

            //////Check whether that number exists in an array or not and the position of that index number
            numericHelper.CheckAnArrayElementIndexPositionAndExistsInArray();
            Console.WriteLine("\n");

            ////// Write a program on a random number and search how many times the number exists in the array element
            numericHelper.CheckHowManyTimesAnArrayElementFound();
            Console.WriteLine("\n");

            ////// Write a program to check if a specific number exists in an array,
            ////// count how many times it occurs, and find the index positions of those occurrences
            numericHelper.CheckHowManyTimesAnArrayElementFoundAndWhichIndexPositions();
            Console.WriteLine("\n");

            //////Write a program to generate a pattern of stars using loop
            numericHelper.GenerateStarPattern();
            Console.WriteLine("\n");

            ////Write a program to find the factors of a user-provided number
            numericHelper.GetFactorialNumber();
            Console.WriteLine("\n");

            ////Write a program to find the factors of a user-provided number and sum of total factor
            numericHelper.GetFactorialNumberWithSum();
            Console.WriteLine("\n");

            ////Find unmatched elements in array1 and array2
            NumericHelper.GetUnmatchedElements();
            Console.WriteLine("\n");

            ////Find matched elements between array1 and array2
            NumericHelper.GetMatchedElementsBetweenTwoArrays();
            Console.WriteLine("\n");

            ////Find matched elements between array1, array2 and array3
            NumericHelper.GetMatchedElementsBetweenThreeArrays();
            Console.WriteLine("\n");

            ////Find The Alternate Array Element
            NumericHelper.GetAlternateNumbers();
            Console.WriteLine("\n");

            ////Find total count and sum of matched elements in array1 and array2
            NumericHelper.GetCountAndSumOfMatchedElements();
            Console.WriteLine("\n");

            ////Find only those elements which is matched and have same index position between two arrays
            NumericHelper.GetMatchedIndexAndElements();
        }
    }
}
