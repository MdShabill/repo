using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MyConsoleApp
{
    public class NumericHelper
    {
        public void ValidateGivenNumberExistsInArray()
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

        //public void GetFirstIndexForGivenNumberInAnArray(int inputNumber)
        //{
        //    int[] numbers = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 10 };
        //    for (int i = 0; i < numbers.Length; i++)
        //    {
        //        if (numbers[i] == inputNumber)
        //        {
        //            Console.WriteLine($"{inputNumber} exists in the index {i}.");
        //            return;
        //        }
        //    }

        //    Console.WriteLine($"{inputNumber} does not exist in the array.");
        //}

        //public void ValidateOddandEvenForArrayElements()
        //{
        //    ////identify odd and even number by using loop and array
        //    int[] num = { 7, 8, 19, 7, 8, 15, 16, 12, 5, 8, 9, };
        //    for (int i = 0; i < num.Length; i++)
        //    {
        //        if (num[i] % 2 == 0)
        //        {
        //            Console.WriteLine("\n Even Number:" + num[i]);
        //        }
        //        else
        //        {
        //            Console.WriteLine("\n Odd Number :" + num[i]);
        //        }
        //    }
        //}

        //public void GetPrimeNumbersBetween1To100()
        //{
        //    ////Find out the prime numbers between 1 to 100

        //    for (int number = 1; number <= 100; number++)
        //    {
        //        int count = 0;
        //        for (int i = 1; i <= number; i++)
        //        {
        //            if (number % i == 0)
        //                count++;
        //        }
        //        if (count == 2)
        //        {
        //            Console.WriteLine(number);
        //            Console.WriteLine("\n These are prime numbers");
        //        }
        //    }
        //}

        //public void GetTableForGivenNumber()
        //{
        //    //Give nubmer between 1 to 20 and create Multiplication table

        //    Console.WriteLine("\n Enter a number to get the  Multiplication result");
        //    int num = int.Parse(Console.ReadLine());
        //    if (num > 0 && num <= 20)
        //    {
        //        Console.WriteLine("\n Your Multiplication Is Ready");
        //    }
        //    else
        //    {
        //        Console.WriteLine("\n Please Enter Multiplication Number Between 1 to 20 ");
        //        return;
        //    }
        //    int multiplyResult;
        //    for (int i = 1; i <= 10; i++)
        //    {
        //        multiplyResult = num * i;
        //        Console.WriteLine("{0} X {1} = {2}\n", num, i, multiplyResult);
        //    }
        //}

        //public void GetFibonacciSeriesGivenNumber()
        //{
        //    //Fibonacci series based on user input
        //    int num1 = 0, num2 = 1, num3, num4;
        //    Console.Write("\n Enter Number Of Element : ");
        //    num3 = Convert.ToInt32(Console.ReadLine());

        //    Console.WriteLine("\n Your Fibonacci Series Of " + num3 + " Elements is below");
        //    Console.Write(num1 + " " + num2 + " ");
        //    for (int i = 2; i < num3; i++)
        //    {
        //        num4 = num1 + num2;
        //        Console.Write(num4 + " ");
        //        num1 = num2;
        //        num2 = num4;
        //    }
        //    Console.ReadLine();
        //}

        //public void ReverseAnArray()
        //{
        //    //Check Array number and changed into reverse order
        //    int[] numbers = { 1, 2, 3, 4, 5 };

        //    Console.WriteLine("\n Array in reverse order:");

        //    for (int i = numbers.Length - 1; i >= 0; i--)
        //    {
        //        Console.Write(numbers[i] + " ");
        //    }
        //}

        //public void DisplayFibonacciSeriesBetween1to10()
        //{
        //    //Fabonacci series 1 to 10 elements
        //    int num1 = 0, num2 = 1, num3;

        //    Console.WriteLine("Your Fibonacci Series Of 1 to 10 Elements ");

        //    Console.Write(num1 + " " + num2 + " ");

        //    for (int i = 2; i < 10; i++)
        //    {
        //        num3 = num1 + num2;

        //        Console.Write(num3 + " ");

        //        num1 = num2;
        //        num2 = num3;
        //    }
        //    Console.ReadLine();
        //}

        //public void CheckAnArrayElementIndexPositionAndExistsInArray()
        //{
        //    //// Check whether that number exists in an array or not and the position of that  index number

        //    int[] numbers = { 2, 12, 33, 44, 68, 50, 80, 75, 10, 1 };

        //    Console.Write("Enter Number: ");
        //    int inputNumber = int.Parse(Console.ReadLine());

        //    for (int i = 0; i < numbers.Length; i++)
        //    {
        //        if (numbers[i] == inputNumber)
        //        {
        //            Console.WriteLine($"{inputNumber} exists in the array at index {i}.");
        //            return;
        //        }
        //    }
        //    Console.WriteLine($"{inputNumber} does not exist in the array.");
        //}

        //public void CheckHowManyTimesAnArrayElementFound()
        //{
        //    //// Write a program on a random number and search how many times the number exists in the array element

        //    int[] numbers = { 10, 20, 33, 40, 20, 66, 20, 33, 66, 30, 20 };
        //    Console.Write("Enter Number to search: ");
        //    int inputNumber = int.Parse(Console.ReadLine());

        //    int count = 0;

        //    for (int i = 0; i < numbers.Length; i++)
        //    {
        //        if (numbers[i] == inputNumber)
        //        {
        //            count++;
        //        }
        //    }

        //    if (count > 0)
        //    {
        //        Console.WriteLine($"{inputNumber} This Number Found {count} Times.");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"{inputNumber} This Number is Not Found.");
        //    }
        //}

        //public void CheckHowManyTimesAnArrayElementFoundAndWhichIndexPositions()
        //{
        //    //// Write a program to check if a specific number exists in an array,
        //    //// count how many times it occurs, and find the index positions of those occurrences

        //    int[] numbers = { 15, 10, 25, 30, 10, 15, 60, 70, 80, 10 };

        //    Console.Write("Enter Number to search: ");
        //    int inputNumber = int.Parse(Console.ReadLine());

        //    int count = 0;

        //    if (Array.IndexOf(numbers, inputNumber) != -1)
        //    {
        //        Console.Write($"{inputNumber} found at index: ");
        //        for (int i = 0; i < numbers.Length; i++)
        //        {
        //            if (numbers[i] == inputNumber)
        //            {
        //                count++;
        //                Console.Write($"{i} ");
        //            }
        //        }
        //        Console.WriteLine($"\n{inputNumber} This Number Found {count} Times ");
        //    }
        //    else
        //    {
        //        Console.WriteLine($"\n{inputNumber} This Number is Not Found ");
        //    }
        //}

        //public void GenerateStarPattern()
        //{
        //    ////Write a program to generate a pattern of stars using loop
        //    Console.Write("\n Enter the number of rows for the pattern: ");
        //    int numRows = Convert.ToInt32(Console.ReadLine());

        //    Console.WriteLine("Pattern:");

        //    for (int i = 1; i <= numRows; i++)
        //    {
        //        for (int j = 1; j <= i; j++)
        //        {
        //            Console.Write("* ");
        //        }
        //        Console.WriteLine();
        //    }
        //    Console.ReadLine();
        //}

        //public void GetFactorialNumber()
        //{
        //    ////Write a program to find the factors of a user provided number
        //    Console.Write("\n Enter a number to find the factors: ");
        //    int number = Convert.ToInt32(Console.ReadLine());

        //    Console.WriteLine($"Factors of {number}:");
        //    for (int i = 1; i <= number; i++)
        //    {
        //        if (number % i == 0)
        //        {
        //            Console.WriteLine(i);
        //        }
        //    }
        //}

        //public void GetFactorialNumberWithSum()
        //{
        //    ////Write a program to find the factors of a user provided number and sum of total factor
        //    Console.Write("\n Enter a number to find the factors: ");
        //    int number = Convert.ToInt32(Console.ReadLine());

        //    int sumOfFactors = 0;
        //    Console.WriteLine($"Factors of {number}:");
        //    for (int i = 1; i <= number; i++)
        //    {
        //        if (number % i == 0)
        //        {
        //            Console.WriteLine(i);
        //            //sumOfFactors += i;
        //            sumOfFactors = sumOfFactors + i;
        //        }
        //    }

        //    Console.WriteLine($"Sum of factors of {number}: {sumOfFactors}");
        //}

        //public static void GetUnmatchedElements(int[] array1, int[] array2)
        //{
        //    Console.WriteLine("Array-1: " + string.Join(", ", array1));
        //    Console.WriteLine("Array-2: " + string.Join(", ", array2));

        //    Console.WriteLine("Unmatched elements:");

        //    //// Find unmatched elements in array1
        //    for (int i = 0; i < array1.Length; i++)
        //    {
        //        bool found = false;
        //        for (int j = 0; j < array2.Length; j++)
        //        {
        //            if (array1[i] == array2[j])
        //            {
        //                found = true;
        //                break;
        //            }
        //        }

        //        if (!found)
        //        {
        //            Console.WriteLine(array1[i]);
        //        }
        //    }

        //    // Find unmatched elements in array2
        //    for (int i = 0; i < array2.Length; i++)
        //    {
        //        bool found = false;
        //        for (int j = 0; j < array1.Length; j++)
        //        {
        //            if (array2[i] == array1[j])
        //            {
        //                found = true;
        //                break;
        //            }
        //        }
        //        if (!found)
        //        {
        //            Console.WriteLine(array2[i]);
        //        }
        //    }
        //}

        //public static void GetMatchedElements()
        //{
        //    int[] array1 = { 1, 2, 3 };
        //    int[] array2 = { 4, 3, 2 };

        //    Console.WriteLine("Array1: " + string.Join(", ", array1));
        //    Console.WriteLine("Array2: " + string.Join(", ", array2));

        //    Console.WriteLine("Macthed Element: ");

        //    for (int b = 0; b < array1.Length; b++)
        //    {
        //        for (int a = 0; a < array2.Length; a++)
        //        {
        //            if (array1[b] == array2[a])
        //            {
        //                Console.WriteLine(array1[b]);
        //                break;
        //            }
        //        }
        //    }
        //}

        //public static void GetMatchedElements()
        //{
        //    ////Find matched elements between array1, array2 and array3

        //    int[] array1 = { 1, 2, 3 };
        //    int[] array2 = { 4, 2, 1 };
        //    int[] array3 = { 2, 3, 7 };

        //    Console.WriteLine("Array1: " + string.Join(", ", array1));
        //    Console.WriteLine("Array2: " + string.Join(", ", array2));
        //    Console.WriteLine("Array2: " + string.Join(", ", array3));

        //    Console.WriteLine("Macthed Element: ");

        //    for (int i = 0; i < array1.Length; i++)
        //    {
        //        bool foundInArray2 = false;
        //        for (int j = 0; j < array2.Length; j++)
        //        {
        //            if (array1[i] == array2[j])
        //            {
        //                Console.WriteLine(array1[i]);
        //                foundInArray2 = true;
        //                break;
        //            }
        //        }

        //        if(!foundInArray2)
        //        {
        //            for (int k = 0; k < array3.Length; k++)
        //            {
        //                if ((array1[i] == array3[k]))
        //                {
        //                    Console.WriteLine(array1[i]);
        //                    break;
        //                }
        //            }
        //        }
        //    }
        //}

        //public static void GetAlternateNumbers()
        //{
        //    ////Find The Alternate Array Element

        //    int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        //    Console.WriteLine("This is your Alternate Numbers: ");
        //    for(int i = 0; i < numbers.Length; i = i + 2) 
        //    {
        //        Console.WriteLine(numbers[i]);
        //    }
        //}

        //public static void GetCountAndSumOfMatchedElements()
        //{
        //    // Find total count and sum of matched elements which is not duplicate element in array1 and array2
        //    int[] array1 = { 1, 2, 3, 4, 5, 6 };
        //    int[] array2 = { 7, 4, 3, 2, 2, 8 };

        //    Console.WriteLine("Array1: " + string.Join(", ", array1));
        //    Console.WriteLine("Array2: " + string.Join(", ", array2));

        //    int sumOfMatchedElements = 0;
        //    Console.WriteLine("\nTotal Count of Matching Array Elements:");

        //    string matchedElements = "";
        //    for (int i = 0; i < array1.Length; i++)
        //    {
        //        int count = 0;
        //        for (int j = 0; j < array2.Length; j++)
        //        {
        //            if (array1[i] == array2[j])
        //            {
        //                count++;
        //            }
        //        }
        //        Console.WriteLine($"{array1[i]} - {count}");
        //        if (count > 0)
        //        {
        //            if (!string.IsNullOrEmpty(matchedElements))
        //            {
        //                matchedElements = $"{matchedElements} + ";
        //            }
        //            matchedElements = matchedElements + array1[i];
        //            sumOfMatchedElements = sumOfMatchedElements + array1[i];
        //        }
        //    }
        //    Console.WriteLine($"\nSum Of Matching Elements : {matchedElements} = {sumOfMatchedElements}");
        //}

        public static void GetMatchedIndexAndElements()
        {
            ////Find only those elements which is matched and have same index position between two arrays

            int[] array1 = { 10, 20, 30, 40, 50, 60 };
            int[] array2 = { 10, 30, 20, 40, 50, 10, 70, 10, 60 };

            Console.WriteLine("Array1: " + string.Join(", ", array1));
            Console.WriteLine("Array2: " + string.Join(", ", array2));

            Console.WriteLine("\n Matched Elements at Same Index:");

            for (int i = 0; i < array1.Length; i++)
            {
                if (i < array2.Length && array1[i] == array2[i])
                {
                    Console.WriteLine($"\n Index {i}: {array1[i]}");
                }
            }
        }
    }
}

