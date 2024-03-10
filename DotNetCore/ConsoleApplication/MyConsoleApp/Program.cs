using System;

namespace MyConsoleApp
{
    class Program
    {
        static void Main(string[] args) 
        {
            //CheckExistsNumber
            CheckExistsNumber existsNumber = new CheckExistsNumber();
            existsNumber.CheckNumber();
        }
    }
}

//---Array - Print the first occurrence index of that number----   

//int[] numbers = { 10, 20, 30, 40, 50, 60, 70, 80, 90, 10 };

//Console.Write("Enter the number: ");
//int inputNumber = int.Parse(Console.ReadLine());

//for (int i = 0; i < numbers.Length; i++)
//{
//    if (numbers[i] == inputNumber)
//    {
//        Console.WriteLine($"{inputNumber} exist in the index {i}");
//        return;
//    }
//}

//Console.WriteLine($"{inputNumber} does not exist in the array.");

//identify even and odd number by using loop and array

//int[] num = { 7, 8, 19, 7, 8, 15, 16, 12, 5, 8, 9,};
//for (int i = 0; i < num.Length; i++)
//{
//    if (num[i] % 2 == 0)
//    {
//        Console.WriteLine("\n Even Number: " + num[i]);
//    }
//    else
//    {
//        Console.WriteLine("\n Odd Number :" + num[i]);
//    }
//}


//Find out the prime numbers between 1 to 100
//for(int number = 1; number <= 100; number++)
//{
//    int count = 0;
//    for (int i = 1; i <= number; i++)
//    {
//        if (number % i == 0)
//            count++;
//    }
//    if (count == 2)
//    {
//        Console.WriteLine(number);
//        Console.WriteLine("These are prime numbers");
//    }
//}

//int[] numbers = { 1, 2, 3, 4, 5 };

//Console.WriteLine("Array in reverse order:");

//for (int i = numbers.Length - 1; i >= 0; i--)
//{
//    Console.Write(numbers[i] + " ");
//}

// Write a program for 1 to 20 Multiplication result
//Console.WriteLine("Enter a number to get the  Multiplication result");
//int num = int.Parse(Console.ReadLine());
//if (num > 0 && num <= 20)
//{
//    Console.WriteLine("Your Multiplication Is Ready");
//}
//else
//{
//    Console.WriteLine("Please Enter Multiplication Number Between 1 to 20 ");
//    return;
//}
//int multiplyResult;
//for (int i = 1; i <= 10; i++)
//{
//    multiplyResult = num * i;
//    Console.WriteLine("{0} X {1} = {2}\n", num, i, multiplyResult);
//}

//Fabonacci series based on user input

//int num1 = 0, num2 = 1, num3, num4;
//Console.Write("Enter Number Of Element : ");
//num3 = Convert.ToInt32(Console.ReadLine());

//Console.WriteLine("Your Fibonacci Series Of " + num3 + " Elements is below");
//Console.Write(num1 + " " + num2 + " ");
//for (int i = 2; i < num3; i++)
//{
//    num4 = num1 + num2;
//    Console.Write(num4 + " ");
//    num1 = num2;
//    num2 = num4;
//}
//Console.ReadLine();


//Fabonacci series 1 to 10 elements
//int num1 = 0, num2 = 1, num3;

//Console.WriteLine("Your Fibonacci Series Of 1 to 10 Elements ");

//Console.Write(num1 + " " + num2 + " ");

//for (int i = 2; i < 10; i++)
//{
//    num3 = num1 + num2;

//    Console.Write(num3 + " ");

//    num1 = num2;
//    num2 = num3;
//}
//Console.ReadLine();


//Reverse the array
//int[] numbers = {10, 20, 30, 40, 50 };

//Console.WriteLine("Original array:");
//foreach (int num in numbers)
//{
//    Console.Write(num + " ");
//}
//Console.WriteLine();

//Array.Reverse(numbers);

//Console.WriteLine("Reversed array:");
//foreach (int num in numbers)
//{
//    Console.Write(num + " ");
//}
//Console.WriteLine();
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

//Q: Write a program on a random number and search how many times the number exists in the array

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










